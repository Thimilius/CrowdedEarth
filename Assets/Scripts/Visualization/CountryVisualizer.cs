using CrowdedEarth.Data;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.Visualization {
    public class CountryVisualizer : Visualizer {
        private static ICountry s_Country;
        public ICountry Country => s_Country;

        private void Start() {
            // This is just so we can start the scene normally
            if (s_Country == null) {
                s_Country = DataLoader.GetCountries().Find(c => c.Name == "Germany");
            }
        }

        public static void SetCountry(ICountry country) {
            s_Country = country;
        }
    }
}
