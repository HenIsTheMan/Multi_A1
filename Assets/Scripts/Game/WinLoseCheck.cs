using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class WinLoseCheck: MonoBehaviour {
        #region Fields

        private CanvasGroup winLoseBG;
        private float timer;

        [SerializeField] private float fadeDuration;
        [SerializeField] private float displayImgDuration;

        [SerializeField] private CanvasGroup winBG;
        [SerializeField] private CanvasGroup loseBG;
        [SerializeField] private Text infoText;

        #endregion

        #region Properties

        public static bool CheckWinLose {
            get;
            set;
        } = false;

        #endregion

        #region Ctors and Dtor

        private WinLoseCheck() {
            winLoseBG = null;
            timer = 0.0f;

            fadeDuration = 0.0f;
            displayImgDuration = 0.0f;

            winBG = null;
            loseBG = null;
            infoText = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Update() {
            if(!CheckWinLose) {
                return;
            }
            if(winLoseBG != null) {
                EndGame(winLoseBG);
                return;
            }

            bool isImposter = ((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerCharKill>().IsImposter;

            UniversalTaskRatio.CalcSums(out int completeTasksSum, out int tasksSum);
            if(completeTasksSum == tasksSum && tasksSum != 0) {
                if(isImposter) {
                    infoText.text = "Impastas lose...";
                    infoText.color = Color.red;

                    winLoseBG = loseBG;
                } else {
                    infoText.text = "Humans win!";
                    infoText.color = Color.green;

                    winLoseBG = winBG;
                }
                return;
            }

            int aliveImposters = 0;
            int aliveHumans = 0;
            GameObject tagObj;
            PlayerCharKill playerCharKill;

            int arrLen = PhotonNetwork.CurrentRoom.PlayerCount;
            for(int i = 0; i < arrLen; ++i) {
                tagObj = (GameObject)PhotonNetwork.PlayerList[i].TagObject;

                if(tagObj) {
                    playerCharKill = tagObj.GetComponent<PlayerCharKill>();

                    if(!playerCharKill.IsDead) {
                        if(playerCharKill.IsImposter) {
                            ++aliveImposters;
                        } else {
                            ++aliveHumans;
                        }
                    }
                }
            }

            if(aliveImposters == aliveHumans) {
                if(isImposter) {
                    infoText.text = "Impastas win!";
                    infoText.color = Color.green;

                    winLoseBG = winBG;
                } else {
                    infoText.text = "Humans lose...";
                    infoText.color = Color.red;

                    winLoseBG = loseBG;
                }
            }
        }

        private void EndGame(CanvasGroup imgCanvasGroup) {
            timer += Time.deltaTime;
            imgCanvasGroup.alpha = timer / fadeDuration;

            if(timer > fadeDuration + displayImgDuration) {
                _ = StartCoroutine(nameof(DisconnectAndLoad));
            }
        }

        private System.Collections.IEnumerator DisconnectAndLoad() {
            PhotonNetwork.Disconnect();

            while(PhotonNetwork.IsConnected) {
                yield return null;
            }

            SceneManager.LoadScene(0);
        }

        #endregion
    }
}