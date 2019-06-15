using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrowdedEarth.Visualization;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.UI {
    public class DetailPanelUI : MonoBehaviour {
        [SerializeField] private Visualizer m_Visualizer;
        [SerializeField] private GameObject m_InfoPanel;
        [Header("General Info")]
        [SerializeField] private TMP_Text m_CountryText;
        [SerializeField] private TMP_Text m_PopulationText;
        [Header("Male/Female Info")]
        [SerializeField] private GameObject m_MaleFemalePercentageInfo;
        [SerializeField] private TMP_Text m_MalePercentageText;
        [SerializeField] private TMP_Text m_FemalePercentageText;
        [SerializeField] private Image m_MalePercentageImage;
        [SerializeField] private Image m_FemalePercentageImage;

        private void Awake() {
            m_Visualizer.OnVisualObjectCreated += vo => {
                vo.OnPointerEntered += () => OnPointerEntered(vo);
                vo.OnPointerExited  += () => OnPointerExited(vo);
            };
        }

        private void OnPointerEntered(VisualObject vo) {
            m_InfoPanel.SetActive(true);

            IPopulationInfo info = vo.Country.PopulationInfo[m_Visualizer.GetYearIndex()];

            m_CountryText.text = vo.Country.NameGerman;
            m_PopulationText.text = $"Population: {info.TotalPopulation.ToString("N0", new CultureInfo("de-DE"))}";

            if (info.MalePercentage > 0 && info.FemalePercentage > 0) {
                m_MaleFemalePercentageInfo.SetActive(true);
                m_MalePercentageText.text = $"{info.MalePercentage.ToString("0.00")} %";
                m_FemalePercentageText.text = $"{info.FemalePercentage.ToString("0.00")} %";
                m_MalePercentageImage.fillAmount = info.MalePercentage / 100.0f;
            } else {
                m_MaleFemalePercentageInfo.SetActive(false);
            }
        }

        private void OnPointerExited(VisualObject vo) {
            m_InfoPanel.SetActive(false);
        }
    }
}