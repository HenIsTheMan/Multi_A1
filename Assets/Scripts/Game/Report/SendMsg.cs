using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class SendMsg: MonoBehaviour {
        #region Fields

        [SerializeField] private Text msg;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public SendMsg() {
            msg = null;
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public void OnSendButtonPressed() {
            msg.text = string.Empty;
        }
    }
}