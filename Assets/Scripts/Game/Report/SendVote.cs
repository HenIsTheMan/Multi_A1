using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Impasta.Game {
    internal sealed class SendVote: MonoBehaviour {
        #region Fields

        private static int prevIndex;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private SendVote() {
            prevIndex = -1;
        }

        #endregion

        #region Unity User Callback Event Funcs

        public void OnVoteButtonPressed() {
            string buttonName = EventSystem.current.currentSelectedGameObject.name;
            int index = Convert.ToInt32(buttonName.Last()) - 48;

            object[] data = new object[]{
                index,
                prevIndex,
                1
            };
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                Receivers = ReceiverGroup.All
            };
			PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode.UpdateVoteCountEvent,
				data, raiseEventOptions, ExitGames.Client.Photon.SendOptions.SendReliable);

            prevIndex = index;
        }

        #endregion
    }
}