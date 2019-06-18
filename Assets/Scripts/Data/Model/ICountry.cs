using System.Collections.Generic;

namespace CrowdedEarth.Data.Model {
    public interface ICountry {
        string ID { get; }
        string Name { get; }

        float Latitude { get; }
        float Longitude { get; }

        int Size { get; }

        string Flag { get; }

        IList<IPopulationInfo> PopulationInfo { get; }
    }
}
