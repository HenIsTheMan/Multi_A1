using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class PlayerCharReport: MonoBehaviour {
        #region Fields

        private bool isDead;
        private bool isReportButtonPressed;

        private List<GameObject> playerCharBodies;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private PlayerCharReport() {
            isDead = false;
            isReportButtonPressed = false;

            playerCharBodies = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Start() {
            playerCharBodies = new List<GameObject>();
        }

        private void Update() {
            if(Input.GetButtonDown("Report")) {
                isReportButtonPressed = true;
            }
        }

        private void FixedUpdate() {
            if(isReportButtonPressed && !isDead) {
                int listCount = playerCharBodies.Count;

                if(listCount > 0) { //Find nearest player char body to report
                    float currShortestDist = float.MaxValue;
                    GameObject currClosestPlayerCharBody = null;

                    for(int i = 0; i < listCount; ++i) {
                        GameObject playerCharBody = playerCharBodies[i];
                        if(!playerCharBody.activeSelf){
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
        }

        #endregion

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