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

        [PunRPC] public void Kill(GameObject human) {
            GameObject humanPlayerChar = GameObject.Find("humanName");

            humanPlayerChar.GetComponent<PlayerCharKill>().KennaKilled();

            GameObject humanBody = Instantiate(human, human.transform.position, human.transform.rotation);
            //humanBody.name
        }
    }
}