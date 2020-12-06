using UnityEngine;

namespace Impasta.Game {
    internal sealed class TaskBlock: MonoBehaviour {
        #region Fields

        private GameObject playerCharTagObjNearby;
        private TaskTypes.TaskType taskType;

        [SerializeField] private GameObject taskCanvasGO;
        [SerializeField] private MeshRenderer meshRendererComponent;

        #endregion

        #region Properties

        public GameObject PlayerCharTagObjNearby {
            get {
                return playerCharTagObjNearby;
            }
            set {
                playerCharTagObjNearby = value;
            }
        }

        public TaskTypes.TaskType TaskType {
            get {
                return taskType;
            }
            set {
                taskType = value;

                Material mtl = meshRendererComponent.material;
                if(taskType == TaskTypes.TaskType.NoTask) {
                    mtl.SetColor("_Color", Color.white);
                } else if(taskType == TaskTypes.TaskType.TaskDone) {
                    mtl.SetColor("_Color", Color.green);
                } else {
                    mtl.SetColor("_Color", Color.red);
                }
            }
        }

        #endregion

        #region Ctors and Dtor

        public TaskBlock() {
           playerCharTagObjNearby = null;
           taskType = TaskTypes.TaskType.NoTask;

           taskCanvasGO = null;
           meshRendererComponent = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Update() {
            if(playerCharTagObjNearby != null && Input.GetKeyDown(KeyCode.Space)
                && !playerCharTagObjNearby.GetComponent<PlayerCharKill>().IsImposter
                && (byte)taskType > (byte)TaskTypes.TaskType.TaskDone) {
                taskCanvasGO.SetActive(!taskCanvasGO.activeSelf);
                playerCharTagObjNearby.GetComponent<PlayerCharMovement>().CanMove = !playerCharTagObjNearby.GetComponent<PlayerCharMovement>().CanMove;
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