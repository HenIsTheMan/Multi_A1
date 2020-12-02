using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Impasta.Game{
    internal sealed class PlayerCharCam: MonoBehaviour, IOnEventCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public PlayerCharCam() {
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
            GameManager.EventCode eventCode = (GameManager.EventCode)photonEvent.Code;
            switch(eventCode) {
                case GameManager.EventCode.PlayerCharsCreatedEvent:
                    Dictionary<int, string> data = (Dictionary<int, string>)photonEvent.CustomData;
                    string playerCharName = data[PhotonNetwork.LocalPlayer.ActorNumber];
                    GameObject playerChar = GameObject.Find(playerCharName).gameObject;

                    gameObject.transform.position = new Vector3(playerChar.transform.position.x, playerChar.transform.position.y, gameObject.transform.position.z);
                    gameObject.GetComponent<CamFollow>().TargetTransform = playerChar.transform;

                    PlayerCharKill playerCharKill = playerChar.GetComponent<PlayerCharKill>();
                    playerCharKill.IsImposter = true;

					PlayerCharMovement playerCharMovement = playerChar.GetComponent<PlayerCharMovement>();
					playerCharMovement.CanMove = true;
					break;
            }
        }
    }
}