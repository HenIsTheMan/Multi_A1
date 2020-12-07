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

        public static int PrevIndex {
            get {
                return prevIndex;
            }
            set {
                prevIndex = value;
            }
        }

        #endregion

        #region Ctors and Dtor

        private SendVote() {
            prevIndex = -1;
        }

        #endregion

        #region Unity User Callback Event Funcs

        public void OnVoteButtonPressed() {
            if(((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerCharKill>().IsDead) { //So ghosts cannot vote
                return;
            }

            string buttonName = EventSystem.current.currentSelectedGameObject.name;
            int index = Convert.ToInt32(buttonName.Last()) - 48;
            if(((GameObject)PhotonNetwork.CurrentRoom.GetPlayer(index + 1).TagObject).GetComponent<PlayerCharKill>().IsDead) { //So cannot vote for ghosts
                return;
            }

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