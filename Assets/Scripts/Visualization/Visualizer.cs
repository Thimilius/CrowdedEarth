using System;
using UnityEngine;

namespace CrowdedEarth.Visualization {
    public abstract class Visualizer : MonoBehaviour {
        public event Action OnYearChanged;

        private int m_Year;

        public virtual void SetYear(int year) {
            m_Year = year;

            OnYearChanged?.Invoke();
        }

        public int GetYearIndex() {
            const int YEAR_LIMIT_MIN = 1960;
            const int YEAR_LIMIT_MAX = 2050;

            // HACK: Hardcoded!
            int year = Mathf.Clamp(m_Year, YEAR_LIMIT_MIN, YEAR_LIMIT_MAX);
            return year - YEAR_LIMIT_MIN;
        }
    }
}
