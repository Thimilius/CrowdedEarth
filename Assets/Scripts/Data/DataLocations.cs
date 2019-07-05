using System.IO;

namespace CrowdedEarth.Data {
    public static class DataLocations {
        private static readonly string DATA_PATH = "Data";

        public static readonly string COUNTRIES_PATH = Path.Combine(DATA_PATH, "countries.json");

        public static readonly string POPULATION_TOTAL_PATH = Path.Combine(DATA_PATH, "population_total.csv");

        public static readonly string POPULATION_FEMALE_PERCENTAGE_PATH = Path.Combine(DATA_PATH, "population_female_percentage.csv");
        public static readonly string POPULATION_MALE_PERCENTAGE_PATH = Path.Combine(DATA_PATH, "population_male_percentage.csv");
        public static readonly string POPULATION_RURAL_PERCENTAGE_PATH = Path.Combine(DATA_PATH, "population_rural_percentage.csv");
        public static readonly string POPULATION_URBAN_PERCENTAGE_PATH = Path.Combine(DATA_PATH, "population_urban_percentage.csv");

        public static readonly string POPULATION_AGE_0_4_MALE_ABSOLUTE_PATH      = Path.Combine(DATA_PATH, "population_0-4_male_absolute.csv");
        public static readonly string POPULATION_AGE_4_9_MALE_ABSOLUTE_PATH      = Path.Combine(DATA_PATH, "population_5-9_male_absolute.csv");
        public static readonly string POPULATION_AGE_10_14_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_10-14_male_absolute.csv");
        public static readonly string POPULATION_AGE_15_19_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_15-19_male_absolute.csv");
        public static readonly string POPULATION_AGE_20_24_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_20-24_male_absolute.csv");
        public static readonly string POPULATION_AGE_25_29_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_25-29_male_absolute.csv");
        public static readonly string POPULATION_AGE_30_34_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_30-34_male_absolute.csv");
        public static readonly string POPULATION_AGE_35_39_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_35-39_male_absolute.csv");
        public static readonly string POPULATION_AGE_40_44_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_40-44_male_absolute.csv");
        public static readonly string POPULATION_AGE_45_49_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_45-49_male_absolute.csv");
        public static readonly string POPULATION_AGE_50_54_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_50-54_male_absolute.csv");
        public static readonly string POPULATION_AGE_55_59_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_55-59_male_absolute.csv");
        public static readonly string POPULATION_AGE_60_64_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_60-64_male_absolute.csv");
        public static readonly string POPULATION_AGE_65_69_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_65-69_male_absolute.csv");
        public static readonly string POPULATION_AGE_70_74_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_70-74_male_absolute.csv");
        public static readonly string POPULATION_AGE_75_79_MALE_ABSOLUTE_PATH    = Path.Combine(DATA_PATH, "population_75-79_male_absolute.csv");
        public static readonly string POPULATION_AGE_80_ABOVE_MALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_80-above_male_absolute.csv");

        public static readonly string POPULATION_AGE_0_4_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_0-4_female_absolute.csv");
        public static readonly string POPULATION_AGE_4_9_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_5-9_female_absolute.csv");
        public static readonly string POPULATION_AGE_10_14_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_10-14_female_absolute.csv");
        public static readonly string POPULATION_AGE_15_19_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_15-19_female_absolute.csv");
        public static readonly string POPULATION_AGE_20_24_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_20-24_female_absolute.csv");
        public static readonly string POPULATION_AGE_25_29_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_25-29_female_absolute.csv");
        public static readonly string POPULATION_AGE_30_34_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_30-34_female_absolute.csv");
        public static readonly string POPULATION_AGE_35_39_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_35-39_female_absolute.csv");
        public static readonly string POPULATION_AGE_40_44_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_40-44_female_absolute.csv");
        public static readonly string POPULATION_AGE_45_49_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_45-49_female_absolute.csv");
        public static readonly string POPULATION_AGE_50_54_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_50-54_female_absolute.csv");
        public static readonly string POPULATION_AGE_55_59_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_55-59_female_absolute.csv");
        public static readonly string POPULATION_AGE_60_64_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_60-64_female_absolute.csv");
        public static readonly string POPULATION_AGE_65_69_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_65-69_female_absolute.csv");
        public static readonly string POPULATION_AGE_70_74_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_70-74_female_absolute.csv");
        public static readonly string POPULATION_AGE_75_79_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_75-79_female_absolute.csv");
        public static readonly string POPULATION_AGE_80_ABOVE_FEMALE_ABSOLUTE_PATH = Path.Combine(DATA_PATH, "population_80-above_female_absolute.csv");
    }
}
