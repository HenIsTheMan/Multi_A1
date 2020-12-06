using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Impasta.Game {
    internal static class PlayerUniversal {
        #region Fields

        private static bool[] roles;
        private static Color[] colors;

        #endregion

        #region Properties

        public static bool[] Roles {
            get {
                return roles;
            }
            set {
                roles = value;
            }
        }

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
            roles = System.Array.Empty<bool>();
            colors = System.Array.Empty<Color>();
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public static void GenRoles() {
            List<bool> myRoles = new List<bool>();
            int arrLen = PhotonNetwork.PlayerList.Length;

            for(int i = 0; i < arrLen; ++i) {
                myRoles.Add(arrLen > 5 ? (i < 2) : (i == 0));
            }
            ShuffleElements.Shuffle(myRoles);

            roles = myRoles.ToArray();
        }

        public static void InitColors() {
            float commonVal = Random.Range(0.0f, 1.0f);

            colors = new Color[]{
                new Color(0.0f, 0.0f, 0.0f, 1.0f),
                new Color(1.0f, 1.0f, 1.0f, 1.0f),
                new Color(commonVal, commonVal, commonVal, 1.0f),
                new Color(Random.Range(0.2f, 0.8f), 0.0f, 0.0f, 1.0f),
                new Color(0.0f, Random.Range(0.2f, 0.8f), 0.0f, 1.0f),
                new Color(0.0f, 0.0f, Random.Range(0.2f, 0.8f), 1.0f),
                new Color(Random.Range(0.2f, 0.8f), 0.0f, Random.Range(0.2f, 0.8f), 1.0f),
                new Color(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), 0.0f, 1.0f),
                new Color(0.0f, Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), 1.0f),
                new Color(0.82f, 0.41f, 0.11f),
            };
            ShuffleElements.Shuffle(colors);
        }
    }
}