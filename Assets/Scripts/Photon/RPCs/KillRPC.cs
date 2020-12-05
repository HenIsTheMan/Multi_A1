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

        [PunRPC] public void Kill(string humanPlayerCharName, Vector3 humanPlayerCharPos) {
            GameObject humanPlayerChar = GameObject.Find(humanPlayerCharName);

            humanPlayerChar.GetComponent<PlayerCharKill>().KennaKilled();

            if(humanPlayerChar == (GameObject)PhotonNetwork.LocalPlayer.TagObject) {
                GameManager.SpawnDeadBody(humanPlayerCharPos);
            }
        }
    }
}