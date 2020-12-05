using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class PlayerCharOutfitColor: MonoBehaviour {
        #region Fields

        [SerializeField] private SpriteRenderer outfitRenderer;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public PlayerCharOutfitColor() {
            outfitRenderer = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Start() {
            outfitRenderer.color = PlayerUniversal.Colors[PhotonView.Get(this).Owner.GetPlayerNumber()];
        }

        #endregion
    }
}