using Impasta.Game;
using Photon.Pun;
using UnityEngine;

namespace Impasta {
    internal sealed class SetMsgRPC: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private SetMsgRPC() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        [PunRPC] public void SetMsg(string msg) {
            MsgListItemOnPhotonInstantiate.Msg = msg;
        }
    }
}