using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Impasta.Game{
    internal sealed class PlayerCharOutfitColor: MonoBehaviour, IPunInstantiateMagicCallback {
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

        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            int index = info.Sender.ActorNumber - 1;
            gameObject.name = "PlayerChar" + index;

            Transform childTransform = gameObject.transform.Find("PlayerNameCanvas");
            Transform grandchildTransform = childTransform.Find("PlayerNameText");

            UnityEngine.UI.Text textComponent = grandchildTransform.GetComponent<UnityEngine.UI.Text>();
            textComponent.text = gameObject.name + ' ' + info.Sender.NickName;

            float angle = (360.0f / (float)System.Convert.ToDouble(PhotonNetwork.CurrentRoom.PlayerCount)) * Mathf.Deg2Rad * (float)System.Convert.ToDouble(index);
            float radius = 3.0f;
            gameObject.transform.position = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0.0f) * radius;
        }

        private void Start() {
            childSpriteRenderer.color = PlayerUniversal.Colors[photonView.Owner.GetPlayerNumber()];
        }

        #endregion
    }
}