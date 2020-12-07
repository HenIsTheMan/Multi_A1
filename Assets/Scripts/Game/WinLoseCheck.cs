using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Impasta.Game {
    internal sealed class WinLoseCheck: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties

        public static bool CheckWinLose {
            get;
            set;
        } = false;

        #endregion

        #region Ctors and Dtor
        #endregion

        #region Unity User Callback Event Funcs

        private void Update() {
            if(!CheckWinLose) {
                return;
            }

            UniversalTaskRatio.CalcSums(out int completeTasksSum, out int tasksSum);
            if(completeTasksSum == tasksSum && tasksSum != 0) {
                _ = StartCoroutine(nameof(DisconnectAndLoad));
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
                _ = StartCoroutine(nameof(DisconnectAndLoad));
            }
        }

        private System.Collections.IEnumerator DisconnectAndLoad() {
            PhotonNetwork.Disconnect();
            //PhotonNetwork.LeaveRoom();

            while(PhotonNetwork.IsConnected) {
                yield return null;
            }
            //while(PhotonNetwork.InRoom) {
            //  yield return null;
            //}

            SceneManager.LoadScene(0);
        }

        #endregion
    }
}