using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class SendMsg: MonoBehaviour {
        #region Fields

        [SerializeField] private InputField msgBox;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public SendMsg() {
            msgBox = null;
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public void OnSendButtonPressed() {
            GameManager.CreateMsgListItem();

            msgBox.text = string.Empty;
        }
    }
}