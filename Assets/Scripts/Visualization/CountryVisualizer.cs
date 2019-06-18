using CrowdedEarth.Data.Model;

namespace CrowdedEarth.Visualization {
    public class CountryVisualizer : Visualizer {
        private static ICountry s_Country;
        public ICountry Country => s_Country;

        private void Start() {
                
        }

        public static void SetCountry(ICountry country) {
            s_Country = country;
        }
    }
}
