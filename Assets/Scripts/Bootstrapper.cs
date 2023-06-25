using UnityEngine;
using InhabitedEarth.Data;
using InhabitedEarth.Visualization;

namespace InhabitedEarth {
    public static class Bootstrapper {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap() {
            DataLoader.LoadCountries();
            SpriteManager.LoadSprites();
        }
    }
}
