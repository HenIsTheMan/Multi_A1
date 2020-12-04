using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Impasta.Game {
    internal static class PlayerUniversal {
        #region Fields

        private static bool[] roles;

        private static Color[] colors;

        private static string[] names;

        private static Vector3[] spawnPos;

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

        public static string[] Names {
            get {
                return names;
            }
            set {
                names = value;
            }
        }

        public static Vector3[] SpawnPos {
            get {
                return spawnPos;
            }
            set {
                spawnPos = value;
            }
        }

        #endregion

        #region Ctors and Dtor

        static PlayerUniversal() {
            roles = System.Array.Empty<bool>();
            colors = System.Array.Empty<Color>();
            names = System.Array.Empty<string>();
            spawnPos = System.Array.Empty<Vector3>();
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public static void GenRoles() {
            List<bool> myRoles = new List<bool>();
            int maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;

            for(int i = 0; i < maxPlayers; ++i) {
                myRoles[i] = maxPlayers > 5 ? (i < 2) : (i == 0);
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

        public static void GenNames() {
            names = new string[PhotonNetwork.CurrentRoom.MaxPlayers];
            int arrLen = names.Length;

            for(int i = 0; i < arrLen; ++i) {
                names[i] = "PlayerChar" + i;
            }
        }

        public static void CreateSpawnPos() {
            spawnPos = new Vector3[PhotonNetwork.CurrentRoom.MaxPlayers];
            int arrLen = spawnPos.Length;

            for(int i = 0; i < arrLen; ++i) {
                float angle = (360.0f / (float)arrLen) * Mathf.Deg2Rad;
                float radius = 3.0f;
                spawnPos[i] = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0.0f) * radius;
            }
        }
    }
}