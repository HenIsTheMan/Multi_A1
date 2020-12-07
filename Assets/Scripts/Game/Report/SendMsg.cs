using Photon.Pun;
using Photon.Realtime;
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

            if(((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerCharKill>().IsDead) {
                Color myTagObjColor = ((GameObject)PhotonNetwork.LocalPlayer.TagObject).transform.Find("PlayerCharOutfitSprite").GetComponent<SpriteRenderer>().color;

                object[] customData = new object[]{
                    PhotonNetwork.LocalPlayer.NickName,
                    new Vector3(myTagObjColor.r, myTagObjColor.g, myTagObjColor.b)
                };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                    Receivers = ReceiverGroup.All
                };
                PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode.MsgSentByGhostEvent,
                    customData, raiseEventOptions, ExitGames.Client.Photon.SendOptions.SendReliable);
            } else {
                GameManager.CreateMsgListItem();
            }

            yield return null;
        }
    }
}