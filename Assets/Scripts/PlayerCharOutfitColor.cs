using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Impasta.Game{
    internal sealed class PlayerCharOutfitColor: MonoBehaviour {
        #region Fields

        private PhotonView photonView;
        private SpriteRenderer childSpriteRenderer;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public PlayerCharOutfitColor() {
            photonView = null;
            childSpriteRenderer = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            childSpriteRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            UnityEngine.Assertions.Assert.IsNotNull(childSpriteRenderer);

            photonView = gameObject.GetComponent<PhotonView>();
            UnityEngine.Assertions.Assert.IsNotNull(photonView);
        }

        private void Start() {
            childSpriteRenderer.color = PlayerColors.GetPlayerColor(photonView.Owner.GetPlayerNumber());
        }

        #endregion
    }
}