using UnityEngine;

namespace Impasta{
    internal static class PlayerColors {
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

        static PlayerColors() {
            colors = System.Array.Empty<Color>();
        }

        #endregion

        private static Color FormRandColor() {
            Color myColor = Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true);
            return myColor;
        }

        public static void InitColors() {
            colors = new Color[]{
                FormRandColor(),
                FormRandColor(),
                FormRandColor(),
                FormRandColor(),
                FormRandColor(),
                FormRandColor(),
                FormRandColor(),
                FormRandColor(),
                FormRandColor(),
                FormRandColor()
            };
        }

        public static Color GetPlayerColor(int index) {
            if(index >= 0 && index < colors.Length) {
                return colors[index];
            } else {
                Debug.LogWarning("<color=yellow>Var 'index' is out of range!</color>");
                return Color.black;
            }
        }
    }
}