namespace CrowdedEarth.Data.Model {
    public interface IPopulationInfo {
        int PopulationTotal { get; }

        float MalePercentage { get; }
        float FemalePercentage { get; }

        float RuralPercentage { get; }
        float UrbanPercentage { get; }

        int Age0_14MaleAbsolute { get; }
        int Age15_64MaleAbsolute { get; }
        int Age64_AboveMaleAbsolute { get; }

        int Age0_14FemaleAbsolute { get; }
        int Age15_64FemaleAbsolute { get; }
        int Age64_AboveFemaleAbsolute { get; }
    }
}
