using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class MsgListItem: MonoBehaviour {
        #region Fields

        [SerializeField] private Text textComponent;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public MsgListItem() {
            textComponent = null;
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public void SetMsgText(string text) {
            textComponent.text = text;
        }
    }
}