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

            public int Age0_9MaleAbsolute { get; set; }
            public int Age10_19MaleAbsolute { get; set; }
            public int Age20_29MaleAbsolute { get; set; }
            public int Age30_39MaleAbsolute { get; set; }
            public int Age40_49MaleAbsolute { get; set; }
            public int Age50_59MaleAbsolute { get; set; }
            public int Age60_69MaleAbsolute { get; set; }
            public int Age70_79MaleAbsolute { get; set; }
            public int Age80_AboveMaleAbsolute { get; set; }

            public int Age0_9FemaleAbsolute { get; set; }
            public int Age10_19FemaleAbsolute { get; set; }
            public int Age20_29FemaleAbsolute { get; set; }
            public int Age30_39FemaleAbsolute { get; set; }
            public int Age40_49FemaleAbsolute { get; set; }
            public int Age50_59FemaleAbsolute { get; set; }
            public int Age60_69FemaleAbsolute { get; set; }
            public int Age70_79FemaleAbsolute { get; set; }
            public int Age80_AboveFemaleAbsolute { get; set; }
        }

        private class ReadDataResult<T> {
            public IDictionary<string, T> Data { get; set; }
            public IList<PropertyInfo> Properties { get; set; }
        }

        private static List<ICountry> m_Countries;

        public static List<ICountry> GetCountries() {
            return m_Countries;
        }

        public static void LoadCountries() {
            m_Countries = new List<ICountry>();

            // This constant for how many years the data contains is a little hackey but works
            const int YEAR_COUNT = 90;

            // Load general country information
            string json = File.ReadAllText(DataLocations.COUNTRIES_PATH);
            List<Country> countries = JsonConvert.DeserializeObject<List<Country>>(json);

            var populationAge20_24MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_20_24_MALE_ABSOLUTE_PATH);

            // Load total population
            var populationTotal = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_TOTAL_PATH);
            
            // Load percentages
            var populationMalePercentage = ReadDataWithProperties<PopulationPercentageLayout>(DataLocations.POPULATION_MALE_PERCENTAGE_PATH);
            var populationFemalePercentage = ReadDataWithProperties<PopulationPercentageLayout>(DataLocations.POPULATION_FEMALE_PERCENTAGE_PATH);
            var populationRuralPercentage = ReadDataWithProperties<PopulationPercentageLayout>(DataLocations.POPULATION_RURAL_PERCENTAGE_PATH);
            var populationUrbanPercentage = ReadDataWithProperties<PopulationPercentageLayout>(DataLocations.POPULATION_URBAN_PERCENTAGE_PATH);

            // Load male age groups
            var populationAge0_4MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_0_4_MALE_ABSOLUTE_PATH);
            var populationAge5_9MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_4_9_MALE_ABSOLUTE_PATH);
            var populationAge10_14MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_10_14_MALE_ABSOLUTE_PATH);
            var populationAge15_19MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_15_19_MALE_ABSOLUTE_PATH);
            var populationAge25_29MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_25_29_MALE_ABSOLUTE_PATH);
            var populationAge30_34MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_30_34_MALE_ABSOLUTE_PATH);
            var populationAge35_39MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_35_39_MALE_ABSOLUTE_PATH);
            var populationAge40_44MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_40_44_MALE_ABSOLUTE_PATH);
            var populationAge45_49MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_45_49_MALE_ABSOLUTE_PATH);
            var populationAge50_54MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_50_54_MALE_ABSOLUTE_PATH);
            var populationAge55_59MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_55_59_MALE_ABSOLUTE_PATH);
            var populationAge60_64MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_60_64_MALE_ABSOLUTE_PATH);
            var populationAge65_69MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_65_69_MALE_ABSOLUTE_PATH);
            var populationAge70_74MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_70_74_MALE_ABSOLUTE_PATH);
            var populationAge75_79MaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_75_79_MALE_ABSOLUTE_PATH);
            var populationAge80_AboveMaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_80_ABOVE_MALE_ABSOLUTE_PATH);

            // Load female age groups
            var populationAge0_4FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_0_4_FEMALE_ABSOLUTE_PATH);
            var populationAge5_9FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_4_9_FEMALE_ABSOLUTE_PATH);
            var populationAge10_14FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_10_14_FEMALE_ABSOLUTE_PATH);
            var populationAge15_19FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_15_19_FEMALE_ABSOLUTE_PATH);
            var populationAge20_24FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_20_24_FEMALE_ABSOLUTE_PATH);
            var populationAge25_29FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_25_29_FEMALE_ABSOLUTE_PATH);
            var populationAge30_34FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_30_34_FEMALE_ABSOLUTE_PATH);
            var populationAge35_39FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_35_39_FEMALE_ABSOLUTE_PATH);
            var populationAge40_44FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_40_44_FEMALE_ABSOLUTE_PATH);
            var populationAge45_49FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_45_49_FEMALE_ABSOLUTE_PATH);
            var populationAge50_54FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_50_54_FEMALE_ABSOLUTE_PATH);
            var populationAge55_59FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_55_59_FEMALE_ABSOLUTE_PATH);
            var populationAge60_64FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_60_64_FEMALE_ABSOLUTE_PATH);
            var populationAge65_69FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_65_69_FEMALE_ABSOLUTE_PATH);
            var populationAge70_74FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_70_74_FEMALE_ABSOLUTE_PATH);
            var populationAge75_79FemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_75_79_FEMALE_ABSOLUTE_PATH);
            var populationAge80_AboveFemaleAbsolute = ReadDataWithProperties<PopulationAbsoluteLayout>(DataLocations.POPULATION_AGE_80_ABOVE_FEMALE_ABSOLUTE_PATH);

            // Fill country population info
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

                    // Add the population info 
                    country.PopulationInfo.Add(new PopulationInfo() {
                        PopulationTotal = total,

                        MalePercentage = GetPercentageDataResult(populationMalePercentage, name, i),
                        FemalePercentage = GetPercentageDataResult(populationFemalePercentage, name, i),

                        UrbanPercentage = GetPercentageDataResult(populationUrbanPercentage, name, i),
                        RuralPercentage = GetPercentageDataResult(populationRuralPercentage, name, i),

                        Age0_9MaleAbsolute = GetAbsoluteDataResult(populationAge0_4MaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge5_9MaleAbsolute, name, i),
                        Age10_19MaleAbsolute = GetAbsoluteDataResult(populationAge10_14MaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge15_19MaleAbsolute, name, i),
                        Age20_29MaleAbsolute = GetAbsoluteDataResult(populationAge20_24MaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge25_29MaleAbsolute, name, i),
                        Age30_39MaleAbsolute = GetAbsoluteDataResult(populationAge30_34MaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge35_39MaleAbsolute, name, i),
                        Age40_49MaleAbsolute = GetAbsoluteDataResult(populationAge40_44MaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge45_49MaleAbsolute, name, i),
                        Age50_59MaleAbsolute = GetAbsoluteDataResult(populationAge50_54MaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge55_59MaleAbsolute, name, i),
                        Age60_69MaleAbsolute = GetAbsoluteDataResult(populationAge60_64MaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge65_69MaleAbsolute, name, i),
                        Age70_79MaleAbsolute = GetAbsoluteDataResult(populationAge70_74MaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge75_79MaleAbsolute, name, i),
                        Age80_AboveMaleAbsolute = GetAbsoluteDataResult(populationAge80_AboveMaleAbsolute, name, i),

                        Age0_9FemaleAbsolute = GetAbsoluteDataResult(populationAge0_4FemaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge5_9FemaleAbsolute, name, i),
                        Age10_19FemaleAbsolute = GetAbsoluteDataResult(populationAge10_14FemaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge15_19FemaleAbsolute, name, i),
                        Age20_29FemaleAbsolute = GetAbsoluteDataResult(populationAge20_24FemaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge25_29FemaleAbsolute, name, i),
                        Age30_39FemaleAbsolute = GetAbsoluteDataResult(populationAge30_34FemaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge35_39FemaleAbsolute, name, i),
                        Age40_49FemaleAbsolute = GetAbsoluteDataResult(populationAge40_44FemaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge45_49FemaleAbsolute, name, i),
                        Age50_59FemaleAbsolute = GetAbsoluteDataResult(populationAge50_54FemaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge55_59FemaleAbsolute, name, i),
                        Age60_69FemaleAbsolute = GetAbsoluteDataResult(populationAge60_64FemaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge65_69FemaleAbsolute, name, i),
                        Age70_79FemaleAbsolute = GetAbsoluteDataResult(populationAge70_74FemaleAbsolute, name, i) + GetAbsoluteDataResult(populationAge75_79FemaleAbsolute, name, i),
                        Age80_AboveFemaleAbsolute = GetAbsoluteDataResult(populationAge80_AboveFemaleAbsolute, name, i),
                    });
                }

                m_Countries.Add(country);
            }
        }

        private static float GetPercentageDataResult(ReadDataResult<PopulationPercentageLayout> result, string name, int index) {
            float value = -1;
            if (result.Data.ContainsKey(name)) {
                PopulationPercentageLayout entry = result.Data[name];
                value = (float)result.Properties[index].GetValue(entry);
            }
            return value;
        }

        private static int GetAbsoluteDataResult(ReadDataResult<PopulationAbsoluteLayout> result, string name, int index) {
            int value = -1;
            if (result.Data.ContainsKey(name)) {
                PopulationAbsoluteLayout entry = result.Data[name];
                value = (int)result.Properties[index].GetValue(entry);
            }
            return value;
        }

        private static ReadDataResult<T> ReadData<T>(string path) where T : DataLayout {
            using (var reader = new StreamReader(path)) {
                using (var csv = new CsvReader(reader)) {
                    csv.Configuration.MissingFieldFound = (headers, index, context) => FailHandler(headers, index, context, path);
                    csv.Configuration.Delimiter = ",";
                    var entries = csv.GetRecords<T>();
                    var data = entries.ToDictionary(l => l.Country);
                    return new ReadDataResult<T>() { Data = data };
                }
            }
        }

        private static void FailHandler(string[] headers, int index, ReadingContext context, string path) {
            Debug.Log($"{path} - {context.RawRow}");
        }

        private static ReadDataResult<T> ReadDataWithProperties<T>(string path) where T : DataLayout {
            ReadDataResult<T> result = ReadData<T>(path);
            var properties = result.Data.First().Value.GetType().GetProperties().Where(p => p.Name.StartsWith("Value"));
            result.Properties = properties.ToList();
            return result;
        }
    }
}
