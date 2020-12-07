using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class SetPlayerTasksEventHandler: MonoBehaviour, IOnEventCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private SetPlayerTasksEventHandler() {
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
            if(photonEvent.Code == (byte)EventCodes.EventCode.SetPlayerTasksEvent) {
                object[] data = (object[])photonEvent.CustomData;

                PlayerCharTasks playerCharTasks = GameObject.Find((string)data[0]).GetComponent<PlayerCharTasks>();

                playerCharTasks.TotalAmtOfTasks = (int)data[1];

                Transform canvasChildTransform = GameObject.Find((string)data[0]).transform.Find("PlayerNameCanvas");
                Transform grandchildTransform = canvasChildTransform.Find("PlayerNameText");

                UnityEngine.UI.Text textComponent = grandchildTransform.GetComponent<UnityEngine.UI.Text>();

                textComponent.text = playerCharTasks.AmtOfCompleteTasks.ToString() + '/' + playerCharTasks.TotalAmtOfTasks;
            }
        }
    }
}