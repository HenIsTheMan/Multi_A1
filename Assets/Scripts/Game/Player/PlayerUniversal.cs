using UnityEngine;

namespace Impasta.Game {
    internal static class PlayerUniversal {
        #region Fields

        private static Color[] colors;

        #endregion

        #region Properties

        public static Color[] Colors {
            get {
                return colors;
            }
            set {
                colors = value;
            }
        }

        #endregion

        #region Ctors and Dtor

        static PlayerUniversal() {
            colors = System.Array.Empty<Color>();
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public static void InitColors() {
            colors = new Color[]{
                Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true)
            };
        }

        public static Color GetPlayerColor(int index) {
            if(index >= 0 && index < colors.Length) {
                Debug.Log(colors[index]);
                return colors[index];
            } else {
                Debug.LogWarning("<color=yellow>Var 'index' is out of range!</color>");
                return Color.black;
            }
        }
    }
}