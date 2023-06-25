using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InhabitedEarth.UI {
    public class InfoPanelUI : MonoBehaviour {
        [SerializeField] private GameObject m_InfoPanel;
        [SerializeField] private Button m_InfoButton;
        [SerializeField] private TMP_Text m_InfoButtonText;
        [SerializeField] private Color m_IsOpenColor;

        private void Start() {
            m_InfoButton.onClick.AddListener(() => {
                m_InfoPanel.SetActive(!m_InfoPanel.activeSelf);
                m_InfoButtonText.color = m_InfoPanel.activeSelf ? m_IsOpenColor : Color.white;
            });
        }
    }
}
