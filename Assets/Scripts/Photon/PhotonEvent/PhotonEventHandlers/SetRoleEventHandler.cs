using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class SetRoleEventHandler: MonoBehaviour, IOnEventCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private SetRoleEventHandler() {
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void OnEnable() {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable() {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        #endregion

        public void OnEvent(EventData photonEvent) {
            EventCodes.EventCode eventCode = (EventCodes.EventCode)photonEvent.Code;
            switch(eventCode) {
                case EventCodes.EventCode.SetRoleEvent:
                    bool isImposter = (bool)photonEvent.CustomData;

                    gameObject.GetComponent<PlayerCharKill>().IsImposter = isImposter;

                    Text textComponent = GameObject.Find("PlayerNameText").GetComponent<Text>();
                    textComponent.text = PhotonNetwork.LocalPlayer.NickName;
                    //childTextComponent.color

                    break;
            }
        }
    }
}