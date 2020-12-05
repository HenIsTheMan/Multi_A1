using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class PlayerCharOutfitColor: MonoBehaviour {
        #region Fields

        private PhotonView photonView;

        [SerializeField] private SpriteRenderer outfitRenderer;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public PlayerCharOutfitColor() {
            photonView = null;
            outfitRenderer = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            photonView = gameObject.GetComponent<PhotonView>();
            UnityEngine.Assertions.Assert.IsNotNull(photonView);
        }

        private void Start() {
            outfitRenderer.color = PlayerUniversal.Colors[photonView.Owner.GetPlayerNumber()];
        }

        #endregion
    }
}