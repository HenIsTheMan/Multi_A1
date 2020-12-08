namespace Impasta.Game {
    internal sealed class ButtonTaskBlock: TaskBlock {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtzor

        private ButtonTaskBlock(): base() {
        }

        #endregion

        #region Unity User Callback Event Funcs

		private new void Update() => base.Update();

        #endregion

        protected override void TaskLogic() {
        }

        public void OnButtonClick() {
            TaskCompleted();
        }
    }
}