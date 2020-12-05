using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class LogicIDUpdateEventHandler: MonoBehaviour, IOnEventCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private LogicIDUpdateEventHandler() {
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
            if(photonEvent.Code == (byte)EventCodes.EventCode.LogicIDUpdateEvent) {
                PlayerOnPhotonInstantiate.LogicID = (int)photonEvent.CustomData;
            }
        }
    }
}