using System;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class PlayerCharKill: MonoBehaviour {
        #region Fields

        private bool isDead;
        private bool isImposter;
        private bool isKillButtonPressed;

        private PlayerCharKill targetPlayerCharKill;
        private Collider myCollider;

        #endregion

        #region Properties

        public bool IsImposter {
            get {
                return isImposter;
            }
            set {
                isImposter = value;
            }
        }

        #endregion

        #region Ctors and Dtor

        public PlayerCharKill() {
            isDead = false;
            isImposter = false;
            isKillButtonPressed = false;

            targetPlayerCharKill = null;
            myCollider = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            myCollider = gameObject.GetComponent<CapsuleCollider>();
        }

        private void Update() {
            if(Input.GetButtonDown("Kill")) {
                isKillButtonPressed = true;
            }
        }

        private void FixedUpdate(){
            if(isKillButtonPressed) {
                if(targetPlayerCharKill != null) {
					transform.position = targetPlayerCharKill.transform.position;
                    targetPlayerCharKill.KennaKilled();
                    targetPlayerCharKill = null;
                }

                isKillButtonPressed = false;
            }
        }

        private void OnTriggerEnter(Collider otherCollider) {
            PlayerCharKill otherPlayerCharKill = otherCollider.gameObject.GetComponent<PlayerCharKill>();
            UnityEngine.Assertions.Assert.IsNotNull(otherPlayerCharKill);

            if(isImposter && !isDead && otherCollider.tag == "Player" && !otherPlayerCharKill.isImposter && !otherPlayerCharKill.isDead) {
                targetPlayerCharKill = otherPlayerCharKill;
            }
        }

        #endregion

        private void KennaKilled() {
            isDead = true;
            transform.position -= new Vector3(0.0f, 0.0f, -0.1f); //Ensure killer renders over killed player

            ///Make player look like a ghost
            try {
                Transform childTransform = gameObject.transform.Find("PlayerCharOutfitSprite");
                SpriteRenderer childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
                childSpriteRenderer.color -= new Color(0.0f, 0.0f, 0.0f, 0.5f);

                Transform grandchildTransform = childTransform.Find("PlayerCharSprite");
                SpriteRenderer grandchildSpriteRenderer = grandchildTransform.GetComponent<SpriteRenderer>();
                grandchildSpriteRenderer.color = new Color(0.5f, 0.5f, 1.0f, 0.5f);
            } catch(NullReferenceException) {
                UnityEngine.Assertions.Assert.IsTrue(false);
            }

            myCollider.enabled = false;
        }
    }
}