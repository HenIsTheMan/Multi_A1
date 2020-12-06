﻿using UnityEngine;

namespace Impasta.Game {
    internal sealed class TaskBlock: MonoBehaviour {
        #region Fields

        private GameObject playerCharTagObjNearby;

        [SerializeField] private GameObject taskCanvasGO;
        [SerializeField] private TaskTypes.TaskType taskType;

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

        #endregion

        #region Ctors and Dtor

        public TaskBlock() {
           playerCharTagObjNearby = null;

           taskCanvasGO = null;
           taskType = TaskTypes.TaskType.Amt;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Update() {
            if(playerCharTagObjNearby != null && Input.GetKeyDown(KeyCode.Space)) {
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