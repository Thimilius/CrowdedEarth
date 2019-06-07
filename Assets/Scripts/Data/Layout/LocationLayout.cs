using CsvHelper.Configuration.Attributes;

namespace CrowdedEarth.Data.Layout {
    public class LocationLayout : DataLayout {
        [Index(0)] public override string Country { get; set; }
        [Index(1)] public float Latitude { get; set; }
        [Index(2)] public float Longitude { get; set; }
    }
}
