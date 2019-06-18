using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using CrowdedEarth.Visualization;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.UI {
    public class CountryUI : MonoBehaviour {
        [SerializeField] private CountryVisualizer m_Visualizer;
        [SerializeField] private Image m_CountryFlag;
        [SerializeField] private TMP_Text m_CountryNameText;
        [SerializeField] private Button m_BackToEarthButton;

        private void Start() {
            m_BackToEarthButton.onClick.AddListener(() => {
                SceneManager.LoadScene(0);
            });

            ICountry country = m_Visualizer.Country;
            m_CountryFlag.sprite = SpriteManager.GetFlag(country.Flag);
            m_CountryNameText.text = country.NameGerman;
        }
    }
}
