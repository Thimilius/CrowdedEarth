using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdedEarth.Data;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth {
    public class VirtualEarth : MonoBehaviour {
        [SerializeField] private float m_Radius;
        [SerializeField] private float m_Latitude;
        [SerializeField] private float m_Longitude;

        private void Start() {
            ICitiesAPI citiesAPI = new CitiesAPI();
            //citiesAPI.GetCityDetails(1325, (response, success) => {
            //    if (success) {
            //        Debug.Log($"City {response.Name} ({response.Latitude}, {response.Longitude})");
            //        m_Latitude = response.Latitude;
            //        m_Longitude = response.Longitude;
            //    }
            //});
            citiesAPI.GetCities(10000000, (response, success) => {
                if (success) {
                    foreach (ICity city in response) {
                        Debug.Log($"{city.ID}");
                    }
                }
            });
        }

        private void OnDrawGizmos() {
            Debug.DrawLine(Vector3.zero, ToCartesian(m_Latitude, m_Longitude), Color.red);
        }

        private Vector3 ToCartesian(float latitude, float longitude) {
            latitude *= Mathf.Deg2Rad;
            longitude *= Mathf.Deg2Rad;

            float x = m_Radius * Mathf.Cos(latitude) * Mathf.Cos(longitude);
            float y = m_Radius * Mathf.Cos(latitude) * Mathf.Sin(longitude);
            float z = m_Radius * Mathf.Sin(latitude);

            return new Vector3(x, z, y);
        }
    }
}
