using UnityEngine;
using CrowdedEarth.Data;
using CrowdedEarth.Visualization;

namespace CrowdedEarth {
    public static class Bootstrapper {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap() {
            DataLoader.LoadCountries();
            SpriteManager.LoadSprites();
        }
    }
}
