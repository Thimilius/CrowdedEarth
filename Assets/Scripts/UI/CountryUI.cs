using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using CrowdedEarth.Visualization;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.UI {
    public class CountryUI : MonoBehaviour {
        [SerializeField] private CountryVisualizer m_Visualizer;
        [SerializeField] private Button m_BackToEarthButton;
        [Header("General Info")]
        [SerializeField] private Image m_CountryFlag;
        [SerializeField] private TMP_Text m_CountryNameText;
        [SerializeField] private TMP_Text m_PopulationText;
        [SerializeField] private TMP_Text m_SizeText;
        [SerializeField] private TMP_Text m_DensityText;
        [Header("Male/Female Info")]
        [SerializeField] private TMP_Text m_MalePercentageText;
        [SerializeField] private TMP_Text m_FemalePercentageText;
        [Header("Urban/Rural Info")]
        [SerializeField] private TMP_Text m_UrbanPercentageText;
        [SerializeField] private TMP_Text m_RuralPercentageText;
        [Header("Age Info")]
        [SerializeField] private TMP_Text m_AgeGroupInfo;
        [SerializeField] private float m_HoverY;

        private void Awake() {
            m_Visualizer.OnYearChanged += OnYearChanged;
            m_Visualizer.OnVisualObjectCreated += vo => {
                vo.OnPointerEntered += () => OnPointerEntered(vo);
                vo.OnPointerExited += () => OnPointerExited(vo);
            };
        }

        private void Start() {
            m_BackToEarthButton.onClick.AddListener(() => {
                SceneManager.LoadScene(0);
            });

            ICountry country = m_Visualizer.Country;
            m_CountryFlag.sprite = SpriteManager.GetFlag(country.Flag);

            OnYearChanged();
        }

        private void OnYearChanged() {
            ICountry country = m_Visualizer.Country;
            IPopulationInfo info = country.PopulationInfo[m_Visualizer.GetYearIndex()];

            m_CountryNameText.text = country.Name;
            m_PopulationText.text = info.PopulationTotal.ToString("N0", new CultureInfo("de-DE"));
            m_SizeText.text = country.Size.ToString("N0", new CultureInfo("de-DE"));
            m_DensityText.text = (info.PopulationTotal / country.Size).ToString("0");

            m_MalePercentageText.text = info.MalePercentage.ToString("0.00");
            m_FemalePercentageText.text = info.FemalePercentage.ToString("0.00");
            m_UrbanPercentageText.text = info.UrbanPercentage.ToString("0.00");
            m_RuralPercentageText.text = info.RuralPercentage.ToString("0.00");
        }

        private void OnPointerEntered(VisualObject<AgeGroup> vo) {
            string text;
            string tag = "<size=18><color=#233C46>";
            if (m_Visualizer.IsAgeGroupMale(vo.Data)) {
                text = $"\uf222 {tag}";
            } else {
                text = $"\uf221 {tag}";
            }
            text += $"{m_Visualizer.GetAge(vo).ToString("N0", new CultureInfo("de-DE"))}</color></size>";

            Vector3 position = vo.transform.position;
            position.y = m_HoverY;
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(position);

            m_AgeGroupInfo.transform.position = screenPoint;
            m_AgeGroupInfo.text = text;
            m_AgeGroupInfo.gameObject.SetActive(true);
        }

        private void OnPointerExited(VisualObject<AgeGroup> vo) {
            m_AgeGroupInfo.gameObject.SetActive(false);
        }
    }
}
