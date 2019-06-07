namespace CrowdedEarth.Data.Model {
    public interface IPopulationInfo {
        int TotalPopulation { get; }
        float MalePercentage { get; }
        float FemalePercentage { get; }
    }
}
