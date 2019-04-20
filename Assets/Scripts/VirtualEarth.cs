using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdedEarth.Data;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth {
    public class VirtualEarth : MonoBehaviour {
        [SerializeField] private float m_Latitude;
        [SerializeField] private float m_Longitude;
        [SerializeField] private GameObject m_Prefab;

        private void Start() {
            ICitiesAPI citiesAPI = new CitiesAPI();
            //citiesAPI.GetCityDetails(1325, (response, success) => {
            //    if (success) {
            //        Debug.Log($"City {response.Name} ({response.Latitude}, {response.Longitude})");
            //        m_Latitude = response.Latitude;
            //        m_Longitude = response.Longitude;
            //    }
            //});
            //citiesAPI.GetCities(10000000, (response, success) => {
            //    if (success) {
            //        foreach (ICity city in response) {
            //            Debug.Log($"{city.ID}");
            //        }
            //    }
            //});

            GameObject go = Instantiate(m_Prefab, Coordinates.ToCartesian(m_Latitude, m_Longitude), Coordinates.LookFrom(m_Latitude, m_Longitude));
        }

        private void OnDrawGizmos() {
            Debug.DrawLine(Vector3.zero, Coordinates.ToCartesian(m_Latitude, m_Longitude), Color.red);
        }
    }
}
