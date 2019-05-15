using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdedEarth.Data;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth {
    public class Visualizer : MonoBehaviour {
        [SerializeField] private WorldCamera m_WorldCamera;
        [SerializeField] private GameObject m_VisualObjectPrefab;
        [Header("Earth Visualization")]
        [SerializeField] private MeshRenderer m_EarthRenderer;
        [SerializeField] private Material m_InfoMaterial;
        [SerializeField] private Material m_RealMaterial;

        private List<CountryObject> m_CountryObjects;

        private void Start() {
            m_CountryObjects = new List<CountryObject>();

            DataLoader.GetCountries((country, success) => {
                if (success) {
                    float population = country.Population[0];
                    var co = MakeVisualObject<CountryObject>(country.Latitude, country.Longitude, population / 20000000f);
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

            if (Input.GetKeyDown(KeyCode.Space)) {
                StartCoroutine(PlayAnimation());
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

        private IEnumerator PlayAnimation() {
            // Starts out at 1991 and 1990 is the current year
            for (int year = 1; year < 28; year++) {
                yield return StartCoroutine(PlayAnimationForYear(year));
                Debug.Log($"Year: {year}");
            }
        }

        private IEnumerator PlayAnimationForYear(int year) {
            const float time = 1;
            foreach (var co in m_CountryObjects) {
                float population = co.Country.Population[year];
                float scale = population / 20000000f;

                Vector3 localScale = co.transform.localScale;
                localScale.z = scale;

                iTween.ScaleTo(co.gameObject, localScale, time);
            }
            yield return new WaitForSeconds(time);
        }
    }
}
