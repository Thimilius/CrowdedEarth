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
            public IList<IPopulationInfo> PopulationInfo { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }

        private class PopulationInfo : IPopulationInfo {
            public int TotalPopulation { get; set; }
            public float MalePercentage { get; set; }
            public float FemalePercentage { get; set; }
        }

        private class ReadDataResult<T> {
            public IDictionary<string, T> Data { get; set; }
            public IList<PropertyInfo> Properties { get; set; }
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

            // Load total population
            ReadDataResult<PopulationAbsoluteLayout> populationTotal = ReadDataWithProperties<PopulationAbsoluteLayout>(POPULATION_TOTAL_PATH);

            // Load population gender percentages
            ReadDataResult<PopulationPercentageLayout> populationMalePercentage = ReadDataWithProperties<PopulationPercentageLayout>(POPULATION_MALE_PERCENTAGE_PATH);
            ReadDataResult<PopulationPercentageLayout> populationFemalePercentage = ReadDataWithProperties<PopulationPercentageLayout>(POPULATION_FEMALE_PERCENTAGE_PATH);

            foreach (PopulationAbsoluteLayout entry in populationTotal.Data.Values) {
                string name = entry.Country;
                var country = new Country() {
                    Name = name,
                    PopulationInfo = new List<IPopulationInfo>(YEAR_COUNT)
                };

                for (int i = 0; i < YEAR_COUNT; i++) {
                    int total = (int)populationTotal.Properties[i].GetValue(entry);

                    float malePercentage = -1;
                    if (populationMalePercentage.Data.ContainsKey(name)) {
                        PopulationPercentageLayout percentageEntry = populationMalePercentage.Data[name];
                        malePercentage = (float)populationMalePercentage.Properties[i].GetValue(percentageEntry);
                    }

                    float femalePercentage = -1;
                    if (populationFemalePercentage.Data.ContainsKey(name)) {
                        PopulationPercentageLayout percentageEntry = populationFemalePercentage.Data[name];
                        femalePercentage = (float)populationFemalePercentage.Properties[i].GetValue(percentageEntry);
                    }

                    country.PopulationInfo.Add(new PopulationInfo() {
                        TotalPopulation = total,
                        MalePercentage = malePercentage,
                        FemalePercentage = femalePercentage
                    });
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
            result.Properties = properties.ToList();
            return result;
        }
    }
}
