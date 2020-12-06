using UnityEngine;

namespace Impasta.Game {
    internal sealed class TaskBlock: MonoBehaviour {
        #region Fields

        private bool playerCharTagObjNearby;

        [SerializeField] private GameObject taskCanvasGO;
        [SerializeField] private TaskTypes.TaskType taskType;

        #endregion

        #region Properties

        public bool PlayerCharTagObjNearby {
            get {
                return playerCharTagObjNearby;
            }
            set {
                playerCharTagObjNearby = value;
            }
        }

        #endregion

        #region Ctors and Dtor

        public TaskBlock() {
           playerCharTagObjNearby = false;

           taskCanvasGO = null;
           taskType = TaskTypes.TaskType.Amt;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Update() {
            if(playerCharTagObjNearby && Input.GetKeyDown(KeyCode.Space)) {
                taskCanvasGO.SetActive(!taskCanvasGO.activeSelf);
            }

            if(taskCanvasGO.activeSelf) {
                switch(taskType) {
                    case TaskTypes.TaskType.WaitTask:
                        break;
                }
            }
        }

        #endregion
    }
}