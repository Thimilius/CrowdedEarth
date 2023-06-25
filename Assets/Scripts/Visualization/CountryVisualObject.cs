using InhabitedEarth.Data.Model;

namespace InhabitedEarth.Visualization {
    public class CountryVisualObject : VisualObject<ICountry> {
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
