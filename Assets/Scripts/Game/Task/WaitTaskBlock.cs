namespace Impasta.Game {
    internal sealed class WaitTaskBlock: TaskBlock {
        #region Fields

        private float waitTime;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public WaitTaskBlock(): base()
        {
            waitTime = UnityEngine.Random.Range(4.0f, 7.0f);
        }

		#endregion

		#region Unity User Callback Event Funcs

		private new void Update() => base.Update();

        protected override void TaskLogic() {
        }

        #endregion
    }
}