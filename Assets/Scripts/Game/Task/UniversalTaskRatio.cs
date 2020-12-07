using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class UniversalTaskRatio: MonoBehaviour {
        #region Fields

        [SerializeField] private Text textComponent;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public UniversalTaskRatio() {
            textComponent = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Update() {
            GameObject tagObj = (GameObject)PhotonNetwork.LocalPlayer.TagObject;
            if(tagObj != null && !tagObj.GetComponent<PlayerCharKill>().IsImposter) {
                GameObject[] playerChars = GameObject.FindGameObjectsWithTag("Player");
                int completeTasksSum = 0;
                int tasksSum = 0;
                int arrLen = playerChars.Length;

                for(int i = 0; i < arrLen; ++i) {
                    GameObject playerChar = playerChars[i];
                    if(!playerChar.GetComponent<PlayerCharKill>().IsImposter) {
                        PlayerCharTasks playerCharTasks = playerChar.GetComponent<PlayerCharTasks>();
                        completeTasksSum += playerCharTasks.AmtOfCompleteTasks;
                        tasksSum += playerCharTasks.TotalAmtOfTasks;
                        break;
                    }
                }

                textComponent.text = completeTasksSum.ToString() + '/' + tasksSum;
            }
        }

        #endregion
    }
}