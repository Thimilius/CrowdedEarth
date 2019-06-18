using System;
using UnityEngine;
using UnityEngine.UI;
using CrowdedEarth.Visualization;
using TMPro;

namespace CrowdedEarth.UI {
    public class VisualizationModeUI : MonoBehaviour {
        [SerializeField] private EarthVisualizer m_Visualizer;
        [SerializeField] private Button m_VisualizationButton;
        [SerializeField] private TMP_Text m_VisualizationButtonText;

        private void Start() {
            m_VisualizationButton.onClick.AddListener(() => {
                if (m_Visualizer.VisualizationMode == VisualizationMode.Info) {
                    m_Visualizer.SetVisualizationMode(VisualizationMode.Real);
                    m_VisualizationButtonText.text = "\uf57c";
                } else {
                    m_Visualizer.SetVisualizationMode(VisualizationMode.Info);
                    m_VisualizationButtonText.text = "\uf0ac";
                }
            });
        }
    }
}
