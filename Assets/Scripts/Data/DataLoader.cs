using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
            public IList<int> Population { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }

        private class ReadDataResult<T> {
            public IDictionary<string, T> Data { get; set; }
            public IEnumerable<PropertyInfo> Properties { get; set; }
        }

        private static readonly string DATA_PATH = Path.Combine(Application.dataPath, "Data");
        private static readonly string LOCATION_PATH = Path.Combine(DATA_PATH, "locations.csv");
        private static readonly string POPULATION_TOTAL_PATH = Path.Combine(DATA_PATH, "population_total.csv");
        private static readonly string POPULATION_FEMALE_PERCENTAGE_PATH = Path.Combine(DATA_PATH, "population_female_percentage.csv");
        private static readonly string POPULATION_MALE_PERCENTAGE_PATH = Path.Combine(DATA_PATH, "population_male_percentage.csv");

        public static void GetCountries(GetCountriesHandler callback) {
            // HACK: We have a constant for how many years the data contains
            const int YEAR_COUNT = 90;

            // Load locations (latitude and longitude)
            ReadDataResult<LocationLayout> locations = ReadData<LocationLayout>(LOCATION_PATH);

            // Load population gender percentages
            ReadDataResult<PopulationPercentageLayout> populationFemalePercentage = ReadDataWithProperties<PopulationPercentageLayout>(POPULATION_FEMALE_PERCENTAGE_PATH);
            ReadDataResult<PopulationPercentageLayout> populationMalePercentage = ReadDataWithProperties<PopulationPercentageLayout>(POPULATION_MALE_PERCENTAGE_PATH);

            // Load total population
            ReadDataResult<PopulationAbsoluteLayout> populationTotal = ReadDataWithProperties<PopulationAbsoluteLayout>(POPULATION_TOTAL_PATH);

            foreach (PopulationAbsoluteLayout entry in populationTotal.Data.Values) {
                string name = entry.Country;
                var country = new Country() {
                    Name = name,
                    Population = new List<int>(YEAR_COUNT)
                };

                foreach (var property in populationTotal.Properties) {
                    int population = (int)property.GetValue(entry);
                    country.Population.Add(population);
                }

                if (locations.Data.ContainsKey(name) == false) {
                    Debug.LogError($"[DataLoader] - Failed to get location for country: {name}!");
                } else {
                    LocationLayout location = locations.Data[name];
                    country.Latitude = location.Latitude;
                    country.Longitude = location.Longitude;
                    callback?.Invoke(country, true);
                }
            }
        }

        private static ReadDataResult<T> ReadData<T>(string path) where T : DataLayout {
            using (var reader = new StreamReader(path)) {
                using (var csv = new CsvReader(reader)) {
                    csv.Configuration.Delimiter = ",";
                    var entries = csv.GetRecords<T>();
                    var data = entries.ToDictionary(l => l.Country);
                    return new ReadDataResult<T>() { Data = data };
                }
            }
        }

        private static ReadDataResult<T> ReadDataWithProperties<T>(string path) where T : DataLayout {
            ReadDataResult<T> result = ReadData<T>(path);
            var properties = result.Data.First().Value.GetType().GetProperties().Where(p => p.Name.StartsWith("Value"));
            result.Properties = properties;
            return result;
        }
    }
}
