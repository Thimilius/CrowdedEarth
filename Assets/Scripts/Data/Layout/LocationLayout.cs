using CsvHelper.Configuration.Attributes;

namespace CrowdedEarth.Data.Layout {
    public class Location {
        [Index(0)] public string Name { get; set; }
        [Index(1)] public float Latitude { get; set; }
        [Index(2)] public float Longitude { get; set; }
    }
}
