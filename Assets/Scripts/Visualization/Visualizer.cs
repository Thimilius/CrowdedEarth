using System;
using UnityEngine;

namespace CrowdedEarth.Visualization {
    public abstract class Visualizer : MonoBehaviour {
        public event Action OnYearChanged;

        private static int s_Year = 1960;
        public int Year => s_Year;

        public virtual void SetYear(int year) {
            s_Year = year;

            OnYearChanged?.Invoke();
        }

        public int GetYearIndex() {
            // Those hardcoded limits are bad but work
            const int YEAR_LIMIT_MIN = 1960;
            const int YEAR_LIMIT_MAX = 2050;

            int year = Mathf.Clamp(s_Year, YEAR_LIMIT_MIN, YEAR_LIMIT_MAX);
            return year - YEAR_LIMIT_MIN;
        }
    }
}
