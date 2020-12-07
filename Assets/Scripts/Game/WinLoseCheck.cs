using Photon.Pun;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class WinLoseCheck: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor
        #endregion

        #region Unity User Callback Event Funcs

        private void Update() {
            UniversalTaskRatio.CalcSums(out int completeTasksSum, out int tasksSum);
            if(completeTasksSum == tasksSum) {
                Debug.Log("Here1", this);
                //
                return;
            }

            int aliveImposters = 0;
            int aliveHumans = 0;
            GameObject tagObj;
            PlayerCharKill playerCharKill;

            int arrLen = PhotonNetwork.CurrentRoom.PlayerCount;
            for(int i = 0; i < arrLen; ++i) {
                tagObj = (GameObject)PhotonNetwork.PlayerList[i].TagObject;
                playerCharKill = tagObj.GetComponent<PlayerCharKill>();

                if(!playerCharKill.IsDead) {
                    if(playerCharKill.IsImposter) {
                        ++aliveImposters;
                    } else {
                        ++aliveHumans;
                    }
                }
            }

            if(aliveImposters == aliveHumans) {
                Debug.Log("Here2", this);
                //
            }
        }

        #endregion
    }
}