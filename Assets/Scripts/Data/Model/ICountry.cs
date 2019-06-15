using System.Collections.Generic;

namespace CrowdedEarth.Data.Model {
    public interface ICountry {
        string Name { get; }
        string NameGerman { get; }

        float Latitude { get; }
        float Longitude { get; }

        IList<IPopulationInfo> PopulationInfo { get; }

        string Flag { get; }
    }
}
