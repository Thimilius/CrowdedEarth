using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrowdedEarth.Visualization {
    public static class SpriteManager {
        public static Sprite GetFlag(string name) {
            return Resources.Load<Sprite>($"Flags/{name}");
        }
    }
}