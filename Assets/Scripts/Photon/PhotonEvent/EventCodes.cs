﻿namespace Impasta.Game {
    internal sealed class EventCodes {
        public enum EventCode: byte {
            NotAnEvent,
            MsgSentByGhostEvent,
            RetrievePlayerRolesEvent,
            SetPlayerRolesEvent,
            SetPlayerTasksEvent,
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