using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CsvHelper;
using System.Linq;
using CrowdedEarth.Data.Model;
using CrowdedEarth.Data.Layout;

namespace CrowdedEarth.Data {
    public delegate void GetCountriesHandler(ICountry country, bool success);

    // NOTE: Countries that need to be stripped from the file because of insufficient data:
    //       - Eritrea
    //       - Serbia
    //       - Sint Maarten (Dutch part)
    //       - Sint Maarten (French part)
    //       - West Bank and Gaza
    //       - Kuwait
    //       - Micronesia
    //       - Macao SAR, China
    //       - Greenland
    //       - Bermuda
    public static class DataLoader {
        private class Country : ICountry {
            public string Name { get; set; }
            public List<int> Population { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }

        private static readonly string DATA_PATH = Path.Combine(Application.dataPath, "Data");

        public static void GetCountries(GetCountriesHandler callback) {
            string populationPath = Path.Combine(DATA_PATH, "population_total.csv");
            string locationPath = Path.Combine(DATA_PATH, "locations.csv");

            List<Location> locations = new List<Location>();
            using (var reader = new StreamReader(locationPath)) {
                using (var csv = new CsvReader(reader)) {
                    var entries = csv.GetRecords<Location>();
                    locations.AddRange(entries);
                }
            }

            const int historySize = 28;
            using (var reader = new StreamReader(populationPath)) {
                using (var csv = new CsvReader(reader)) {
                    csv.Configuration.Delimiter = ",";
                    var entries = csv.GetRecords<CountryPopulationLayout>();

                    var properties = entries.First().GetType().GetProperties().Where(p => p.Name.StartsWith("Population"));

                    foreach (CountryPopulationLayout entry in entries) {
                        string name = entry.Country;
                        var country = new Country() {
                            Name = name,
                            Population = new List<int>(historySize)
                        };

                        foreach (var property in properties) {
                            // HACK
                            int population = (int)property.GetValue(entry);
                            country.Population.Add(population);
                        }

                        Location location = locations.Find(l => l.Name == country.Name);
                        if (location == null) {
                            Debug.LogError($"[DataLoader] - Failed to get location for country: {name}!");
                            continue;
                        }

                        country.Latitude = location.Latitude;
                        country.Longitude = location.Longitude;
                        callback?.Invoke(country, true);
                    }
                }
            }
        }
    }
}
