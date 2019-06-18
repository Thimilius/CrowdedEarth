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
        [SerializeField] private Color m_ScaleMinColor;
        [SerializeField] private Color m_ScaleMaxColor;
        [Header("Earth Visualization")]
        [SerializeField] private MeshRenderer m_EarthRenderer;
        [SerializeField] private Material m_InfoMaterial;
        [SerializeField] private Material m_RealMaterial;

        public event Action<VisualObject> OnVisualObjectCreated;

        private List<VisualObject> m_VisualObjects;

        private void Start() {
            m_VisualObjects = new List<VisualObject>();

            DataLoader.GetCountries(country => {
                VisualObject co = CreateVisualObject(VisualObjectType.Pillar, country);
                m_VisualObjects.Add(co);
            });
        }

        public override void SetYear(int year) {
            base.SetYear(year);

            int index = GetYearIndex();
            foreach (var vo in m_VisualObjects) {
                Vector3 localScale = vo.transform.localScale;
                localScale.z = GetScale(vo.Country.PopulationInfo[index].TotalPopulation);
                vo.transform.localScale = localScale;
            }
        }

        private VisualObject CreateVisualObject(VisualObjectType type, ICountry country) {
            float latitude = country.Latitude;
            float longitude = country.Longitude;

            Vector3 position = Coordinates.ToCartesian(latitude, longitude);
            Quaternion rotation = Coordinates.LookFrom(latitude, longitude);
            VisualObject vo = Instantiate(GetPrefabForType(type), position, rotation, m_EarthRenderer.transform);
            vo.tag = tag;
            vo.name = $"Country: {country.Name}";

            Vector3 localScale = vo.transform.localScale;
            localScale.z = GetScale(country.PopulationInfo[0].TotalPopulation);
            vo.transform.localScale = localScale;

            vo.Type = type;
            vo.Country = country;
            vo.SetColor(Color.yellow, new Color(0.5f, 0.5f, 0));

            vo.OnPointerClicked += () => {
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

        private float GetScale(int population) {
            return population / SCALE_NORMALIZATION;
        }
    }
}
