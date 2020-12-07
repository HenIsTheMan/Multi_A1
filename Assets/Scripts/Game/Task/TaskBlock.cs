using Photon.Pun;
using Photon.Realtime;
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
            if(myTaskStatus == TaskStatuses.TaskStatus.NotDone) {
                if(playerCharTagObjNearby != null && Input.GetKeyDown(KeyCode.Space)
                    && !playerCharTagObjNearby.GetComponent<PlayerCharKill>().IsImposter
                ) {
                    taskCanvasGO.SetActive(!taskCanvasGO.activeSelf);

                    PlayerCharMovement playerCharMovement = playerCharTagObjNearby.GetComponent<PlayerCharMovement>();
                    if(taskCanvasGO.activeSelf) {
                        playerCharMovement.CanMove = false;
                        playerCharMovement.RigidbodyComponent.velocity = Vector3.zero;

                        RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                            Receivers = ReceiverGroup.All
                        };
                        PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode.DisablePlayerSpriteAniEvent,
                            playerCharTagObjNearby.name, raiseEventOptions, ExitGames.Client.Photon.SendOptions.SendReliable);
                    } else {
                        playerCharMovement.CanMove = true;
                    }
                }

                if(taskCanvasGO.activeSelf) {
                    TaskLogic();
                }
            }
        }

        #endregion

        protected abstract void TaskLogic();

        protected void TaskCompleted() {
            taskCanvasGO.SetActive(false);
            playerCharTagObjNearby.GetComponent<PlayerCharMovement>().CanMove = true;

            //* MyTaskStatus = TaskStatuses.TaskStatus.Done;
            myTaskStatus = TaskStatuses.TaskStatus.Done;
            meshRendererComponent.material.SetColor("_Color", Color.green);
            //*/

            ++playerCharTagObjNearby.GetComponent<PlayerCharTasks>().AmtOfCompleteTasks;

            //object[] customData = new object[]{
            //        PhotonNetwork.LocalPlayer.NickName,
            //        new Vector3(myTagObjColor.r, myTagObjColor.g, myTagObjColor.b)
            //    };
            //RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
            //    Receivers = ReceiverGroup.All
            //};
            //PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode.MsgSentByGhostEvent,
            //    customData, raiseEventOptions, ExitGames.Client.Photon.SendOptions.SendReliable);
        }
    }
}