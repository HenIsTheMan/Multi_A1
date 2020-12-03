﻿using System.Collections.Generic;
using UnityEngine;

namespace Impasta{
    internal static class PlayerColors {
        #region Fields
        
        private static readonly List<Color> colors;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        static PlayerColors() {
            colors = new List<Color> {
                Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
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

        #endregion

        public static Color GetPlayerColor(int index) {
            return (index >= 0 && index < colors.Count) ? colors[index] : Color.black;
        }
    }
}