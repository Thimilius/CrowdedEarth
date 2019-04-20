using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.Data {
    public delegate void GetCitiesHandler(IEnumerable<ICity> cities, bool success);
    public delegate void GetCityDetailsHandler(ICityDetails city, bool success);

    public interface ICitiesAPI {
        void GetCities(int maxPopulation, GetCitiesHandler callback);
        void GetCityDetails(int cityID, GetCityDetailsHandler callback);
    }
}
