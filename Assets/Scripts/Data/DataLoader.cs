using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;
using CsvHelper;
using CrowdedEarth.Data.Model;
using CrowdedEarth.Data.Layout;

namespace CrowdedEarth.Data {
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
            public string NameGerman { get; set; }

            public float Latitude { get; set; }
            public float Longitude { get; set; }

            public IList<IPopulationInfo> PopulationInfo { get; set; }

            public string Flag { get; set; }
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

        private static readonly string DATA_PATH = "Data";
        private static readonly string COUNTRIES_PATH = Path.Combine(DATA_PATH, "countries.json");
        private static readonly string POPULATION_TOTAL_PATH = Path.Combine(DATA_PATH, "population_total.csv");
        private static readonly string POPULATION_FEMALE_PERCENTAGE_PATH = Path.Combine(DATA_PATH, "population_female_percentage.csv");
        private static readonly string POPULATION_MALE_PERCENTAGE_PATH = Path.Combine(DATA_PATH, "population_male_percentage.csv");

        private static bool m_DataLoaded;
        private static List<ICountry> m_Countries;

        public static List<ICountry> GetCountries() {
            if (!m_DataLoaded) {
                LoadData();
            }
            return m_Countries;
        }

        private static void LoadData() {
            m_Countries = new List<ICountry>();

            // HACK: We have a constant for how many years the data contains
            const int YEAR_COUNT = 90;

            // Load city information
            string json = File.ReadAllText(COUNTRIES_PATH);
            List<Country> countries = JsonConvert.DeserializeObject<List<Country>>(json);

            // Load total population
            ReadDataResult<PopulationAbsoluteLayout> populationTotal = ReadDataWithProperties<PopulationAbsoluteLayout>(POPULATION_TOTAL_PATH);

            // Load population gender percentages
            ReadDataResult<PopulationPercentageLayout> populationMalePercentage = ReadDataWithProperties<PopulationPercentageLayout>(POPULATION_MALE_PERCENTAGE_PATH);
            ReadDataResult<PopulationPercentageLayout> populationFemalePercentage = ReadDataWithProperties<PopulationPercentageLayout>(POPULATION_FEMALE_PERCENTAGE_PATH);

            foreach (var country in countries) {
                string name = country.Name;
                country.PopulationInfo = new List<IPopulationInfo>();

                if (populationTotal.Data.ContainsKey(name) == false) {
                    Debug.Log(name);
                    continue;
                }
                PopulationAbsoluteLayout population = populationTotal.Data[name];
                for (int i = 0; i <= YEAR_COUNT; i++) {
                    // Get total population
                    int total = (int)populationTotal.Properties[i].GetValue(population);

                    // Get male percentage if existent
                    float malePercentage = -1;
                    if (populationMalePercentage.Data.ContainsKey(name)) {
                        PopulationPercentageLayout percentageEntry = populationMalePercentage.Data[name];
                        malePercentage = (float)populationMalePercentage.Properties[i].GetValue(percentageEntry);
                    }

                    // Get female percentage if existent
                    float femalePercentage = -1;
                    if (populationFemalePercentage.Data.ContainsKey(name)) {
                        PopulationPercentageLayout percentageEntry = populationFemalePercentage.Data[name];
                        femalePercentage = (float)populationFemalePercentage.Properties[i].GetValue(percentageEntry);
                    }

                    // Add the population info 
                    country.PopulationInfo.Add(new PopulationInfo() {
                        TotalPopulation = total,
                        MalePercentage = malePercentage,
                        FemalePercentage = femalePercentage
                    });

                }

                m_Countries.Add(country);
            }

            m_DataLoaded = true;
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
