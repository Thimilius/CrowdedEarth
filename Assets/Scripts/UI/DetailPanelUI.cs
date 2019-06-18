using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrowdedEarth.Visualization;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.UI {
    public class DetailPanelUI : MonoBehaviour {
        [SerializeField] private EarthVisualizer m_Visualizer;
        [SerializeField] private GameObject m_InfoPanel;
        [Header("General Info")]
        [SerializeField] private TMP_Text m_CountryText;
        [SerializeField] private Image m_CountryFlag;
        [SerializeField] private TMP_Text m_PopulationText;
        

        private void Awake() {
            m_Visualizer.OnVisualObjectCreated += vo => {
                vo.OnPointerEntered += () => OnPointerEntered(vo);
                vo.OnPointerExited  += () => OnPointerExited(vo);
            };
        }

        private void OnPointerEntered(VisualObject vo) {
            m_InfoPanel.SetActive(true);

            IPopulationInfo info = vo.Country.PopulationInfo[m_Visualizer.GetYearIndex()];

            // Set the flag with correct aspect ratio
            m_CountryFlag.sprite = SpriteManager.GetFlag(vo.Country.Flag);

            m_CountryText.text = vo.Country.NameGerman;
            m_PopulationText.text = $"Bevölkerung: {info.TotalPopulation.ToString("N0", new CultureInfo("de-DE"))}";

            
        }

        private void OnPointerExited(VisualObject vo) {
            m_InfoPanel.SetActive(false);
        }
    }
}