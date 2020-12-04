using Photon.Pun;
using UnityEngine;
using Impasta.Game;

namespace Impasta {
    internal sealed class RetrievePlayerRolesRPC: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private RetrievePlayerRolesRPC() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        [PunRPC] public void RetrievePlayerRoles(){
            PhotonView.Get(this).RPC("SetPlayerRoles", RpcTarget.Others, PlayerRoles.Roles);
        }
    }
}