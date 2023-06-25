using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InhabitedEarth.Visualization;

namespace InhabitedEarth.UI {
    public class TimelineUI : MonoBehaviour {
        [SerializeField] private Visualizer m_Visualizer;
        [SerializeField] private Slider m_TimelineSlider;
        [SerializeField] private TMP_Text m_TimelineYearText;

        private void Start() {
            m_TimelineSlider.value = m_Visualizer.Year;
            m_TimelineYearText.text = m_Visualizer.Year.ToString();

            m_TimelineSlider.onValueChanged.AddListener(value => {
                int year = (int)value;
                m_TimelineYearText.text = year.ToString();
                m_Visualizer.SetYear(year);
            });
        }
    }
}
