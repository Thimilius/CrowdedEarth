namespace CrowdedEarth.Data.Model {
    public interface ICityDetails {
        string Name { get; }
        string Country { get; }
        string Region { get; }
        float Latitude { get; }
        float Longitude { get; }
        int Population { get; }
        int ElevationMeters { get; }
    }
}

