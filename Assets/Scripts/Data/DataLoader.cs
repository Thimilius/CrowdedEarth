using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System.Linq;
using UnityEngine.Networking;
using Newtonsoft.Json;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.Data {
    public delegate void GetCountriesHandler(ICountry country, bool success);

    public static class DataLoader {
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
        private class CountryPopulationEntry {
            [Index(2)]  public string Country { get; set; }
            [Index(4)]  public int Population1960 { get; set; }
            [Index(5)]  public int Population1961 { get; set; }
            [Index(6)]  public int Population1962 { get; set; }
            [Index(7)]  public int Population1963 { get; set; }
            [Index(8)]  public int Population1964 { get; set; }
            [Index(9)]  public int Population1965 { get; set; }
            [Index(10)] public int Population1966 { get; set; }
            [Index(11)] public int Population1967 { get; set; }
            [Index(12)] public int Population1968 { get; set; }
            [Index(13)] public int Population1969 { get; set; }
            [Index(14)] public int Population1970 { get; set; }
            [Index(15)] public int Population1971 { get; set; }
            [Index(16)] public int Population1972 { get; set; }
            [Index(17)] public int Population1973 { get; set; }
            [Index(18)] public int Population1974 { get; set; }
            [Index(19)] public int Population1975 { get; set; }
            [Index(20)] public int Population1976 { get; set; }
            [Index(21)] public int Population1977 { get; set; }
            [Index(22)] public int Population1978 { get; set; }
            [Index(23)] public int Population1979 { get; set; }
            [Index(24)] public int Population1980 { get; set; }
            [Index(25)] public int Population1981 { get; set; }
            [Index(26)] public int Population1982 { get; set; }
            [Index(27)] public int Population1983 { get; set; }
            [Index(28)] public int Population1984 { get; set; }
            [Index(29)] public int Population1985 { get; set; }
            [Index(30)] public int Population1986 { get; set; }
            [Index(31)] public int Population1987 { get; set; }
            [Index(32)] public int Population1988 { get; set; }
            [Index(33)] public int Population1989 { get; set; }
            [Index(34)] public int Population1990 { get; set; }
            [Index(35)] public int Population1991 { get; set; }
            [Index(36)] public int Population1992 { get; set; }
            [Index(37)] public int Population1993 { get; set; }
            [Index(38)] public int Population1994 { get; set; }
            [Index(39)] public int Population1995 { get; set; }
            [Index(40)] public int Population1996 { get; set; }
            [Index(41)] public int Population1997 { get; set; }
            [Index(42)] public int Population1998 { get; set; }
            [Index(43)] public int Population1999 { get; set; }
            [Index(44)] public int Population2000 { get; set; }
            [Index(45)] public int Population2001 { get; set; }
            [Index(46)] public int Population2002 { get; set; }
            [Index(47)] public int Population2003 { get; set; }
            [Index(48)] public int Population2004 { get; set; }
            [Index(49)] public int Population2005 { get; set; }
            [Index(50)] public int Population2006 { get; set; }
            [Index(51)] public int Population2007 { get; set; }
            [Index(52)] public int Population2008 { get; set; }
            [Index(53)] public int Population2009 { get; set; }
            [Index(54)] public int Population2010 { get; set; }
            [Index(55)] public int Population2011 { get; set; }
            [Index(56)] public int Population2012 { get; set; }
            [Index(57)] public int Population2013 { get; set; }
            [Index(58)] public int Population2014 { get; set; }
            [Index(59)] public int Population2015 { get; set; }
            [Index(60)] public int Population2016 { get; set; }
            [Index(61)] public int Population2017 { get; set; }
            [Index(62)] public int Population2018 { get; set; }
            [Index(63)] public int Population2019 { get; set; }
            [Index(64)] public int Population2020 { get; set; }
            [Index(65)] public int Population2021 { get; set; }
            [Index(66)] public int Population2022 { get; set; }
            [Index(67)] public int Population2023 { get; set; }
            [Index(68)] public int Population2024 { get; set; }
            [Index(69)] public int Population2025 { get; set; }
            [Index(70)] public int Population2026 { get; set; }
            [Index(71)] public int Population2027 { get; set; }
            [Index(72)] public int Population2028 { get; set; }
            [Index(73)] public int Population2029 { get; set; }
            [Index(74)] public int Population2030 { get; set; }
            [Index(75)] public int Population2031 { get; set; }
            [Index(76)] public int Population2032 { get; set; }
            [Index(77)] public int Population2033 { get; set; }
            [Index(78)] public int Population2034 { get; set; }
            [Index(79)] public int Population2035 { get; set; }
            [Index(80)] public int Population2036 { get; set; }
            [Index(81)] public int Population2037 { get; set; }
            [Index(82)] public int Population2038 { get; set; }
            [Index(83)] public int Population2039 { get; set; }
            [Index(84)] public int Population2040 { get; set; }
            [Index(85)] public int Population2041 { get; set; }
            [Index(86)] public int Population2042 { get; set; }
            [Index(87)] public int Population2043 { get; set; }
            [Index(88)] public int Population2044 { get; set; }
            [Index(89)] public int Population2045 { get; set; }
            [Index(90)] public int Population2046 { get; set; }
            [Index(91)] public int Population2047 { get; set; }
            [Index(92)] public int Population2048 { get; set; }
            [Index(93)] public int Population2049 { get; set; }
            [Index(94)] public int Population2050 { get; set; }
        }
        
        private class Location {
            public string Name { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }

        private class Country : ICountry {
            public string Name { get; set; }
            public List<int> Population { get; set; }
            public float Latitude { get; set; }
            public float Longitude { get; set; }
        }

        private class Response {
            public string Lat { get; set; }
            public string Lon { get; set; }
        }

        private const string BASE_URL = "https://nominatim.openstreetmap.org/search/";
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
                    var entries = csv.GetRecords<CountryPopulationEntry>();

                    var properties = entries.First().GetType().GetProperties().Where(p => p.Name.StartsWith("Population"));

                    foreach (CountryPopulationEntry entry in entries) {
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
                        if (location != null) {
                            country.Latitude = location.Latitude;
                            country.Longitude = location.Longitude;
                            callback?.Invoke(country, true);
                            continue;
                        }

                        MakeRequest<List<Response>>($"{name}", (responses, success) => {
                            if (success) {
                                if (responses.Count < 1) {
                                    Debug.LogError($"Failed to get location for: {country.Name}");
                                    callback?.Invoke(null, false);
                                } else {
                                    Response response = responses[0];
                                    country.Latitude = float.Parse(response.Lat);
                                    country.Longitude = float.Parse(response.Lon);
                                    Debug.Log($"\"{country.Name}\",{country.Latitude},{country.Longitude}");
                                    callback?.Invoke(country, true);
                                }
                            } else {
                                callback?.Invoke(null, false);
                            }
                        });
                    }
                }
            }
        }

        private static void MakeRequest<T>(string query, Action<T, bool> callback) {
            UnityWebRequest request = UnityWebRequest.Get($"{BASE_URL}{query}?format=json");
            request.SendWebRequest().completed += operation => {
                // Prevent callbacks from being called after we already quit play mode
                if (!Application.isPlaying) {
                    return;
                }

                if (request.isHttpError || request.isNetworkError) {
                    Debug.LogError($"[DataLoader] - Request failed with error ({request.responseCode}): {request.error}");
                    callback?.Invoke(default, false);
                } else {
                    T response = JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                    callback?.Invoke(response, true);
                }
            };
        }
    }
}
