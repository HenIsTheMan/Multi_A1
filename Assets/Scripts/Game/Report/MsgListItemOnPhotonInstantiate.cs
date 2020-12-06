using Photon.Pun;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class MsgListItemOnPhotonInstantiate: MonoBehaviour, IPunInstantiateMagicCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private MsgListItemOnPhotonInstantiate() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            gameObject.transform.parent = GameObject.Find("Content").transform;
        }
    }
}