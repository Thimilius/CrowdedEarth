using System.Collections.Generic;
using CrowdedEarth.Data;
using CrowdedEarth.Data.Model;
using UnityEngine;

namespace CrowdedEarth.Visualization {
    public class CountryVisualizer : Visualizer {
        private const float SCALE_NORMALIZATION = 20000000.0f;

        private static ICountry s_Country;

        [SerializeField] private GameObject m_AgePillarPrefab;
        [SerializeField] private Material m_AgePillarMaleMaterial;
        [SerializeField] private Material m_AgePillarFemaleMaterial;

        public ICountry Country => s_Country;

        private GameObject m_Age0_14MalePillar;
        private GameObject m_Age0_14FemalePillar;
        private GameObject m_Age15_64MalePillar;
        private GameObject m_Age15_64FemalePillar;
        private GameObject m_Age65_AboveMalePillar;
        private GameObject m_Age65_AboveFemalePillar;

        private void Start() {
            // This is just so we can start the scene normally
            if (s_Country == null) {
                s_Country = DataLoader.GetCountries().Find(c => c.ID == "Germany");
            }

            CreateAgePillars();

            OnYearChanged += SetScaleForPillars;
        }

        public static void SetCountry(ICountry country) {
            s_Country = country;
        }

        private void CreateAgePillars() {
            IPopulationInfo info = s_Country.PopulationInfo[GetYearIndex()];

            m_Age0_14MalePillar       = CreatePillar(0, m_AgePillarMaleMaterial, info.Age0_14MaleAbsolute);
            m_Age0_14FemalePillar     = CreatePillar(0.5f, m_AgePillarFemaleMaterial, info.Age0_14FemaleAbsolute);

            m_Age15_64MalePillar      = CreatePillar(2, m_AgePillarMaleMaterial, info.Age15_64MaleAbsolute);
            m_Age15_64FemalePillar    = CreatePillar(2.5f, m_AgePillarFemaleMaterial, info.Age15_64FemaleAbsolute);

            m_Age65_AboveMalePillar   = CreatePillar(4, m_AgePillarMaleMaterial, info.Age64_AboveMaleAbsolute);
            m_Age65_AboveFemalePillar = CreatePillar(4.5f, m_AgePillarFemaleMaterial, info.Age64_AboveFemaleAbsolute);
        }

        private GameObject CreatePillar(float x, Material material, int age) {
            GameObject go = Instantiate(m_AgePillarPrefab, transform);
            go.transform.position = new Vector3(x, 0, 0);

            go.GetComponent<Renderer>().material = material;

            SetScaleForPillar(go, age);

            return go;
        }

        private void SetScaleForPillars() {
            IPopulationInfo info = s_Country.PopulationInfo[GetYearIndex()];

            SetScaleForPillar(m_Age0_14MalePillar, info.Age0_14MaleAbsolute);
            SetScaleForPillar(m_Age0_14FemalePillar, info.Age0_14FemaleAbsolute);

            SetScaleForPillar(m_Age15_64MalePillar, info.Age15_64MaleAbsolute);
            SetScaleForPillar(m_Age15_64FemalePillar, info.Age15_64FemaleAbsolute);

            SetScaleForPillar(m_Age65_AboveMalePillar, info.Age64_AboveMaleAbsolute);
            SetScaleForPillar(m_Age65_AboveFemalePillar, info.Age64_AboveFemaleAbsolute);
        }

        private void SetScaleForPillar(GameObject go, int age) {
            Vector3 localScale = go.transform.localScale;
            localScale.z = GetScale(age);
            go.transform.localScale = localScale;
        }

        private float GetScale(int age) {
            return age / SCALE_NORMALIZATION;
        }
    }
}
