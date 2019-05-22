using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdedEarth.Data;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.Visualization {
    public class Visualizer : MonoBehaviour {
        [SerializeField] private WorldCamera m_WorldCamera;
        [SerializeField] private GameObject m_VisualObjectPrefab;
        [Header("Earth Visualization")]
        [SerializeField] private MeshRenderer m_EarthRenderer;
        [SerializeField] private Material m_InfoMaterial;
        [SerializeField] private Material m_RealMaterial;

        private List<CountryObject> m_CountryObjects;
        private int m_Year;

        private void Start() {
            m_CountryObjects = new List<CountryObject>();

            DataLoader.GetCountries((country, success) => {
                if (success) {
                    float population = country.Population[0];
                    float scale = population / 20000000f;
                    var co = MakeVisualObject<CountryObject>(country.Latitude, country.Longitude, scale, $"Country: {country.Name}");
                    co.Country = country;

                    m_CountryObjects.Add(co);
                }
            });
        }

        private void Update() {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit)) {
                        var vo = hit.transform.GetComponent<VisualObject>();
                        if (vo != null) {
                            if (vo.Type == VisualObjectType.Country) {
                                CountryObject co = (CountryObject)vo;
                                ICountry country = co.Country;
                                Debug.Log($"{country.Name} - {country.Population[YearToIndex(m_Year)]}");
                                m_WorldCamera.RotateTo(country.Latitude, country.Longitude);
                            }
                        }
                    }
                }
            }
        }

        public void SetVisualizationMode(VisualizationMode mode) {
            switch (mode) {
                case VisualizationMode.Info:
                    m_EarthRenderer.material = m_InfoMaterial;
                    m_WorldCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
                    RenderSettings.ambientSkyColor = new Color(0.5f, 0.5f, 0.5f);
                    break;
                case VisualizationMode.Real:
                    m_EarthRenderer.material = m_RealMaterial;
                    m_WorldCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
                    RenderSettings.ambientSkyColor = new Color(0.2f, 0.2f, 0.2f);
                    break;
                default:
                    break;
            }
        }

        public void SetYear(int year) {
            m_Year = year;

            int index = YearToIndex(year); 
            foreach (var co in m_CountryObjects) {
                float population = co.Country.Population[index];
                float scale = population / 20000000f;

                Vector3 localScale = co.transform.localScale;
                localScale.z = scale;
                co.transform.localScale = localScale;
            }
        }

        private T MakeVisualObject<T>(float latitude, float longitude, float scale, string name) where T : VisualObject {
            // HACK: Hardcoded prefab
            GameObject go = Instantiate(m_VisualObjectPrefab, Coordinates.ToCartesian(latitude, longitude), Coordinates.LookFrom(latitude, longitude), transform);
            go.tag = tag;
            go.name = name;

            Vector3 localScale = go.transform.localScale;
            localScale.z = scale;
            go.transform.localScale = localScale;

            var vo = go.AddComponent<T>();
            vo.Latitude = latitude;
            vo.Longitude = longitude;

            return vo;
        }

        private int YearToIndex(int year) {
            // HACK: Hardcoded!
            year = Mathf.Clamp(year, 1960, 2050);
            return year - 1960;
        }
    }
}
