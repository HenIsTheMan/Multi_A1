using Impasta.Game;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta {
    internal sealed class SetPlayerRolesRPC: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private SetPlayerRolesRPC() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        [PunRPC] public void SetPlayerRoles(bool[] playerRoles){
            PlayerRoles.Roles = playerRoles;
        }
    }
}