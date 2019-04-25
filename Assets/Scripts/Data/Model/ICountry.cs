using System.Collections.Generic;

namespace CrowdedEarth.Data.Model {
    public interface ICountry {
        string Name { get; }
        List<int> Population { get; }
        float Latitude { get; }
        float Longitude { get; }
    }
}
