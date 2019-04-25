using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdedEarth.Data;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth {
    public class Visualizer : MonoBehaviour {
        [SerializeField] private WorldCamera m_WorldCamera;
        [SerializeField] private GameObject m_VisualObjectPrefab;

        private void Start() {
            DataLoader.GetCountries((country, success) => {
                if (success) {
                    float population = country.Population[country.Population.Count - 1];
                    var co = MakeVisualObject<CountryObject>(country.Latitude, country.Longitude, population / 20000000f);
                    co.Country = country;
                }
            });
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    if (hit.transform.CompareTag("CountryObject")) {
                        // HACK: Hardcoded visual object type parameter
                        var co = hit.transform.GetComponent<CountryObject>();
                        ICountry country = co.Country;
                        Debug.Log($"{country.Name} - {country.Population[country.Population.Count - 1]}");
                        m_WorldCamera.RotateTo(country.Latitude, country.Longitude);
                    }
                }
            }
        }

        private T MakeVisualObject<T>(float latitude, float longitude, float scale) where T : VisualObject {
            // HACK: Hardcoded prefab
            GameObject go = Instantiate(m_VisualObjectPrefab, Coordinates.ToCartesian(latitude, longitude), Coordinates.LookFrom(latitude, longitude), transform);
            go.tag = "CountryObject";

            Vector3 localScale = go.transform.localScale;
            localScale.z = scale;
            go.transform.localScale = localScale;

            var vo = go.AddComponent<T>();
            vo.Latitude = latitude;
            vo.Longitude = longitude;

            return vo;
        }
    }
}
