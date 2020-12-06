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
            if(otherCollider.name == name) { //Detected another alive player entering
                playerCharKill.Triggering(otherCollider);
            } else { //Detected a dead body entering
                playerCharReport.AddPlayerCharBodyNearby(otherCollider.transform.parent.gameObject);
            }
        }

        private void OnTriggerExit(Collider otherCollider) {
            if(otherCollider.name == name) { //Detected another alive player leaving
                playerCharKill.NotTriggering(otherCollider);
            } else { //Detected a dead body leaving
                playerCharReport.RemovePlayerCharBodyNearby(otherCollider.transform.parent.gameObject);
            }
        }

        #endregion
    }
}