namespace Impasta.Game {
    internal sealed class EventCodes {
        public enum EventCode: byte {
            NotAnEvent,
            RetrievePlayerRolesEvent,
            SetPlayerRolesEvent,
            LogicIDUpdateEvent,
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