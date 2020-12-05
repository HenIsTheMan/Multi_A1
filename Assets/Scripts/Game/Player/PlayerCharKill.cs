using System;
using System.Collections.Generic;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class PlayerCharKill: MonoBehaviour {
        #region Fields

        private bool isDead;
        private bool isImposter;
        private bool isKillButtonPressed;

        private List<PlayerCharKill> playerCharKillTargets;
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

            playerCharKillTargets = null;
            myCollider = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            myCollider = gameObject.GetComponent<CapsuleCollider>();
        }

        private void Start() {
            playerCharKillTargets = new List<PlayerCharKill>();
        }

        private void Update() {
            if(Input.GetButtonDown("Kill")) {
                isKillButtonPressed = true;
            }
        }

        private void FixedUpdate(){
            if(isKillButtonPressed) {
                int listCount = playerCharKillTargets.Count;
                if(listCount > 0) {
                    ///Find nearest non-imposter to kill
                    float currShortestDist = float.MaxValue;
                    PlayerCharKill currClosestTargetPlayerCharKill = null;

                    for(int i = 0; i < listCount; ++i) {
                        if(currClosestTargetPlayerCharKill == null) {
                            currClosestTargetPlayerCharKill = playerCharKillTargets[i];
                        } else {
                            PlayerCharKill targetPlayerCharKill = playerCharKillTargets[i];
                            float dist = (gameObject.transform.position - targetPlayerCharKill.gameObject.transform.position).magnitude;
                            if(dist < currShortestDist) {
                                currShortestDist = dist;
                                currClosestTargetPlayerCharKill = targetPlayerCharKill;
                            }
                        }
                    }

                    transform.position = currClosestTargetPlayerCharKill.transform.position;
                    currClosestTargetPlayerCharKill.KennaKilled();
                    playerCharKillTargets.Remove(currClosestTargetPlayerCharKill);

                    GameManager.SpawnDeadBody(transform.position);
                }

                isKillButtonPressed = false;
            }
        }

        private void OnTriggerEnter(Collider otherCollider) {
            if(otherCollider.CompareTag("Player")) {
                PlayerCharKill otherPlayerCharKill = otherCollider.gameObject.GetComponent<PlayerCharKill>();
                UnityEngine.Assertions.Assert.IsNotNull(otherPlayerCharKill);

                if(isImposter && !isDead && otherCollider.tag == "Player" && !otherPlayerCharKill.isImposter && !otherPlayerCharKill.isDead) {
                    playerCharKillTargets.Add(otherPlayerCharKill);
                }
            }
        }

        private void OnTriggerExit(Collider otherCollider) {
            if(otherCollider.CompareTag("Player")) {
                PlayerCharKill otherPlayerCharKill = otherCollider.gameObject.GetComponent<PlayerCharKill>();
                if(playerCharKillTargets.Contains(otherPlayerCharKill)) {
                    playerCharKillTargets.Remove(otherPlayerCharKill);
                }
            }
        }

        #endregion

        private void KennaKilled() {
            isDead = true;
            transform.position -= new Vector3(0.0f, 0.0f, -0.1f); //Ensure killer renders over killed player

            ///Make player look like a ghost
            try {
                Transform childTransform0 = gameObject.transform.Find("PlayerCharOutfitSprite");
                SpriteRenderer childSpriteRenderer0 = childTransform0.GetComponent<SpriteRenderer>();
                childSpriteRenderer0.color -= new Color(0.0f, 0.0f, 0.0f, 0.5f);

                Transform childTransform1 = gameObject.transform.Find("PlayerCharSprite");
                SpriteRenderer childSpriteRenderer1 = childTransform1.GetComponent<SpriteRenderer>();
                childSpriteRenderer1.color = new Color(0.5f, 0.5f, 1.0f, 0.5f);
            } catch(NullReferenceException) {
                UnityEngine.Assertions.Assert.IsTrue(false);
            }

            myCollider.enabled = false;
        }
    }
}