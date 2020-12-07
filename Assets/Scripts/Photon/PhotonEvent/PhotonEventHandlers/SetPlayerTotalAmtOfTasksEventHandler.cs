using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class SetPlayerTotalAmtOfTasksEventHandler: MonoBehaviour, IOnEventCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private SetPlayerTotalAmtOfTasksEventHandler() {
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
            if(photonEvent.Code == (byte)EventCodes.EventCode.SetPlayerTotalAmtOfTasksEvent) {
                object[] data = (object[])photonEvent.CustomData;
                _ = StartCoroutine(SetPlayerTask(data));
            }
        }

        private System.Collections.IEnumerator SetPlayerTask(object[] data) {
            GameObject playerChar = GameObject.Find((string)data[0]);

            while(playerChar == null) {
                yield return null;
            }

            playerChar.GetComponent<PlayerCharTasks>().TotalAmtOfTasks = (int)data[1];

            yield return null;
        }
    }
}