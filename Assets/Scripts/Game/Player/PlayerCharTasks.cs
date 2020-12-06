using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class PlayerCharTasks: MonoBehaviour {
        #region Fields

        private int amtOfIncompleteTasks;
        private int amtOfCompleteTasks;

        private PlayerCharKill playerCharKill;

        private Text tasksTextComponent;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public PlayerCharTasks() {
            amtOfIncompleteTasks = 0;
            amtOfCompleteTasks = 0;

            playerCharKill = null;

            tasksTextComponent = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            playerCharKill = gameObject.GetComponent<PlayerCharKill>();

            tasksTextComponent = GameObject.Find("TasksText").GetComponent<Text>();
        }

        private void Update() {
            if(!playerCharKill.IsImposter && gameObject == (GameObject)PhotonNetwork.LocalPlayer.TagObject) {
                tasksTextComponent.text = amtOfIncompleteTasks.ToString() + '/' + amtOfCompleteTasks;
            }
        }

        #endregion
    }
}