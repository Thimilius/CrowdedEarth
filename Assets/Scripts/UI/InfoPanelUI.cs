using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrowdedEarth.Visualization;
using System.Globalization;

namespace CrowdedEarth.UI {
    public class InfoPanelUI : MonoBehaviour {
        [SerializeField] private Visualizer m_Visualizer;
        [SerializeField] private GameObject m_InfoPanel;
        [SerializeField] private TMP_Text m_CountryText;
        [SerializeField] private TMP_Text m_PopulationText;

        private void Awake() {
            m_Visualizer.OnVisualObjectCreated += vo => {
                vo.OnPointerEntered += () => OnPointerEntered(vo);
                vo.OnPointerExited  += () => OnPointerExited(vo);
            };
        }

        private void OnPointerEntered(VisualObject vo) {
            m_InfoPanel.SetActive(true);

            m_CountryText.text = vo.Country.Name;
            m_PopulationText.text = $"Population: {vo.Country.Population[m_Visualizer.GetYearIndex()].ToString("N0", new CultureInfo("de-DE"))}";
        }

        private void OnPointerExited(VisualObject vo) {
            m_InfoPanel.SetActive(false);
        }
    }
}