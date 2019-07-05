namespace CrowdedEarth.Data.Model {
    public interface IPopulationInfo {
        int PopulationTotal { get; }

        float MalePercentage { get; }
        float FemalePercentage { get; }

        float RuralPercentage { get; }
        float UrbanPercentage { get; }

        int Age0_9MaleAbsolute { get; }
        int Age10_19MaleAbsolute { get; }
        int Age20_29MaleAbsolute { get; }
        int Age30_39MaleAbsolute { get; }
        int Age40_49MaleAbsolute { get; }
        int Age50_59MaleAbsolute { get; }
        int Age60_69MaleAbsolute { get; }
        int Age70_79MaleAbsolute { get; }
        int Age80_AboveMaleAbsolute { get; }

        int Age0_9FemaleAbsolute { get; }
        int Age10_19FemaleAbsolute { get; }
        int Age20_29FemaleAbsolute { get; }
        int Age30_39FemaleAbsolute { get; }
        int Age40_49FemaleAbsolute { get; }
        int Age50_59FemaleAbsolute { get; }
        int Age60_69FemaleAbsolute { get; }
        int Age70_79FemaleAbsolute { get; }
        int Age80_AboveFemaleAbsolute { get; }
    }
}
