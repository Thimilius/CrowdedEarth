using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CrowdedEarth.Data;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.Visualization {
    public class Visualizer : MonoBehaviour {
        private const float SCALE_NORMALIZATION = 20000000.0f;

        [SerializeField] private WorldCamera m_WorldCamera;
        [Header("Visual Objects")]
        [SerializeField] private VisualObject m_VisualObjectPillarPrefab;
        [Header("Earth Visualization")]
        [SerializeField] private MeshRenderer m_EarthRenderer;
        [SerializeField] private Material m_InfoMaterial;
        [SerializeField] private Material m_RealMaterial;

        public event Action<VisualObject> OnVisualObjectCreated;
        public VisualizationMode VisualizationMode { get; private set; }

        private List<VisualObject> m_VisualObjects;
        private int m_Year;

        private void Start() {
            m_VisualObjects = new List<VisualObject>();

            DataLoader.GetCountries((country, success) => {
                if (success) {
                    VisualObject co = CreateVisualObject(VisualObjectType.Pillar, country);
                    m_VisualObjects.Add(co);
                }
            });
        }

        private void Update() {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }

            VisualObject vo = GetVisualObjectUnderMouse();

            if (Input.GetKeyDown(KeyCode.Mouse0)) {
                if (vo != null) {
                    ICountry country = vo.Country;
                    Debug.Log($"{country.Name} - {country.PopulationInfo[GetYearIndex()]}");
                    m_WorldCamera.RotateTo(country.Latitude, country.Longitude);
                }
            }
        }

        public void SetVisualizationMode(VisualizationMode mode) {
            VisualizationMode = mode;

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

            int index = GetYearIndex();
            foreach (var vo in m_VisualObjects) {
                Vector3 localScale = vo.transform.localScale;
                localScale.z = GetScale(vo.Country.PopulationInfo[index].TotalPopulation);
                vo.transform.localScale = localScale;
            }
        }

        public int GetYearIndex() {
            const int YEAR_LIMIT_MIN = 1960;
            const int YEAR_LIMIT_MAX = 2050;

            // HACK: Hardcoded!
            int year = Mathf.Clamp(m_Year, YEAR_LIMIT_MIN, YEAR_LIMIT_MAX);
            return year - YEAR_LIMIT_MIN;
        }

        private VisualObject CreateVisualObject(VisualObjectType type, ICountry country) {
            float latitude = country.Latitude;
            float longitude = country.Longitude;

            Vector3 position = Coordinates.ToCartesian(latitude, longitude);
            Quaternion rotation = Coordinates.LookFrom(latitude, longitude);
            VisualObject vo = Instantiate(GetPrefabForType(type), position, rotation, transform);
            vo.tag = tag;
            vo.name = $"Country: {country.Name}";

            Vector3 localScale = vo.transform.localScale;
            localScale.z = GetScale(country.PopulationInfo[0].TotalPopulation);
            vo.transform.localScale = localScale;

            vo.Type = type;
            vo.Country = country;
            vo.SetColor(Color.yellow);

            OnVisualObjectCreated?.Invoke(vo);

            return vo;
        }

        private VisualObject GetVisualObjectUnderMouse() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                return hit.transform.GetComponent<VisualObject>();
            } else {
                return null;
            }
        }

        private VisualObject GetPrefabForType(VisualObjectType type) {
            switch (type) {
                case VisualObjectType.Pillar: return m_VisualObjectPillarPrefab;
                default: return null;
            }
        }

        private float GetScale(int population) {
            return population / SCALE_NORMALIZATION;
        }
    }
}
