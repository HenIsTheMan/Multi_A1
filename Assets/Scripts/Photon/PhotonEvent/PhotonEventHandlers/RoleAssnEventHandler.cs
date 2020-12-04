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


                    bool isImposter = (bool)photonEvent.CustomData;

                    gameObject.GetComponent<PlayerCharKill>().IsImposter = isImposter;

                    Transform childTransform = gameObject.transform.Find("PlayerNameCanvas");
                    Transform grandchildTransform = childTransform.Find("PlayerNameText");
                    Text textComponent = grandchildTransform.GetComponent<Text>();

                    textComponent.text = PhotonNetwork.LocalPlayer.NickName;

                    GameObject[] playerCharGOs = GameObject.FindGameObjectsWithTag("Player");
                    int arrLen = playerCharGOs.Length;
                    for(int i = 0; i < arrLen; ++i) {
                        if(isImposter){
                            textComponent.color = Color.red;

                            GameObject playerCharGO = playerCharGOs[i];
                            if(playerCharGO != gameObject && playerCharGO.GetComponent<PlayerCharKill>().IsImposter) {
                                Transform playerCharGOChildTransform = gameObject.transform.Find("PlayerNameCanvas");
                                Transform playerCharGOGrandchildTransform = childTransform.Find("PlayerNameText");
                                playerCharGOGrandchildTransform.GetComponent<Text>().color = Color.red;
                            }
                        }
                    }

                    break;
            }
        }
    }
}