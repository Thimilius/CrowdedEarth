using CrowdedEarth.Data;
using CrowdedEarth.Data.Model;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdedEarth.Visualization {
    public class CountryVisualizer : Visualizer {
        private const float SCALE_NORMALIZATION = 20000000.0f;

        private static ICountry s_Country;

        [SerializeField] private AgeVisualObject m_AgeVisualObjectPrefab;
        [SerializeField] private Color m_MaleNormalColor;
        [SerializeField] private Color m_MaleEmissonColor;
        [SerializeField] private Color m_FemaleNormalColor;
        [SerializeField] private Color m_FemaleEmissonColor;

        public ICountry Country => s_Country;

        private List<AgeVisualObject> m_AgeVisualObjects;

        private void Start() {
            m_AgeVisualObjects = new List<AgeVisualObject>();

            // This is just so we can start the scene normally
            if (s_Country == null) {
                s_Country = DataLoader.GetCountries().Find(c => c.ID == "Germany");
            }

            CreateAgeVisualObjects();
        }

        public static void SetCountry(ICountry country) {
            s_Country = country;
        }

        public override void SetYear(int year) {
            base.SetYear(year);

            iTween.Stop();

            foreach (AgeVisualObject vo in m_AgeVisualObjects) {
                SetScaleForVisualObject(vo);
            }
        }

        private void CreateAgeVisualObjects() {
            IPopulationInfo info = s_Country.PopulationInfo[GetYearIndex()];

            m_AgeVisualObjects.Add(CreateAgeVisualObject(0, AgeGroup.Age_0To14_Male, info.Age0_14MaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(0.5f, AgeGroup.Age_0To14_Female, info.Age0_14FemaleAbsolute));

            m_AgeVisualObjects.Add(CreateAgeVisualObject(2, AgeGroup.Age_15To64_Male, info.Age15_64MaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(2.5f, AgeGroup.Age_15To64_Female, info.Age15_64FemaleAbsolute));

            m_AgeVisualObjects.Add(CreateAgeVisualObject(4, AgeGroup.Age_65AndAbove_Male, info.Age64_AboveMaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(4.5f, AgeGroup.Age_65AndAbove_Female, info.Age64_AboveFemaleAbsolute));
        }

        private AgeVisualObject CreateAgeVisualObject(float x, AgeGroup group, int age) {
            AgeVisualObject vo = Instantiate(m_AgeVisualObjectPrefab, transform);
            vo.transform.position = new Vector3(x, 0, 0);

            vo.Data = group;

            // Set right color
            if (group == AgeGroup.Age_0To14_Male || group == AgeGroup.Age_15To64_Male || group == AgeGroup.Age_65AndAbove_Male) {
                vo.SetColor(m_MaleNormalColor, m_MaleEmissonColor);
            } else {
                vo.SetColor(m_FemaleNormalColor, m_FemaleEmissonColor);
            }

            // Animate scaling
            Vector3 scale = vo.transform.localScale;
            scale.z = GetScale(age);
            iTween.ScaleTo(vo.gameObject, scale, 1.0f);
            scale.z = 0;
            vo.transform.localScale = scale;

            return vo;
        }

        private void SetScaleForVisualObject(AgeVisualObject vo) {
            int GetAge() {
                IPopulationInfo info = s_Country.PopulationInfo[GetYearIndex()];

                switch (vo.Data) {
                    case AgeGroup.Age_0To14_Male: return info.Age0_14MaleAbsolute;
                    case AgeGroup.Age_0To14_Female: return info.Age0_14FemaleAbsolute;
                    case AgeGroup.Age_15To64_Male: return info.Age15_64MaleAbsolute;
                    case AgeGroup.Age_15To64_Female: return info.Age15_64FemaleAbsolute;
                    case AgeGroup.Age_65AndAbove_Male: return info.Age64_AboveMaleAbsolute;
                    case AgeGroup.Age_65AndAbove_Female: return info.Age64_AboveFemaleAbsolute;
                    default: return 0;
                }
            }

            Vector3 localScale = vo.transform.localScale;
            localScale.z = GetScale(GetAge());
            vo.transform.localScale = localScale;
        }

        private float GetScale(int age) {
            return age / SCALE_NORMALIZATION;
        }
    }
}
