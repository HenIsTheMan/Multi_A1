using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class PressAndHoldTaskBlock: TaskBlock {
        #region Fields

        private bool hold;
        private float holdTime;

        private Text textComponent;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtzor

        private PressAndHoldTaskBlock() : base() {
            hold = false;
            holdTime = 0.0f;

            textComponent = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private new void Update() => base.Update();

        #endregion

        private void Awake() {
            holdTime = Random.Range(7.0f, 10.0f);
            textComponent.text = Mathf.Ceil(holdTime).ToString();

            textComponent = taskCanvasGO.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        }

        protected override void TaskLogic() {
            if(hold) {
                textComponent.text = Mathf.Ceil(holdTime).ToString();
                holdTime -= Time.deltaTime;

                if(holdTime <= 0.0f) {
                    TaskCompleted();
                }
            }
        }

        public void OnHold() {
            hold = true;
        }

        public void OnRelease() {
            hold = false;
        }
    }
}