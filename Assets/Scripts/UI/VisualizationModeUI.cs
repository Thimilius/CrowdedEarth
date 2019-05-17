using System;
using UnityEngine;
using UnityEngine.UI;
using CrowdedEarth.Visualization;

namespace CrowdedEarth.UI {
    public class VisualizationModeUI : MonoBehaviour {
        [SerializeField] private Visualizer m_Visualizer;
        [SerializeField] private Dropdown m_VisualizationDropdown;

        private void Start() {
            m_VisualizationDropdown.onValueChanged.AddListener(value => {
                var mode = (VisualizationMode)Enum.Parse(typeof(VisualizationMode), m_VisualizationDropdown.options[value].text);
                m_Visualizer.SetVisualizationMode(mode);
            });
        }
    }
}
