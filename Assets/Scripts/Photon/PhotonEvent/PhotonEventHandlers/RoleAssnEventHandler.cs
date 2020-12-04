using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class RoleAssnEventHandler: MonoBehaviour, IOnEventCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private RoleAssnEventHandler() {
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
                case EventCodes.EventCode.RoleAssnEvent:
                    Debug.Log("Here!");

                    bool[] data = (bool[])photonEvent.CustomData;
                    int dataArrLen = data.Length;

                    bool isLocalClientImposter = false;
                    for(int i = 0; i < dataArrLen; ++i) {
                        if(PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[i]) {
                            isLocalClientImposter = data[i];
                            break;
                        }
                        UnityEngine.Assertions.Assert.IsTrue(false);
                    }

                    for(int i = 0; i < dataArrLen; ++i) {
                        bool isImposter = data[i];
                        GameObject playerCharGO = GameObject.Find("PlayerChar" + PhotonNetwork.PlayerList[i].ActorNumber);

                        playerCharGO.GetComponent<PlayerCharKill>().IsImposter = isImposter;

                        Transform childTransform = playerCharGO.transform.Find("PlayerNameCanvas");
                        Transform grandchildTransform = childTransform.Find("PlayerNameText");
                        Text textComponent = grandchildTransform.GetComponent<Text>();

                        textComponent.text = PhotonNetwork.PlayerList[i].NickName + "   " + isImposter;

                        if(isLocalClientImposter && isImposter) {
                            textComponent.color = Color.red;
                        }
                    }
                    break;
            }
        }
    }
}