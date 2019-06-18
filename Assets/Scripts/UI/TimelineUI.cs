using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrowdedEarth.Visualization;

namespace CrowdedEarth.UI {
    public class TimelineUI : MonoBehaviour {
        [SerializeField] private Visualizer m_Visualizer;
        [SerializeField] private Slider m_TimelineSlider;
        [SerializeField] private TMP_Text m_TimelineYearText;

        private void Start() {
            m_TimelineSlider.onValueChanged.AddListener(value => {
                int year = (int)value;
                m_TimelineYearText.text = year.ToString();
                m_Visualizer.SetYear(year);
            });
        }
    }
}
