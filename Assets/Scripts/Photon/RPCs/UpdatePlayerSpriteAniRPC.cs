using Photon.Pun;
using UnityEngine;
using Impasta.Game;

namespace Impasta {
    internal sealed class UpdatePlayerSpriteAniRPC: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private UpdatePlayerSpriteAniRPC() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        [PunRPC] public void UpdatePlayerSpriteAni(string name, bool result0, bool result1, bool isFacingLeft) {
            GameObject playerChar = GameObject.Find(name);
            UnityEngine.Assertions.Assert.IsNotNull(playerChar);
            playerChar.GetComponent<PlayerCharMovement>().UpdateSpriteAni(result0, result1, isFacingLeft);
        }
    }
}