using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Impasta.Game {
    internal static class PlayerRoles {
        #region Fields

        private static bool[] roles;

        public static int StartGameDoneCount;

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

        #endregion

        #region Ctors and Dtor

        static PlayerRoles() {
            roles = System.Array.Empty<bool>();
            StartGameDoneCount = 0;
        }

        #endregion

        public static void GenRoles() {
            int arrLen = PhotonNetwork.PlayerList.Length;
            List<bool> flags = new List<bool>();

            for(int i = 0; i < arrLen; ++i) {
                flags.Add(arrLen > 5 ? (i < 2) : (i == 0));
            }

            ShuffleListElements.Shuffle(flags);
            roles = flags.ToArray();
        }
    }
}