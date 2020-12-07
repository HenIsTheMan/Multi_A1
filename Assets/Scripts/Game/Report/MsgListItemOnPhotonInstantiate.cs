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

            Text textComponent = myTransform.Find("Text").GetComponent<Text>();
            GameObject playerChar = (GameObject)info.Sender.TagObject;
            textComponent.text = info.Sender.NickName + ": " + msg;
            textComponent.color = playerChar.transform.Find("PlayerCharOutfitSprite").GetComponent<SpriteRenderer>().color;

            msg = string.Empty; //So next msg list item will only be instantiated when msg is set again
        }
    }
}