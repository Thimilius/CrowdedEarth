using CrowdedEarth.Data.Model;

namespace CrowdedEarth.Visualization {
    public class CountryVisualObject : VisualObject<ICountry> {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
