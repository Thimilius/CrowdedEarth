using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdedEarth.Data;

namespace CrowdedEarth {
    public class Visualizer : MonoBehaviour {
        [SerializeField] private GameObject m_PillarPrefab;

        private void Start() {
            DataLoader.GetCities((city, success) => {
                if (success) {
                    MakePillar(city.Latitude, city.Longitude, city.Population[city.Population.Count - 1] / 20000000f);
                }
            });
        }

        private void MakePillar(float latitude, float longitude, float scale) {
            GameObject go = Instantiate(m_PillarPrefab, Coordinates.ToCartesian(latitude, longitude), Coordinates.LookFrom(latitude, longitude), transform);
            Vector3 localScale = go.transform.localScale;
            localScale.z = scale;
            go.transform.localScale = localScale;
        }
    }
}
