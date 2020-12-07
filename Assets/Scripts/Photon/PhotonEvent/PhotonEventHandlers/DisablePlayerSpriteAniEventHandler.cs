using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class DisablePlayerSpriteAniEventHandler: MonoBehaviour, IOnEventCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private DisablePlayerSpriteAniEventHandler() {
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void OnEnable() {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        private void OnDisable() {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }

        #endregion

        public void OnEvent(EventData photonEvent) {
            if(photonEvent.Code == (byte)EventCodes.EventCode.DisablePlayerSpriteAniEvent) {
                GameObject playerChar = GameObject.Find((string)photonEvent.CustomData);

                UnityEngine.Assertions.Assert.IsNotNull(playerChar);
                playerChar.GetComponent<PlayerCharMovement>().DisableSpriteAni();
            }
        }
    }
}