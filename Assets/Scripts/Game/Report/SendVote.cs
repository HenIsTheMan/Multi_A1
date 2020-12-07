using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Impasta.Game {
    internal sealed class SendVote: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private SendVote() {
        }

        #endregion

        #region Unity User Callback Event Funcs

        public void OnVoteButtonPressed() {
            string buttonName = EventSystem.current.currentSelectedGameObject.name;
            int index = Convert.ToInt32(buttonName.Last());

            object[] data = new object[]{
                index,
                1
            };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                Receivers = ReceiverGroup.All
            };
            PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode.UpdateVoteCountEvent,
                data, raiseEventOptions, ExitGames.Client.Photon.SendOptions.SendReliable);
        }

        #endregion
    }
}