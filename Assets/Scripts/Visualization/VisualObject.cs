﻿using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace InhabitedEarth.Visualization {
    public abstract class VisualObject<T> : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler {
        [SerializeField] private Color m_HighlightColor;
        [SerializeField] private Color m_HighlightEmissionColor;

        public T Data { get; set; }

        public event Action OnPointerEntered;
        public event Action OnPointerClicked;
        public event Action OnPointerExited;

        private Renderer m_Renderer;
        private Color m_NormalColor;
        private Color m_NormalEmissionColor;
        private int m_EmissionColorProperty;

        private void Awake() {
            m_Renderer = GetComponent<Renderer>();

            m_EmissionColorProperty = Shader.PropertyToID("_EmissionColor");
        }

        public void SetColor(Color color, Color emission) {
            m_NormalColor = color;
            m_NormalEmissionColor = emission;

            m_Renderer.material.color = color;
            m_Renderer.material.SetColor(m_EmissionColorProperty, emission);
        }

        public void OnPointerEnter(PointerEventData eventData) {
            m_Renderer.material.color = m_HighlightColor;
            m_Renderer.material.SetColor(m_EmissionColorProperty, m_HighlightEmissionColor);
            OnPointerEntered?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                OnPointerClicked?.Invoke();
            }
        }

        public void OnPointerExit(PointerEventData eventData) {
            m_Renderer.material.color = m_NormalColor;
            m_Renderer.material.SetColor(m_EmissionColorProperty, m_NormalEmissionColor);
            OnPointerExited?.Invoke();
        }
    }
}
