using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class SendMsg: MonoBehaviour {
        #region Fields

        [SerializeField] private InputField msgBox;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public SendMsg() {
            msgBox = null;
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public void OnSendButtonPressed() {
            ((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PhotonView>().RPC("SetMsg", RpcTarget.All, msgBox.text);

            _ = StartCoroutine(nameof(MsgListItemCreate));

            msgBox.text = string.Empty;
        }

        private System.Collections.IEnumerator MsgListItemCreate() {
            while(MsgListItemOnPhotonInstantiate.Msg == string.Empty) {
                yield return null;
            }

            GameManager.CreateMsgListItem();

            yield return null;
        }
    }
}