using Photon.Pun;
using UnityEngine;
using Impasta.Game;

namespace Impasta {
    internal sealed class VotedOffRPC: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private VotedOffRPC() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        [PunRPC] public void VotedOff(string playerCharName) {
            GameObject.Find(playerCharName).GetComponent<PlayerCharKill>().KennaKilled();
        }
    }
}