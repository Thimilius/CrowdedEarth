using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using CrowdedEarth.Visualization;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.UI {
    public class CountryUI : MonoBehaviour {
        [SerializeField] private CountryVisualizer m_Visualizer;
        [SerializeField] private Button m_BackToEarthButton;
        [Header("General Info")]
        [SerializeField] private Image m_CountryFlag;
        [SerializeField] private TMP_Text m_CountryNameText;
        [Header("Size")]
        [SerializeField] private TMP_Text m_SizeText;
        [SerializeField] private TMP_Text m_DensityText;
        [Header("Male/Female Info")]
        [SerializeField] private GameObject m_MaleFemalePercentageInfo;
        [SerializeField] private TMP_Text m_MalePercentageText;
        [SerializeField] private TMP_Text m_FemalePercentageText;
        [Header("Urban/Rural Info")]
        [SerializeField] private GameObject m_UrbanRuralPercentageInfo;
        [SerializeField] private TMP_Text m_UrbanPercentageText;
        [SerializeField] private TMP_Text m_RuralPercentageText;
        [Header("Age Info")]
        [SerializeField] private TMP_Text m_0_14AgeGroupInfo;
        [SerializeField] private TMP_Text m_15_64AgeGroupInfo;
        [SerializeField] private TMP_Text m_64_AboveAgeGroupInfo;

        private void Awake() {
            m_Visualizer.OnYearChanged += OnYearChanged;
            m_Visualizer.OnVisualObjectCreated += vo => {
                vo.OnPointerEntered += () => OnPointerEntered(vo);
                vo.OnPointerExited += () => OnPointerExited(vo);
            };
        }

        private void Start() {
            m_BackToEarthButton.onClick.AddListener(() => {
                SceneManager.LoadScene(0);
            });

            ICountry country = m_Visualizer.Country;
            m_CountryFlag.sprite = SpriteManager.GetFlag(country.Flag);

            OnYearChanged();
        }

        private void OnYearChanged() {
            ICountry country = m_Visualizer.Country;
            IPopulationInfo info = country.PopulationInfo[m_Visualizer.GetYearIndex()];

            m_CountryNameText.text = $"{country.Name} - Bevölkerung: {info.PopulationTotal.ToString("N0", new CultureInfo("de-DE"))}";

            m_SizeText.text = $"Größe in km²: {country.Size.ToString("N0", new CultureInfo("de-DE"))}";
            m_DensityText.text = $"Einwohner pro km²: {(info.PopulationTotal / country.Size).ToString("0")}";

            // Set male/female percentage
            if (info.MalePercentage > 0 && info.FemalePercentage > 0) {
                m_MaleFemalePercentageInfo.SetActive(true);
                m_MalePercentageText.text = $"\uf222 {info.MalePercentage.ToString("0.00")} %";
                m_FemalePercentageText.text = $"\uf221 {info.FemalePercentage.ToString("0.00")} %";
            } else {
                m_MaleFemalePercentageInfo.SetActive(false);
            }

            // Set urban/rural percentage
            if (info.UrbanPercentage >= 0 && info.RuralPercentage >= 0) {
                m_UrbanRuralPercentageInfo.SetActive(true);
                m_UrbanPercentageText.text = $"\uf64f {info.UrbanPercentage.ToString("0.00")} %";
                m_RuralPercentageText.text = $"\uf4d8 {info.RuralPercentage.ToString("0.00")} %";
            } else {
                m_UrbanRuralPercentageInfo.SetActive(false);
            }
        }

        private void OnPointerEntered(VisualObject<AgeGroup> vo) {
            string text;
            if (m_Visualizer.IsAgeGroupMale(vo.Data)) {
                text = "Männlich: ";
            } else {
                text = "Weiblich: ";
            }
            text = text + m_Visualizer.GetAge(vo).ToString("N0", new CultureInfo("de-DE")); 

            m_0_14AgeGroupInfo.gameObject.SetActive(false);
            m_15_64AgeGroupInfo.gameObject.SetActive(false);
            m_64_AboveAgeGroupInfo.gameObject.SetActive(false);

            switch (vo.Data) {
                case AgeGroup.Age_0To9_Male:
                case AgeGroup.Age_0To9_Female:
                    m_0_14AgeGroupInfo.gameObject.SetActive(true);
                    m_0_14AgeGroupInfo.text = text;
                    break;
                case AgeGroup.Age_10To19_Male:
                case AgeGroup.Age_10To19_Female:
                    m_15_64AgeGroupInfo.gameObject.SetActive(true);
                    m_15_64AgeGroupInfo.text = text;
                    break;
                case AgeGroup.Age_20To29_Male:
                case AgeGroup.Age_20To29_Female:
                    m_64_AboveAgeGroupInfo.gameObject.SetActive(true);
                    m_64_AboveAgeGroupInfo.text = text;
                    break;
                default: break;
            }
        }

        private void OnPointerExited(VisualObject<AgeGroup> vo) {
            m_0_14AgeGroupInfo.gameObject.SetActive(false);
            m_15_64AgeGroupInfo.gameObject.SetActive(false);
            m_64_AboveAgeGroupInfo.gameObject.SetActive(false);
        }
    }
}
