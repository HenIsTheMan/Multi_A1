using Photon.Pun;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class PlayerDetectionSphere: MonoBehaviour {
        #region Fields

        PlayerCharKill playerCharKill;
        PlayerCharReport playerCharReport;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private PlayerDetectionSphere() {
            playerCharKill = null;
            playerCharReport = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            playerCharKill = transform.parent.GetComponent<PlayerCharKill>();
            playerCharReport = transform.parent.GetComponent<PlayerCharReport>();
        }

        private void OnTriggerEnter(Collider otherCollider) {
			System.Type colliderType = otherCollider.GetType();

            if(otherCollider.gameObject.name == name) { //Detected another alive player entering
                playerCharKill.Triggering(otherCollider);
            }
            if(otherCollider.gameObject.name == "PlayerDetectionBox") { //Detected a dead body entering
                playerCharReport.AddPlayerCharBodyNearby(otherCollider.transform.parent.gameObject);
            }
            if(otherCollider.gameObject.name == "TaskDetectionBox" && transform.parent.gameObject == (GameObject)PhotonNetwork.LocalPlayer.TagObject) {
                otherCollider.transform.parent.GetComponent<TaskBlock>().PlayerCharTagObjNearby = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
            }
        }

        private void OnTriggerExit(Collider otherCollider) {
            System.Type colliderType = otherCollider.GetType();

            if(otherCollider.gameObject.name == name) { //Detected another alive player leaving
                playerCharKill.NotTriggering(otherCollider);
            }
            if(otherCollider.gameObject.name == "PlayerDetectionBox") { //Detected a dead body leaving
                playerCharReport.RemovePlayerCharBodyNearby(otherCollider.transform.parent.gameObject);
            }
            if(otherCollider.gameObject.name == "TaskDetectionBox" && transform.parent.gameObject == (GameObject)PhotonNetwork.LocalPlayer.TagObject) {
                otherCollider.transform.parent.GetComponent<TaskBlock>().PlayerCharTagObjNearby = null;
            }
        }

        #endregion
    }
}