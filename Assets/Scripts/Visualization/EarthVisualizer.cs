using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CrowdedEarth.Data;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.Visualization {
    public class EarthVisualizer : Visualizer {
        private const float SCALE_NORMALIZATION = 20000000.0f;

        [SerializeField] private WorldCamera m_WorldCamera;
        [Header("Visual Objects")]
        [SerializeField] private VisualObject m_VisualObjectPillarPrefab;
        [SerializeField] private Color m_MinColor;
        [SerializeField] private Color m_MaxColor;
        [SerializeField] private Color m_MinEmissionColor;
        [SerializeField] private Color m_MaxEmissionColor;
        [Header("Earth Visualization")]
        [SerializeField] private MeshRenderer m_EarthRenderer;
        [SerializeField] private Material m_InfoMaterial;
        [SerializeField] private Material m_RealMaterial;

        public event Action<VisualObject> OnVisualObjectCreated;

        private List<VisualObject> m_VisualObjects;
        private int m_MinPopulation;
        private int m_MaxPopulation;

        private void Start() {
            m_VisualObjects = new List<VisualObject>();

            List<ICountry> countries = DataLoader.GetCountries();
            foreach (var country in countries) {
                VisualObject co = CreateVisualObject(VisualObjectType.Pillar, country);
                m_VisualObjects.Add(co);
            }

            FindNewMinAndMaxPopulation();

            // Set initial color
            foreach (var vo in m_VisualObjects) {
                // Set new color
                int population = vo.Country.PopulationInfo[GetYearIndex()].PopulationTotal;
                vo.SetColor(GetColor(population), GetEmissionColor(population));
            }
        }

        public override void SetYear(int year) {
            base.SetYear(year);

            FindNewMinAndMaxPopulation();

            iTween.Stop();

            foreach (var vo in m_VisualObjects) {
                // Set new scale
                Vector3 localScale = vo.transform.localScale;
                localScale.z = GetScale(vo.Country);
                vo.transform.localScale = localScale;

                // Set new color
                int population = vo.Country.PopulationInfo[GetYearIndex()].PopulationTotal;
                vo.SetColor(GetColor(population), GetEmissionColor(population));
            }
        }

        private VisualObject CreateVisualObject(VisualObjectType type, ICountry country) {
            float latitude = country.Latitude;
            float longitude = country.Longitude;

            Vector3 position = Coordinates.ToCartesian(latitude, longitude);
            Quaternion rotation = Coordinates.LookFrom(latitude, longitude);
            VisualObject vo = Instantiate(GetPrefabForType(type), position, rotation, m_EarthRenderer.transform);
            vo.tag = tag;
            vo.name = $"Country: {country.ID}";

            // Animate scaling
            Vector3 scale = vo.transform.localScale;
            scale.z = GetScale(country);
            iTween.ScaleTo(vo.gameObject, scale, 2.0f);
            scale.z = 0;
            vo.transform.localScale = scale;

            vo.Type = type;
            vo.Country = country;

            vo.OnPointerClicked += () => {
                CountryVisualizer.SetCountry(country);
                SceneManager.LoadScene(1);
            };

            OnVisualObjectCreated?.Invoke(vo);

            return vo;
        }

        private VisualObject GetPrefabForType(VisualObjectType type) {
            switch (type) {
                case VisualObjectType.Pillar: return m_VisualObjectPillarPrefab;
                default: return null;
            }
        }

        private float GetScale(ICountry country) {
            return country.PopulationInfo[GetYearIndex()].PopulationTotal / SCALE_NORMALIZATION;
        }

        private Color GetColor(int population) {
            float t = Mathf.InverseLerp(m_MinPopulation, m_MaxPopulation, population);
            return Color.Lerp(m_MinColor, m_MaxColor, t);
        }

        private Color GetEmissionColor(int population) {
            float t = Mathf.InverseLerp(m_MinPopulation, m_MaxPopulation, population);
            return Color.Lerp(m_MinEmissionColor, m_MaxEmissionColor, t);
        }

        private void FindNewMinAndMaxPopulation() {
            m_MinPopulation = int.MaxValue;
            m_MaxPopulation = int.MinValue;

            // Find min and max population
            foreach (var vo in m_VisualObjects) {
                int population = vo.Country.PopulationInfo[GetYearIndex()].PopulationTotal;
                if (population < m_MinPopulation) {
                    m_MinPopulation = population;
                }
                if (population > m_MaxPopulation) {
                    m_MaxPopulation = population;
                }
            }
        }
    }
}
