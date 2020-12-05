using Photon.Pun;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class PlayerCharDeadBodyOnPhotonInstantiate: MonoBehaviour, IPunInstantiateMagicCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor
        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            GameObject playerCharGhost = (GameObject)info.Sender.TagObject;
            name = playerCharGhost.name;

            Transform spriteChildTransform = gameObject.transform.GetChild(0);
            spriteChildTransform.GetComponent<SpriteRenderer>().color = playerCharGhost.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        }
    }
}