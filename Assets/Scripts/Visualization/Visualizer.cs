using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdedEarth.Data;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.Visualization {
    public class Visualizer : MonoBehaviour {
        private float SCALE_NORMALIZATION = 20000000.0f;

        [SerializeField] private WorldCamera m_WorldCamera;
        [SerializeField] private GameObject m_VisualObjectPrefab;
        [Header("Earth Visualization")]
        [SerializeField] private MeshRenderer m_EarthRenderer;
        [SerializeField] private Material m_InfoMaterial;
        [SerializeField] private Material m_RealMaterial;

        private List<VisualObject> m_VisualObjects;
        private int m_Year;

        private void Start() {
            m_VisualObjects = new List<VisualObject>();

            DataLoader.GetCountries((country, success) => {
                if (success) {
                    float population = country.Population[0];
                    VisualObject co = MakeVisualObject(VisualObjectType.Pillar, country);
                    m_VisualObjects.Add(co);
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
                            ICountry country = vo.Country;
                            Debug.Log($"{country.Name} - {country.Population[YearToIndex(m_Year)]}");
                            m_WorldCamera.RotateTo(country.Latitude, country.Longitude);
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
            foreach (var vo in m_VisualObjects) {
                Vector3 localScale = vo.transform.localScale;
                localScale.z = GetScale(vo.Country.Population[index]);
                vo.transform.localScale = localScale;
            }
        }

        private VisualObject MakeVisualObject(VisualObjectType type, ICountry country) {
            float latitude = country.Latitude;
            float longitude = country.Longitude;

            GameObject parent = new GameObject();
            parent.transform.SetParent(transform);
            parent.name = $"Country: {country.Name}";

            GameObject go = Instantiate(GetPrefabForType(type), Coordinates.ToCartesian(latitude, longitude), Coordinates.LookFrom(latitude, longitude), transform);
            go.tag = tag;
            go.name = $"Country: {country.Name}";

            Vector3 localScale = go.transform.localScale;
            localScale.z = GetScale(country.Population[0]);
            go.transform.localScale = localScale;

            VisualObject vo = go.AddComponent<VisualObject>();
            vo.Type = type;
            vo.Country = country;

            return vo;
        }

        private GameObject GetPrefabForType(VisualObjectType type) {
            switch (type) {
                case VisualObjectType.Pillar: return m_VisualObjectPrefab;
                default: return null;
            }
        }

        private float GetScale(int population) {
            return population / SCALE_NORMALIZATION;
        }

        private int YearToIndex(int year) {
            // HACK: Hardcoded!
            year = Mathf.Clamp(year, 1960, 2050);
            return year - 1960;
        }
    }
}
