﻿using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using InhabitedEarth.Data.Model;
using InhabitedEarth.Visualization;

namespace InhabitedEarth.UI {
    public class DetailPanelUI : MonoBehaviour {
        [SerializeField] private EarthVisualizer m_Visualizer;
        [SerializeField] private GameObject m_InfoPanel;
        [Header("General Info")]
        [SerializeField] private TMP_Text m_CountryText;
        [SerializeField] private Image m_CountryFlag;
        [SerializeField] private TMP_Text m_PopulationText;
        [SerializeField] private TMP_Text m_DensityText;

        private void Awake() {
            m_Visualizer.OnVisualObjectCreated += vo => {
                vo.OnPointerEntered += () => OnPointerEntered(vo);
                vo.OnPointerExited  += () => OnPointerExited(vo);
            };
        }

        private void OnPointerEntered(VisualObject<ICountry> vo) {
            m_InfoPanel.SetActive(true);

            ICountry country = vo.Data;
            IPopulationInfo info = country.PopulationInfo[m_Visualizer.GetYearIndex()];

            // Set the flag with correct aspect ratio
            Sprite flag = SpriteManager.GetFlag(country.Flag);
            float aspectRatio = flag.rect.width / flag.rect.height;
            float height = m_CountryFlag.rectTransform.sizeDelta.y;
            m_CountryFlag.sprite = flag;
            m_CountryFlag.rectTransform.sizeDelta = new Vector2(aspectRatio * height, height);

            m_CountryText.text = country.Name;
            m_PopulationText.text = info.PopulationTotal.ToString("N0", new CultureInfo("de-DE"));
            m_DensityText.text = (info.PopulationTotal / country.Size).ToString("0");
        }

        private void OnPointerExited(VisualObject<ICountry> vo) {
            m_InfoPanel.SetActive(false);
        }
    }
}