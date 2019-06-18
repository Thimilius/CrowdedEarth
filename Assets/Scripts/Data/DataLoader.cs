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
            public string ID { get; set; }
            public string Name { get; set; }

            public float Latitude { get; set; }
            public float Longitude { get; set; }

            public int Size { get; set; }

            public string Flag { get; set; }

            public IList<IPopulationInfo> PopulationInfo { get; set; }
        }

        private class PopulationInfo : IPopulationInfo {
            public int PopulationTotal { get; set; }

            public float MalePercentage { get; set; }
            public float FemalePercentage { get; set; }

            public float RuralPercentage { get; set; }
            public float UrbanPercentage { get; set; }

            public int Age0_14MaleAbsolute { get; set; }
            public int Age15_64MaleAbsolute { get; set; }
            public int Age64_AboveMaleAbsolute { get; set; }

            public int Age0_14FemaleAbsolute { get; set; }
            public int Age15_64FemaleAbsolute { get; set; }
            public int Age64_AboveFemaleAbsolute { get; set; }
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
        private static readonly string POPULATION_RURAL_PERCENTAGE_PATH = Path.Combine(DATA_PATH, "population_rural_percentage.csv");
        private static readonly string POPULATION_URBAN_PERCENTAGE_PATH = Path.Combine(DATA_PATH, "population_urban_percentage.csv");
        private static readonly string POPULATION_AGE_0_14_MALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_0-14_male_absolute.csv");
        private static readonly string POPULATION_AGE_15_64_MALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_15-64_male_absolute.csv");
        private static readonly string POPULATION_AGE_65_ABOVE_MALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_65-above_male_absolute.csv");
        private static readonly string POPULATION_AGE_0_14_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_0-14_female_absolute.csv");
        private static readonly string POPULATION_AGE_15_64_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_15-64_female_absolute.csv");
        private static readonly string POPULATION_AGE_65_ABOVE_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_65-above_female_absolute.csv");

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

            // Load population specific data
            ReadDataResult<PopulationPercentageLayout> populationMalePercentage = ReadDataWithProperties<PopulationPercentageLayout>(POPULATION_MALE_PERCENTAGE_PATH);
            ReadDataResult<PopulationPercentageLayout> populationFemalePercentage = ReadDataWithProperties<PopulationPercentageLayout>(POPULATION_FEMALE_PERCENTAGE_PATH);
            ReadDataResult<PopulationPercentageLayout> populationRuralPercentage = ReadDataWithProperties<PopulationPercentageLayout>(POPULATION_RURAL_PERCENTAGE_PATH);
            ReadDataResult<PopulationPercentageLayout> populationUrbanPercentage = ReadDataWithProperties<PopulationPercentageLayout>(POPULATION_URBAN_PERCENTAGE_PATH);
            ReadDataResult<PopulationAbsoluteLayout> populationAge0_14MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(POPULATION_AGE_0_14_MALE_ABSOLUTE_PATH);
            ReadDataResult<PopulationAbsoluteLayout> populationAge15_64MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(POPULATION_AGE_15_64_MALE_ABSOLUTE_PATH);
            ReadDataResult<PopulationAbsoluteLayout> populationAge65_AboveMaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(POPULATION_AGE_65_ABOVE_MALE_ABSOLUTE_PATH);
            ReadDataResult<PopulationAbsoluteLayout> populationAge0_14FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(POPULATION_AGE_0_14_FEMALE_ABSOLUTE_PATH);
            ReadDataResult<PopulationAbsoluteLayout> populationAge15_64FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(POPULATION_AGE_15_64_FEMALE_ABSOLUTE_PATH);
            ReadDataResult<PopulationAbsoluteLayout> populationAge65_AboveFemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(POPULATION_AGE_65_ABOVE_FEMALE_ABSOLUTE_PATH);

            foreach (var country in countries) {
                string name = country.ID;
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

                    // Get rural percentag if existent
                    float ruralPercentage = -1;
                    if (populationRuralPercentage.Data.ContainsKey(name)) {
                        PopulationPercentageLayout percentageEntry = populationRuralPercentage.Data[name];
                        ruralPercentage = (float)populationRuralPercentage.Properties[i].GetValue(percentageEntry);
                    }

                    // Get urban percentag if existent
                    float urbanPercentage = -1;
                    if (populationUrbanPercentage.Data.ContainsKey(name)) {
                        PopulationPercentageLayout percentageEntry = populationUrbanPercentage.Data[name];
                        urbanPercentage = (float)populationUrbanPercentage.Properties[i].GetValue(percentageEntry);
                    }

                    // Get male age group 0-14
                    int age0_14MaleAbsolute = -1;
                    if (populationAge0_14MaleAbsolute.Data.ContainsKey(name)) {
                        PopulationAbsoluteLayout absoluteEntry = populationAge0_14MaleAbsolute.Data[name];
                        age0_14MaleAbsolute = (int)populationAge0_14MaleAbsolute.Properties[i].GetValue(absoluteEntry);
                    }

                    // Get male age group 15-64
                    int age15_64MaleAbsolute = -1;
                    if (populationAge15_64MaleAbsolute.Data.ContainsKey(name)) {
                        PopulationAbsoluteLayout absoluteEntry = populationAge15_64MaleAbsolute.Data[name];
                        age15_64MaleAbsolute = (int)populationAge15_64MaleAbsolute.Properties[i].GetValue(absoluteEntry);
                    }

                    // Get male age group 65-above
                    int age65_AboveMaleAbsolute = -1;
                    if (populationAge65_AboveMaleAbsolute.Data.ContainsKey(name)) {
                        PopulationAbsoluteLayout absoluteEntry = populationAge65_AboveMaleAbsolute.Data[name];
                        age65_AboveMaleAbsolute = (int)populationAge65_AboveMaleAbsolute.Properties[i].GetValue(absoluteEntry);
                    }

                    // Get female age group 0-14
                    int age0_14FemaleAbsolute = -1;
                    if (populationAge0_14FemaleAbsolute.Data.ContainsKey(name)) {
                        PopulationAbsoluteLayout absoluteEntry = populationAge0_14FemaleAbsolute.Data[name];
                        age0_14FemaleAbsolute = (int)populationAge0_14FemaleAbsolute.Properties[i].GetValue(absoluteEntry);
                    }

                    // Get female age group 15-64
                    int age15_64FemaleAbsolute = -1;
                    if (populationAge15_64FemaleAbsolute.Data.ContainsKey(name)) {
                        PopulationAbsoluteLayout absoluteEntry = populationAge15_64FemaleAbsolute.Data[name];
                        age15_64FemaleAbsolute = (int)populationAge15_64FemaleAbsolute.Properties[i].GetValue(absoluteEntry);
                    }

                    // Get female age group 65-above
                    int age65_AboveFemaleAbsolute = -1;
                    if (populationAge65_AboveMaleAbsolute.Data.ContainsKey(name)) {
                        PopulationAbsoluteLayout absoluteEntry = populationAge65_AboveFemaleAbsolute.Data[name];
                        age65_AboveFemaleAbsolute = (int)populationAge65_AboveFemaleAbsolute.Properties[i].GetValue(absoluteEntry);
                    }

                    // Add the population info 
                    country.PopulationInfo.Add(new PopulationInfo() {
                        PopulationTotal = total,

                        MalePercentage = malePercentage,
                        FemalePercentage = femalePercentage,

                        RuralPercentage = ruralPercentage,
                        UrbanPercentage = urbanPercentage,

                        Age0_14MaleAbsolute = age0_14MaleAbsolute,
                        Age15_64MaleAbsolute = age15_64MaleAbsolute,
                        Age64_AboveMaleAbsolute = age65_AboveMaleAbsolute,

                        Age0_14FemaleAbsolute = age0_14FemaleAbsolute,
                        Age15_64FemaleAbsolute = age15_64FemaleAbsolute,
                        Age64_AboveFemaleAbsolute = age65_AboveFemaleAbsolute
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
