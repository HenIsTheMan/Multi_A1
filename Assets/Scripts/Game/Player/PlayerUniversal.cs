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
            ShuffleListElements.Shuffle(myRoles);

            roles = myRoles.ToArray();
        }

        public static void InitColors() {
            colors = new Color[PhotonNetwork.CurrentRoom.MaxPlayers];
            int arrLen = colors.Length;

            for(int i = 0; i < arrLen; ++i) {
                colors[i] = Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, false);
            }
        }
    }
}