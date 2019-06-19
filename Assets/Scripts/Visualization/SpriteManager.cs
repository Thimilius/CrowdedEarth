using System.Collections.Generic;
using UnityEngine;

namespace CrowdedEarth.Visualization {
    public static class SpriteManager {
        private static Dictionary<string, Sprite> m_Sprites;

        public static void LoadSprites() {
            m_Sprites = new Dictionary<string, Sprite>();
            Sprite[] sprites = Resources.LoadAll<Sprite>("Flags/");
            foreach (Sprite sprite in sprites) {
                m_Sprites[sprite.name] = sprite;
            }
        }

        public static Sprite GetFlag(string name) {
            return m_Sprites[name];
        }
    }
}