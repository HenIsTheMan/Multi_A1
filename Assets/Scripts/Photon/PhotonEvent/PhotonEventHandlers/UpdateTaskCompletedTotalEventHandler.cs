using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class UpdateTaskCompletedTotalEventHandler: MonoBehaviour, IOnEventCallback {
        #region Fields

        [SerializeField] private Text tasksTextComponent;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private UpdateTaskCompletedTotalEventHandler() {
            tasksTextComponent = null;
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
            if(photonEvent.Code == (byte)EventCodes.EventCode.UpdateTaskCompletedTotalEvent) {
			}
        }
    }
}