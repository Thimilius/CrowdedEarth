using CrowdedEarth.Data.Model;

namespace CrowdedEarth {
    public class CountryObject : VisualObject {
        public override VisualObjectType Type => VisualObjectType.Country;

        public ICountry Country { get; set; }
    }
}
