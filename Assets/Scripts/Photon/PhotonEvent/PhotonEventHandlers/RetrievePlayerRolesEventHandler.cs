using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class RetrievePlayerRolesEventHandler: MonoBehaviour, IOnEventCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private RetrievePlayerRolesEventHandler() {
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
            if(photonEvent.Code == (byte)EventCodes.EventCode.RetrievePlayerRolesEvent) {
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode.SetPlayerRolesEvent,
                    PlayerUniversal.Roles, raiseEventOptions, SendOptions.SendReliable);
            }
        }
    }
}