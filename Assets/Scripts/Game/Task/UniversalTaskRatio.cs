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
			CalcSums(out int completeTasksSum, out int tasksSum); //Inline var declaration
			textComponent.text = completeTasksSum.ToString() + '/' + tasksSum;
        }

        public static void CalcSums(out int completeTasksSum, out int tasksSum) {
            GameObject[] playerChars = GameObject.FindGameObjectsWithTag("Player");
            int arrLen = playerChars.Length;

            completeTasksSum = 0;
            tasksSum = 0;

            for(int i = 0; i < arrLen; ++i) {
                GameObject playerChar = playerChars[i];
                if(!playerChar.GetComponent<PlayerCharKill>().IsImposter) {
                    PlayerCharTasks playerCharTasks = playerChar.GetComponent<PlayerCharTasks>();
                    completeTasksSum += playerCharTasks.AmtOfCompleteTasks;
                    tasksSum += playerCharTasks.TotalAmtOfTasks;
                }
            }
        }

        #endregion
    }
}