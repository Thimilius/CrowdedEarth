using UnityEngine;
using UnityEngine.UI;
using CrowdedEarth.Visualization;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.UI {
    public class AgeUI : MonoBehaviour {
        [SerializeField] private CountryVisualizer m_Visualizer;
        [Header("Toggles")]
        [SerializeField] private Toggle m_0To9Toggle;
        [SerializeField] private Toggle m_10To19Toggle;
        [SerializeField] private Toggle m_20To29Toggle;
        [SerializeField] private Toggle m_30To39Toggle;
        [SerializeField] private Toggle m_40To49Toggle;
        [SerializeField] private Toggle m_50To59Toggle;
        [SerializeField] private Toggle m_60To69Toggle;
        [SerializeField] private Toggle m_70To79Toggle;
        [SerializeField] private Toggle m_80AndAboveToggle;
        [Header("Labels")]
        [SerializeField] private GameObject m_0To9Label;
        [SerializeField] private GameObject m_10To19Label;
        [SerializeField] private GameObject m_20To29Label;
        [SerializeField] private GameObject m_30To39Label;
        [SerializeField] private GameObject m_40To49Label;
        [SerializeField] private GameObject m_50To59Label;
        [SerializeField] private GameObject m_60To69Label;
        [SerializeField] private GameObject m_70To79Label;
        [SerializeField] private GameObject m_80AndAboveLabel;

        private void Start() {
            m_0To9Toggle.onValueChanged.AddListener(_ => ToggleAgeGroup(AgeGroup.Age_0To9_Male, AgeGroup.Age_0To9_Female, m_0To9Label));
            m_10To19Toggle.onValueChanged.AddListener(_ => ToggleAgeGroup(AgeGroup.Age_10To19_Male, AgeGroup.Age_10To19_Female, m_10To19Label));
            m_20To29Toggle.onValueChanged.AddListener(_ => ToggleAgeGroup(AgeGroup.Age_20To29_Male, AgeGroup.Age_20To29_Female, m_20To29Label));
            m_30To39Toggle.onValueChanged.AddListener(_ => ToggleAgeGroup(AgeGroup.Age_30To39_Male, AgeGroup.Age_30To39_Female, m_30To39Label));
            m_40To49Toggle.onValueChanged.AddListener(_ => ToggleAgeGroup(AgeGroup.Age_40To49_Male, AgeGroup.Age_40To49_Female, m_40To49Label));
            m_50To59Toggle.onValueChanged.AddListener(_ => ToggleAgeGroup(AgeGroup.Age_50To59_Male, AgeGroup.Age_50To59_Female, m_50To59Label));
            m_60To69Toggle.onValueChanged.AddListener(_ => ToggleAgeGroup(AgeGroup.Age_60To69_Male, AgeGroup.Age_60To69_Female, m_60To69Label));
            m_70To79Toggle.onValueChanged.AddListener(_ => ToggleAgeGroup(AgeGroup.Age_70To79_Male, AgeGroup.Age_70To79_Female, m_70To79Label));
            m_80AndAboveToggle.onValueChanged.AddListener(_ => ToggleAgeGroup(AgeGroup.Age_80AndAbove_Male, AgeGroup.Age_80AndAbove_Female, m_80AndAboveLabel));
        }

        private void ToggleAgeGroup(AgeGroup maleGroup, AgeGroup femaleGroup, GameObject label) {
            m_Visualizer.ToggleAgeGroup(maleGroup, femaleGroup);
            label.SetActive(!label.activeSelf); 
        }
    }
}
