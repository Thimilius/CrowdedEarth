using System.Collections.Generic;

namespace CrowdedEarth.Data.Model {
    public interface ICountry {
        string Name { get; }
        IList<int> Population { get; }
        float Latitude { get; }
        float Longitude { get; }
    }
}
