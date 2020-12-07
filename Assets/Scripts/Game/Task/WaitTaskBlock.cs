using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class WaitTaskBlock: TaskBlock {
        #region Fields

        private float waitTime;

        private Text textComponent;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public WaitTaskBlock(): base() {
            waitTime = 0.0f;

            textComponent = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
           waitTime = Random.Range(4.0f, 7.0f);

           textComponent = taskCanvasGO.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        }

		private new void Update() => base.Update();

        #endregion

        protected override void TaskLogic() {
            textComponent.text = Mathf.Ceil(waitTime).ToString();
            waitTime -= Time.deltaTime;

            if(waitTime <= 0.0f) {
                TaskCompleted();
            }
        }
    }
}