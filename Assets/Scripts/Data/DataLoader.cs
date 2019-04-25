using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Linq;
using UnityEngine.Networking;
using Newtonsoft.Json;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.Data {
    public delegate void GetCountriesHandler(ICountry country, bool success);

    public static class DataLoader {
        private class CountryPopulationEntry {
            [Index(3)] public string Country { get; set; }
            [Index(11)] public int Year { get; set; }
            [Index(18)] public int Population { get; set; }
        }

        private class Country : ICountry {
            public string Name { get; set; }
            public List<int> Population { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }

        private class Response {
            public string Lat { get; set; }
            public string Lon { get; set; }
        }

        private const string BASE_URL = "https://nominatim.openstreetmap.org/search/";
        private static readonly string DATA_PATH = Path.Combine(Application.dataPath, "Data");

        public static void GetCountries(GetCountriesHandler callback) {
            string path = Path.Combine(DATA_PATH, "country_population_1990-2017.csv");
            const int historySize = 28;
            using (var reader = new StreamReader(path)) {
                using (var csv = new CsvReader(reader)) {
                    // NOTE: The whole processing of the data could probably be prettier and faster
                    var entries = csv.GetRecords<CountryPopulationEntry>();
                    var grouped = entries.ToLookup(e => e.Country);
                    var groupedStripped = grouped.Where(g => g.Count() == historySize);
                    foreach (var group in groupedStripped) {
                        string name = group.Key;
                        var country = new Country() {
                            Name = name,
                            Population = new List<int>(historySize)
                        };
                        var ordered = group.OrderBy(e => e.Year);
                        foreach (var e in ordered) {
                            country.Population.Add(e.Population);
                        }
                        MakeRequest<List<Response>>($"{name}", (responses, success) => {
                            if (success) {
                                if (responses.Count < 1) {
                                    callback?.Invoke(null, false);
                                } else {
                                    Response response = responses[0];
                                    country.Latitude = float.Parse(response.Lat);
                                    country.Longitude = float.Parse(response.Lon);
                                    callback?.Invoke(country, true);
                                }
                            } else {
                                callback?.Invoke(null, false);
                            }
                        });
                    }
                }
            }
        }

        private static void MakeRequest<T>(string query, Action<T, bool> callback) {
            UnityWebRequest request = UnityWebRequest.Get($"{BASE_URL}{query}?format=json");
            request.SendWebRequest().completed += operation => {
                if (request.isHttpError || request.isNetworkError) {
                    Debug.LogError($"[DataLoader] - Request failed with error ({request.responseCode}): {request.error}");
                    callback?.Invoke(default, false);
                } else {
                    T response = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                    callback?.Invoke(response, true);
                }
            };
        }
    }
}
