using Photon.Pun;
using UnityEngine;
using Impasta.Game;

namespace Impasta {
    internal sealed class KillRPC: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private KillRPC() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        [PunRPC] public void Kill(GameObject humanPlayerChar) {
            //humanPlayerChar.GetComponent<PlayerCharKill>().KennaKilled();

            //GameObject humanBody = Instantiate(humanPlayerChar, humanPlayerChar.transform.position, humanPlayerChar.transform.rotation);
            //humanBody.name
        }
    }
}