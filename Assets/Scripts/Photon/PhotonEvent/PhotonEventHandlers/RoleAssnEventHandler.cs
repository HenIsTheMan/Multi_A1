using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
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
                    bool[] data = (bool[])photonEvent.CustomData;
                    GameObject[] playerCharGOs = GameObject.FindGameObjectsWithTag("Player");

                    bool localClientIsImposter = false;
                    int arrLen = playerCharGOs.Length;
                    for(int i = 0; i < arrLen; ++i) {
                        if(PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer) {
                            localClientIsImposter = data[i];
                            break;
                        }
                        if(i == arrLen - 1) {
                            UnityEngine.Assertions.Assert.IsTrue(false);
                        }
                    }

                    GameObject playerCharGO;
                    int index = 0;
                    bool isImposter;

                    for(int i = 0; i < arrLen; ++i) {
                        playerCharGO = playerCharGOs[i];

                        ///Calc index
                        for(int j = 0; j < arrLen; ++j) {
                            if(PhotonNetwork.PlayerList[j].NickName == playerCharGO.name) {
                                index = j;
                                break;
                            }
                            if(j == arrLen - 1) {
                                UnityEngine.Assertions.Assert.IsTrue(false);
                            }
                        }

                        isImposter = data[index];

                        playerCharGO.GetComponent<PlayerCharKill>().IsImposter = isImposter;

                        Transform childTransform = playerCharGO.transform.Find("PlayerNameCanvas");
                        Transform grandchildTransform = childTransform.Find("PlayerNameText");

                        Text textComponent = grandchildTransform.GetComponent<Text>();
                        textComponent.text = playerCharGO.name;

                        if(localClientIsImposter && isImposter) {
                            textComponent.color = Color.red;
                        }
                    }
                    break;
            }
        }
    }
}