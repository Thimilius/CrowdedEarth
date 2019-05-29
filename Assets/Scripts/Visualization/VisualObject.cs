using UnityEngine;
using CrowdedEarth.Data.Model;
using UnityEngine.EventSystems;

namespace CrowdedEarth.Visualization {
    public class VisualObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        [SerializeField] private Color m_HighlightColor;

        public VisualObjectType Type { get; set; }

        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public ICountry Country { get; set; }

        private Renderer m_Renderer;
        private Color m_NormalColor;

        private void Awake() {
            m_Renderer = GetComponent<Renderer>();
        }

        public void SetColor(Color color) {
            m_NormalColor = color;
            m_Renderer.material.color = color;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            m_Renderer.material.color = m_HighlightColor;
        }

        public void OnPointerExit(PointerEventData eventData) {
            m_Renderer.material.color = m_NormalColor;
        }
    }
}
