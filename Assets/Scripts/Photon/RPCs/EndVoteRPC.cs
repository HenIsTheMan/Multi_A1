using Impasta.Game;
using Photon.Pun;
using UnityEngine;

namespace Impasta {
    internal sealed class EndVoteRPC: MonoBehaviour {
        #region Fields

        private GameObject reportCanvas;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private EndVoteRPC() {
            reportCanvas = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Start() {
            reportCanvas = GameObject.Find("ReportCanvasWrapper").transform.GetChild(0).gameObject;
        }

        #endregion

        [PunRPC] public void EndVote() {
            GameObject tagObj = (GameObject)PhotonNetwork.LocalPlayer.TagObject;

            tagObj.GetComponent<PlayerCharMovement>().CanMove = true;
            tagObj.GetComponent<PlayerCharReport>().VoteEnd();

            reportCanvas.SetActive(false);
        }
    }
}