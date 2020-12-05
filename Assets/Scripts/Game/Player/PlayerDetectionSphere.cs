using UnityEngine;

namespace Impasta.Game {
    internal sealed class PlayerDetectionSphere: MonoBehaviour {
        #region Fields

        PlayerCharKill playerCharKill;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private PlayerDetectionSphere() {
            playerCharKill = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            playerCharKill = transform.parent.GetComponent<PlayerCharKill>();
        }

        private void OnTriggerEnter(Collider otherCollider) {
            if(otherCollider.name == name) {
                playerCharKill.Colliding(otherCollider);
            }
        }

        private void OnTriggerExit(Collider otherCollider) {
            if(otherCollider.name == name) {
                playerCharKill.NotColliding(otherCollider);
            }
        }

        #endregion
    }
}