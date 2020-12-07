using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class PlayerCharReport: MonoBehaviour {
        #region Fields

        private bool isDead;
        private bool isReportButtonPressed;

        private List<GameObject> playerCharBodies;

        private float voteTime;
        private Text voteTimeTextComponent;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private PlayerCharReport() {
            isDead = false;
            isReportButtonPressed = false;

            playerCharBodies = null;

            voteTime = 0.0f;
            voteTimeTextComponent = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Start() {
            playerCharBodies = new List<GameObject>();

            voteTimeTextComponent = GameObject.Find("VoteTimeText").GetComponent<Text>();
        }

        private void Update() {
            if(Input.GetButtonDown("Report")) {
                isReportButtonPressed = true;
            }

            if(isReportButtonPressed && !isDead) {
                int listCount = playerCharBodies.Count;

                if(listCount > 0) { //Find nearest player char body to report
                    float currShortestDist = float.MaxValue;
                    GameObject currClosestPlayerCharBody = null;

                    for(int i = 0; i < listCount; ++i) {
                        GameObject playerCharBody = playerCharBodies[i];
                        if(!playerCharBody.activeSelf) {
                            continue;
                        }

                        if(currClosestPlayerCharBody == null) {
                            currClosestPlayerCharBody = playerCharBody;
                        } else {
                            Vector3 GOPos = transform.position;
                            Vector3 bodyPos = playerCharBody.transform.position;
                            Vector3 GOPosXY = new Vector3(GOPos.x, GOPos.y, 0.0f);
                            Vector3 bodyPosXY = new Vector3(bodyPos.x, bodyPos.y, 0.0f);

                            float dist = (GOPosXY - bodyPosXY).magnitude;
                            if(dist < currShortestDist) {
                                currShortestDist = dist;
                                currClosestPlayerCharBody = playerCharBody;
                            }
                        }
                    }

                    if(currClosestPlayerCharBody != null) {
                        PhotonView.Get(this).RPC("Report", RpcTarget.All);
                    }
                }

                isReportButtonPressed = false;
            }

            if(voteTimeTextComponent.enabled && voteTime > 0.0f) {
                UpdateVoting();
            }
        }

        #endregion

        public void VoteStart() {
            voteTime = 10.0f;
            voteTimeTextComponent.enabled = true;
            System.Array.Clear(PlayerUniversal.Votes, 0, PlayerUniversal.Votes.Length); //So votes casted during non-voting times will be cleared
            SendVote.PrevIndex = -1;
        }

        public void VoteEnd() {
            voteTimeTextComponent.enabled = false;

            if(PhotonNetwork.IsMasterClient) {
                int indexWithLargestVal = -1;
                int arrLen = PlayerUniversal.Votes.Length;
                for(int i = 0; i < arrLen; ++i) {
                    if(indexWithLargestVal < 0 || PlayerUniversal.Votes[i] > PlayerUniversal.Votes[indexWithLargestVal]) {
                        indexWithLargestVal = i;
                    }
                }

                PhotonView.Get(this).RPC("VotedOff", RpcTarget.All, "PlayerChar" + indexWithLargestVal);
            }

            System.Array.Clear(PlayerUniversal.Votes, 0, PlayerUniversal.Votes.Length);
            SendVote.PrevIndex = -1;
        }

        private void UpdateVoting() {
            voteTime -= Time.deltaTime;
            voteTimeTextComponent.text = (Mathf.Max(0, Mathf.Ceil(voteTime))).ToString();

            if(voteTime <= 0.0f && PhotonNetwork.IsMasterClient) {
                PhotonView.Get(this).RPC("EndVote", RpcTarget.All);
            }
        }

        public void UpdateIsDead(bool isDead) {
            this.isDead = isDead;
        }

        public void AddPlayerCharBodyNearby(in GameObject playerCharBody) {
            if(!playerCharBodies.Contains(playerCharBody)) {
                playerCharBodies.Add(playerCharBody);
            }
        }

        public void RemovePlayerCharBodyNearby(in GameObject playerCharBody) {
            if(playerCharBodies.Contains(playerCharBody)) {
                playerCharBodies.Remove(playerCharBody);
            }
        }
    }
}