using Photon.Pun;
using UnityEngine;
using Impasta.Game;

namespace Impasta {
    internal sealed class IncrementStartGameDoneCountRPC: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private IncrementStartGameDoneCountRPC() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        [PunRPC] public void IncrementStartGameDoneCount() {
            ++PlayerRoles.StartGameDoneCount;
        }
    }
}