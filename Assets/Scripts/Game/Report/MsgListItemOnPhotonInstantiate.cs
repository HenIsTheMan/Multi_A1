using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class MsgListItemOnPhotonInstantiate: MonoBehaviour, IPunInstantiateMagicCallback {
        #region Fields

        private static string msg;

        #endregion

        #region Properties

        public static string Msg {
            get {
                return msg;
            }
            set {
                msg = value;
            }
        }

        #endregion

        #region Ctors and Dtor

        static MsgListItemOnPhotonInstantiate() {
            msg = string.Empty;
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            Transform myTransform = gameObject.transform;

            myTransform.SetParent(GameObject.Find("Content").transform, false);

            myTransform.Find("Text").GetComponent<Text>().text = msg;
            msg = string.Empty; //So next msg list item will only be instantiated when msg is set again
        }
    }
}