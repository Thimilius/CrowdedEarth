using UnityEngine;
using CrowdedEarth.Data.Model;

namespace CrowdedEarth.Visualization {
    public class VisualObject : MonoBehaviour {
        public VisualObjectType Type { get; set; }

        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public ICountry Country { get; set; }
    }
}
