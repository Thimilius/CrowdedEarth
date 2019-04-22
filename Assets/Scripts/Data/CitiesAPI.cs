using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using CrowdedEarth.Data.Model;
using Newtonsoft.Json;

namespace CrowdedEarth.Data {
    public class CitiesAPI : MonoBehaviour, ICitiesAPI {
        private class ResponseMetaData {
            public int CurrentOffset { get; set; }
            public int TotalCount { get; set; }
        }

        private class ResponseLink {
            public string Rel { get; set; }
            public string Href { get; set; }
        }

        private class Response<T> {
            public T Data { get; set; }
            public List<ResponseLink> Links { get; set; }
            public ResponseMetaData MetaData { get; set; }
        }

        private class City : ICity {
            public int ID { get; set; }
            public string Name { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }

        private class CityDetails : ICityDetails {
            public string Name { get; set; }
            public string Country { get; set; }
            public string Region { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
            public int Population { get; set; }
            public int ElevationMeters { get; set; }
        }

        private const string BASE_URL = "http://geodb-free-service.wirefreethought.com";

        public void GetCities(int minPopulation, GetCitiesHandler callback) {
            List<City> cities = new List<City>();

            void DoRequest(string url) {
                MakeRequest<List<City>>(url, (response, success) => {
                    if (success) {
                        cities.AddRange(response.Data);

                        ResponseLink link = response.Links.Find(l => l.Rel == "next");
                        if (link != null) {
                            DoRequest(link.Href);
                        } else {
                            callback?.Invoke(cities, success); 
                        }
                    } else {
                        callback?.Invoke(cities, success);
                    }
                });
            }

            DoRequest($"/v1/geo/cities?limit=5&offset=0&minPopulation={minPopulation}");
        }

        public void GetCityDetails(int cityID, GetCityDetailsHandler callback) {
            MakeRequest<CityDetails>($"/v1/geo/cities/{cityID}", (response, success) => callback?.Invoke(response.Data, success));
        }

        private void MakeRequest<T>(string url, Action<Response<T>, bool> callback) {
            UnityWebRequest request = UnityWebRequest.Get($"{BASE_URL}{url}");
            request.SendWebRequest().completed += operation => {
                if (request.isHttpError || request.isNetworkError) {
                    Debug.LogError($"[CitiesAPI] - Request failed with error ({request.responseCode}): {request.error}");
                    callback?.Invoke(null, false);
                } else {
                    Response<T> response = JsonConvert.DeserializeObject<Response<T>>(request.downloadHandler.text);
                    callback?.Invoke(response, true);
                }
            };
        }
    }
}
