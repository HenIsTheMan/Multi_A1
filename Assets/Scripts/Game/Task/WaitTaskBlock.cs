using UnityEngine;

namespace Impasta.Game {
    internal sealed class WaitTaskBlock: TaskBlock {
        #region Fields

        private float waitTime;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public WaitTaskBlock() : base() {
            waitTime = 0.0f;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
           waitTime = Random.Range(4.0f, 7.0f);
        }

		private new void Update() => base.Update();

        #endregion

        protected override void TaskLogic() {
            waitTime -= Time.deltaTime;
            if(waitTime <= 0.0f) {
                TaskCompleted();
            }
        }
    }
}