using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class PlayerCharTasks: MonoBehaviour {
        #region Fields

        private int amtOfIncompleteTasks;
        private int amtOfCompleteTasks;

        private Text tasksTextComponent;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public PlayerCharTasks() {
            amtOfIncompleteTasks = 0;
            amtOfCompleteTasks = 0;

            tasksTextComponent = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            tasksTextComponent = GameObject.Find("TasksText").GetComponent<Text>();
        }

        private void Update() {
            if(gameObject == (GameObject)PhotonNetwork.LocalPlayer.TagObject) {
                tasksTextComponent.text = amtOfIncompleteTasks.ToString() + '/' + amtOfCompleteTasks;
            }
        }

        #endregion
    }
}