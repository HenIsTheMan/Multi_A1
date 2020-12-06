using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class PlayerCharKill: MonoBehaviour {
        #region Fields

        private bool isDead;
        private bool isImposter;
        private bool isKillButtonPressed;
        private float killCooldownTime;

        private List<PlayerCharKill> playerCharKillTargets;

        private PlayerCharReport playerCharReport;

        private Text killCooldownTimeTextComponent;

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
            killCooldownTime = 30.0f;

            playerCharKillTargets = null;

			playerCharReport = null;

            killCooldownTimeTextComponent = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            playerCharReport = GetComponent<PlayerCharReport>();
            killCooldownTimeTextComponent = GameObject.Find("KillCooldownTimeText").GetComponent<Text>();
        }

        private void Start() {
            playerCharKillTargets = new List<PlayerCharKill>();
        }

        private void Update() {
            if(gameObject == (GameObject)PhotonNetwork.LocalPlayer.TagObject && isImposter) {
                killCooldownTime -= Time.deltaTime;
                killCooldownTimeTextComponent.text = (Mathf.Max(0, (int)killCooldownTime)).ToString();
            }

            if(Input.GetButtonDown("Kill")) {
                isKillButtonPressed = true;
            }
        }

        private void FixedUpdate(){
            if(isKillButtonPressed && isImposter && !isDead && killCooldownTime <= 0.0f) {
                int listCount = playerCharKillTargets.Count;

                if(listCount > 0) { //Find nearest alive human to kill
                    float currShortestDist = float.MaxValue;
                    PlayerCharKill currClosestTargetPlayerCharKill = null;

                    for(int i = 0; i < listCount; ++i) {
                        PlayerCharKill targetPlayerCharKill = playerCharKillTargets[i];

                        if(!targetPlayerCharKill.isImposter && !targetPlayerCharKill.isDead) {
                            if(currClosestTargetPlayerCharKill == null) {
                                currClosestTargetPlayerCharKill = targetPlayerCharKill;
                            } else {
                                Vector3 GOPos = transform.position;
                                Vector3 targetPos = targetPlayerCharKill.transform.position;
                                Vector3 GOPosXY = new Vector3(GOPos.x, GOPos.y, 0.0f);
                                Vector3 targetPosXY = new Vector3(targetPos.x, targetPos.y, 0.0f);

                                float dist = (GOPosXY - targetPosXY).magnitude;
                                if(dist < currShortestDist) {
                                    currShortestDist = dist;
                                    currClosestTargetPlayerCharKill = targetPlayerCharKill;
                                }
                            }
                        }
                    }

                    if(currClosestTargetPlayerCharKill != null) {
                        Vector3 targetPos = currClosestTargetPlayerCharKill.transform.position;

                        transform.position = targetPos;

                        if(gameObject == (GameObject)PhotonNetwork.LocalPlayer.TagObject) {
                            killCooldownTime = 30.0f;
                        }

                        PhotonView.Get(this).RPC("Kill", RpcTarget.All, currClosestTargetPlayerCharKill.name, targetPos);
                    }
                }

                isKillButtonPressed = false;
            }
        }

        public void Triggering(Collider otherCollider){
            if(isImposter && !isDead) {
                PlayerCharKill otherPlayerCharKill = otherCollider.transform.parent.GetComponent<PlayerCharKill>();

                if(!playerCharKillTargets.Contains(otherPlayerCharKill)) {
                    playerCharKillTargets.Add(otherPlayerCharKill);
                }
            }
        }

        public void NotTriggering(Collider otherCollider) {
            if(isImposter && !isDead) {
                PlayerCharKill otherPlayerCharKill = otherCollider.transform.parent.GetComponent<PlayerCharKill>();

                if(playerCharKillTargets.Contains(otherPlayerCharKill)) {
                    playerCharKillTargets.Remove(otherPlayerCharKill);
                }
            }
        }

        #endregion

        private void UpdateIsDead(bool isDead) {
            this.isDead = isDead;
            playerCharReport.UpdateIsDead(isDead);
        }

        public void KennaKilled() {
            UpdateIsDead(true);

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

            ///Set visibility of ghost(s)
            if(name == ((GameObject)PhotonNetwork.LocalPlayer.TagObject).name) {
                GameObject[] playerCharGOs = GameObject.FindGameObjectsWithTag("Player");
                int arrLen = playerCharGOs.Length;

                for(int i = 0; i < arrLen; ++i) {
                    GameObject playerChar = playerCharGOs[i];

                    SpriteRenderer spriteRenderer0 = playerChar.transform.GetChild(0).GetComponent<SpriteRenderer>();
                    Color color0 = spriteRenderer0.color;
                    spriteRenderer0.GetComponent<SpriteRenderer>().color = new Color(
                        color0.r,
                        color0.g,
                        color0.b,
                        1.0f
                    );

                    SpriteRenderer spriteRenderer1 = playerChar.transform.GetChild(1).GetComponent<SpriteRenderer>();
                    Color color1 = spriteRenderer1.GetComponent<SpriteRenderer>().color;
                    spriteRenderer1.GetComponent<SpriteRenderer>().color = new Color(
                        color1.r,
                        color1.g,
                        color1.b,
                        1.0f
                    );

                    playerChar.transform.GetChild(3).gameObject.SetActive(true);
                }
            } else if(!((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerCharKill>().isDead){
                SpriteRenderer spriteRenderer0 = transform.GetChild(0).GetComponent<SpriteRenderer>();
                Color color0 = spriteRenderer0.color;
                spriteRenderer0.GetComponent<SpriteRenderer>().color = new Color(
                    color0.r,
                    color0.g,
                    color0.b,
                    0.0f
                );

                SpriteRenderer spriteRenderer1 = transform.GetChild(1).GetComponent<SpriteRenderer>();
                Color color1 = spriteRenderer1.GetComponent<SpriteRenderer>().color;
                spriteRenderer1.GetComponent<SpriteRenderer>().color = new Color(
                    color1.r,
                    color1.g,
                    color1.b,
                    0.0f
                );

                transform.GetChild(3).gameObject.SetActive(false);
            }
        }
    }
}