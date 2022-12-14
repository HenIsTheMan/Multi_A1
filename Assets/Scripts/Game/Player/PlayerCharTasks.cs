using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class PlayerCharTasks: MonoBehaviour {
        #region Fields

        private int totalAmtOfTasks;
        private int amtOfCompleteTasks;

        private PlayerCharKill playerCharKill;

        private Text localTaskRatioTextComponent;

        #endregion

        #region Properties

        public int TotalAmtOfTasks {
            get {
                return totalAmtOfTasks;
            }
            set {
                totalAmtOfTasks = value;
            }
        }

        public int AmtOfCompleteTasks {
            get {
                return amtOfCompleteTasks;
            }
            set {
                amtOfCompleteTasks = value;
            }
        }

        #endregion

        #region Ctors and Dtor

        public PlayerCharTasks() {
            totalAmtOfTasks = 0;
            amtOfCompleteTasks = 0;

            playerCharKill = null;

            localTaskRatioTextComponent = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            playerCharKill = gameObject.GetComponent<PlayerCharKill>();

            localTaskRatioTextComponent = GameObject.Find("LocalTaskRatioText").GetComponent<Text>();
        }

        private void Update() {
            if(!playerCharKill.IsImposter && gameObject == (GameObject)PhotonNetwork.LocalPlayer.TagObject) {
                localTaskRatioTextComponent.text = amtOfCompleteTasks.ToString() + '/' + totalAmtOfTasks;
            }
        }

        #endregion
    }
}