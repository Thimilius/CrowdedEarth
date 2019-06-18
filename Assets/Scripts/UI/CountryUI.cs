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
        [Header("Urban/Rural Info")]
        [SerializeField] private GameObject m_UrbanRuralPercentageInfo;
        [SerializeField] private TMP_Text m_UrbanPercentageText;
        [SerializeField] private TMP_Text m_RuralPercentageText;
        [SerializeField] private Image m_UrbanPercentageImage;
        [SerializeField] private Image m_RuralPercentageImage;

        private void Start() {
            m_BackToEarthButton.onClick.AddListener(() => {
                SceneManager.LoadScene(0);
            });

            ICountry country = m_Visualizer.Country;
            m_CountryFlag.sprite = SpriteManager.GetFlag(country.Flag);
            m_CountryNameText.text = country.NameGerman;

            m_Visualizer.OnYearChanged += OnYearChanged;

            OnYearChanged();
        }

        private void OnYearChanged() {
            IPopulationInfo info = m_Visualizer.Country.PopulationInfo[m_Visualizer.GetYearIndex()];
            if (info.UrbanPercentage >= 0 && info.RuralPercentage >= 0) {
                m_UrbanRuralPercentageInfo.SetActive(true);
                m_UrbanPercentageText.text = $"{info.UrbanPercentage.ToString("0.00")} %";
                m_RuralPercentageText.text = $"{info.RuralPercentage.ToString("0.00")} %";
                m_UrbanPercentageImage.fillAmount = info.UrbanPercentage / 100.0f;
            } else {
                m_UrbanRuralPercentageInfo.SetActive(false);
            }
        }
    }
}
