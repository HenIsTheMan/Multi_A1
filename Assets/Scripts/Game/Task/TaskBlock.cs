using UnityEngine;

namespace Impasta.Game {
    internal abstract class TaskBlock: MonoBehaviour {
        #region Fields

        protected GameObject playerCharTagObjNearby;
        protected TaskStatuses.TaskStatus myTaskStatus;

        [SerializeField] protected GameObject taskCanvasGO;
        [SerializeField] protected MeshRenderer meshRendererComponent;

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

        public TaskStatuses.TaskStatus MyTaskStatus {
            get {
                return myTaskStatus;
            }
            set {
                myTaskStatus = value;

                Material mtl = meshRendererComponent.material;
                if(myTaskStatus == TaskStatuses.TaskStatus.None) {
                    mtl.SetColor("_Color", Color.white);
                } else if(myTaskStatus == TaskStatuses.TaskStatus.Done) {
                    mtl.SetColor("_Color", Color.green);
                } else {
                    mtl.SetColor("_Color", Color.red);
                }
            }
        }

        #endregion

        #region Ctors and Dtor

        protected TaskBlock() {
           playerCharTagObjNearby = null;
           myTaskStatus = TaskStatuses.TaskStatus.None;

           taskCanvasGO = null;
           meshRendererComponent = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        protected void Update() {
            if(playerCharTagObjNearby != null && Input.GetKeyDown(KeyCode.Space)
                && !playerCharTagObjNearby.GetComponent<PlayerCharKill>().IsImposter
                && myTaskStatus == TaskStatuses.TaskStatus.NotDone) {
                taskCanvasGO.SetActive(!taskCanvasGO.activeSelf);
                playerCharTagObjNearby.GetComponent<PlayerCharMovement>().CanMove = !playerCharTagObjNearby.GetComponent<PlayerCharMovement>().CanMove;
            }

            if(taskCanvasGO.activeSelf) {
                TaskLogic();
            }
        }

        protected abstract void TaskLogic();

        #endregion
    }
}