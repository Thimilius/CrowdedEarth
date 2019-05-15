using UnityEngine;

namespace CrowdedEarth {
    public abstract class VisualObject : MonoBehaviour {
        public abstract VisualObjectType Type { get; }

        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}
