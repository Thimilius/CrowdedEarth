using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using InhabitedEarth.Data;
using InhabitedEarth.Data.Model;

namespace InhabitedEarth.Visualization {
    public class CountryVisualizer : Visualizer {
        private const float SCALE_NORMALIZATION = 20000000.0f;
        private const float SCALE_MAX = 2.0f;

        private static ICountry s_Country;

        [SerializeField] private AgeVisualObject m_AgeVisualObjectPrefab;
        [SerializeField] private Color m_MaleNormalColor;
        [SerializeField] private Color m_MaleEmissonColor;
        [SerializeField] private Color m_FemaleNormalColor;
        [SerializeField] private Color m_FemaleEmissonColor;
        [SerializeField] private MeshFilter m_MalePercentageMeshFilter;
        [SerializeField] private MeshFilter m_UrbanPercentageMeshFilter;

        public ICountry Country => s_Country;

        public event Action<VisualObject<AgeGroup>> OnVisualObjectCreated;

        private List<AgeVisualObject> m_AgeVisualObjects;
        private int m_PopulationMax;

        private void Start() {
            m_AgeVisualObjects = new List<AgeVisualObject>();

            // This is just so we can start the scene normally
            if (s_Country == null) {
                s_Country = DataLoader.GetCountries().Find(c => c.ID == "Germany");
            }

            m_PopulationMax = GetPopulationMaximum();

            CreatePieCharts();
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

            CreatePieCharts();
        }

        public void ToggleAgeGroup(AgeGroup maleGroup, AgeGroup femaleGroup) {
            AgeVisualObject maleVo = m_AgeVisualObjects.Find(vo => vo.Data == maleGroup);
            AgeVisualObject femaleVo = m_AgeVisualObjects.Find(vo => vo.Data == femaleGroup);

            if (maleVo != null && femaleVo != null) {
                maleVo.gameObject.SetActive(!maleVo.gameObject.activeSelf);
                femaleVo.gameObject.SetActive(!femaleVo.gameObject.activeSelf);
            }
        }

        public int GetAge(VisualObject<AgeGroup> vo) {
            IPopulationInfo info = s_Country.PopulationInfo[GetYearIndex()];

            switch (vo.Data) {
                case AgeGroup.Age_0To9_Male: return info.Age0_9MaleAbsolute;
                case AgeGroup.Age_10To19_Male: return info.Age10_19MaleAbsolute;
                case AgeGroup.Age_20To29_Male: return info.Age20_29MaleAbsolute;
                case AgeGroup.Age_30To39_Male: return info.Age30_39MaleAbsolute;
                case AgeGroup.Age_40To49_Male: return info.Age40_49MaleAbsolute;
                case AgeGroup.Age_50To59_Male: return info.Age50_59MaleAbsolute;
                case AgeGroup.Age_60To69_Male: return info.Age60_69MaleAbsolute;
                case AgeGroup.Age_70To79_Male: return info.Age70_79MaleAbsolute;
                case AgeGroup.Age_80AndAbove_Male: return info.Age80_AboveMaleAbsolute;
                case AgeGroup.Age_0To9_Female: return info.Age0_9FemaleAbsolute;
                case AgeGroup.Age_10To19_Female: return info.Age10_19FemaleAbsolute;
                case AgeGroup.Age_20To29_Female: return info.Age20_29FemaleAbsolute;
                case AgeGroup.Age_30To39_Female: return info.Age30_39FemaleAbsolute;
                case AgeGroup.Age_40To49_Female: return info.Age40_49FemaleAbsolute;
                case AgeGroup.Age_50To59_Female: return info.Age50_59FemaleAbsolute;
                case AgeGroup.Age_60To69_Female: return info.Age60_69FemaleAbsolute;
                case AgeGroup.Age_70To79_Female: return info.Age70_79FemaleAbsolute;
                case AgeGroup.Age_80AndAbove_Female: return info.Age80_AboveFemaleAbsolute;
                default: return -1;
            }
        }

        public bool IsAgeGroupMale(AgeGroup group) {
            switch (group) {
                case AgeGroup.Age_0To9_Male:
                case AgeGroup.Age_10To19_Male:
                case AgeGroup.Age_20To29_Male:
                case AgeGroup.Age_30To39_Male:
                case AgeGroup.Age_40To49_Male:
                case AgeGroup.Age_50To59_Male:
                case AgeGroup.Age_60To69_Male:
                case AgeGroup.Age_70To79_Male:
                case AgeGroup.Age_80AndAbove_Male: return true;
                case AgeGroup.Age_0To9_Female:
                case AgeGroup.Age_10To19_Female:
                case AgeGroup.Age_20To29_Female:
                case AgeGroup.Age_30To39_Female:
                case AgeGroup.Age_40To49_Female:
                case AgeGroup.Age_50To59_Female:
                case AgeGroup.Age_60To69_Female:
                case AgeGroup.Age_70To79_Female:
                case AgeGroup.Age_80AndAbove_Female: return false;
                default: return false;
            }
        }

        private void CreateAgeVisualObjects() {
            IPopulationInfo info = s_Country.PopulationInfo[GetYearIndex()];

            m_AgeVisualObjects.Add(CreateAgeVisualObject(0, AgeGroup.Age_0To9_Male, info.Age0_9MaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(0.3f, AgeGroup.Age_0To9_Female, info.Age0_9FemaleAbsolute));
            
            m_AgeVisualObjects.Add(CreateAgeVisualObject(1, AgeGroup.Age_10To19_Male, info.Age10_19MaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(1.3f, AgeGroup.Age_10To19_Female, info.Age10_19FemaleAbsolute));
            
            m_AgeVisualObjects.Add(CreateAgeVisualObject(2, AgeGroup.Age_20To29_Male, info.Age20_29MaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(2.3f, AgeGroup.Age_20To29_Female, info.Age20_29FemaleAbsolute));

            m_AgeVisualObjects.Add(CreateAgeVisualObject(3, AgeGroup.Age_30To39_Male, info.Age30_39MaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(3.3f, AgeGroup.Age_30To39_Female, info.Age30_39FemaleAbsolute));

            m_AgeVisualObjects.Add(CreateAgeVisualObject(4, AgeGroup.Age_40To49_Male, info.Age40_49MaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(4.3f, AgeGroup.Age_40To49_Female, info.Age40_49FemaleAbsolute));

            m_AgeVisualObjects.Add(CreateAgeVisualObject(5, AgeGroup.Age_50To59_Male, info.Age50_59MaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(5.3f, AgeGroup.Age_50To59_Female, info.Age50_59FemaleAbsolute));

            m_AgeVisualObjects.Add(CreateAgeVisualObject(6, AgeGroup.Age_60To69_Male, info.Age60_69MaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(6.3f, AgeGroup.Age_60To69_Female, info.Age60_69FemaleAbsolute));

            m_AgeVisualObjects.Add(CreateAgeVisualObject(7, AgeGroup.Age_70To79_Male, info.Age70_79MaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(7.3f, AgeGroup.Age_70To79_Female, info.Age70_79FemaleAbsolute));

            m_AgeVisualObjects.Add(CreateAgeVisualObject(8, AgeGroup.Age_80AndAbove_Male, info.Age80_AboveMaleAbsolute));
            m_AgeVisualObjects.Add(CreateAgeVisualObject(8.3f, AgeGroup.Age_80AndAbove_Female, info.Age80_AboveFemaleAbsolute));
        }

        private void CreatePieCharts() {
            IPopulationInfo info = s_Country.PopulationInfo[GetYearIndex()];
            m_MalePercentageMeshFilter.mesh = MeshBuilder.BuildNewCylinder(info.MalePercentage / 100.0f);
            m_UrbanPercentageMeshFilter.mesh = MeshBuilder.BuildNewCylinder(info.UrbanPercentage / 100.0f);
        }

        private AgeVisualObject CreateAgeVisualObject(float x, AgeGroup group, int age) {
            AgeVisualObject vo = Instantiate(m_AgeVisualObjectPrefab, transform);
            vo.transform.position = new Vector3(x, 0, 0);

            vo.Data = group;

            // Set right color
            if (IsAgeGroupMale(group)) {
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

            OnVisualObjectCreated?.Invoke(vo);

            return vo;
        }

        private void SetScaleForVisualObject(AgeVisualObject vo) {
            Vector3 localScale = vo.transform.localScale;
            localScale.z = GetScale(GetAge(vo));
            vo.transform.localScale = localScale;
        }

        private int GetPopulationMaximum() {
            return new List<int>(18) {
                s_Country.PopulationInfo.Max(i => i.Age0_9MaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age10_19MaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age20_29MaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age30_39MaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age40_49MaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age50_59MaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age60_69MaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age70_79MaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age80_AboveMaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age0_9FemaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age10_19FemaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age20_29FemaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age30_39FemaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age40_49FemaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age50_59FemaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age60_69FemaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age70_79FemaleAbsolute),
                s_Country.PopulationInfo.Max(i => i.Age80_AboveFemaleAbsolute),
            }.Max();
        }

        private float GetScale(int age) {
            return (float)age / ((float)m_PopulationMax) * SCALE_MAX;
        }
    }
}
