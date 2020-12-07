namespace Impasta.Game {
    internal sealed class EventCodes {
        public enum EventCode: byte {
            NotAnEvent,
            CompletedTaskEvent,
            DisablePlayerSpriteAniEvent,
            MsgSentByGhostEvent,
            RetrievePlayerRolesEvent,
            SetPlayerRolesEvent,
            SetPlayerTotalAmtOfTasksEvent,
            UpdateVoteCountEvent,
            Amt
        };

        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private EventCodes() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion
    }
}