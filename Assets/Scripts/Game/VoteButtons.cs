using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class VoteButtons: MonoBehaviour {
        #region Fields

        private int arrLen;
        [SerializeField] private GameObject[] voteButtons;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private VoteButtons() {
            arrLen = 0;
            voteButtons = Array.Empty<GameObject>();
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Start(){
            arrLen = voteButtons.Length;
        }

        private void Update() {
            for(int i = 0; i < arrLen; ++i) {
                GameObject voteButton = voteButtons[i];
                if(PlayerUniversal.Votes.Length > i && !Mathf.Approximately(voteButton.GetComponent<RawImage>().color.a, 0.0f)) {
                    if(((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerCharKill>().IsDead) {
                        voteButton.transform.Find("Text").GetComponent<Text>().text = "---";
                    } else {
                        voteButton.transform.Find("Text").GetComponent<Text>().text
                            = ((GameObject)PhotonNetwork.CurrentRoom.GetPlayer(i + 1).TagObject).GetComponent<PlayerCharKill>().IsDead
                            ? "XX"
                            : PlayerUniversal.Votes[i].ToString();
                    }
                }
            }
        }

        #endregion
    }
}