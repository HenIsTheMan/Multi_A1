using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Impasta.Lobby {
    internal sealed class PlayerListEntry: MonoBehaviour {
        #region Fields

        private bool isPlayerReady;
        private int ownerID;

        [Header("UI Refs")]
        [SerializeField] private Text PlayerNameText;
        [SerializeField] private Image PlayerColorImage;
        [SerializeField] private Button PlayerReadyButton;
        [SerializeField] private Image PlayerReadyImage;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public PlayerListEntry() {
            isPlayerReady = false;
            ownerID = 0;

            PlayerNameText = null;
            PlayerColorImage = null;
            PlayerReadyButton = null;
            PlayerReadyImage = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Start() {
            if(PhotonNetwork.LocalPlayer.ActorNumber != ownerID) {
                PlayerReadyButton.gameObject.SetActive(false);
            } else {
                Hashtable initialProps = new Hashtable() { { "IsPlayerReady", isPlayerReady }, { "PlayerLives", "PlayerMaxLives" } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
                PhotonNetwork.LocalPlayer.SetScore(0);

                PlayerReadyButton.onClick.AddListener(() => {
                    isPlayerReady = !isPlayerReady;
                    SetPlayerReady(isPlayerReady);

                    Hashtable props = new Hashtable() { { "IsPlayerReady", isPlayerReady } };
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    if(PhotonNetwork.IsMasterClient) {
                        FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
                    }
                });
            }
        }

        #endregion

        public void SetPlayerListEntryColors() {
            Debug.Log("HereNoob");

            int playerListArrLen = PhotonNetwork.PlayerList.Length;
            for(int i = 0; i < playerListArrLen; ++i) {
                if(PhotonNetwork.PlayerList[i].ActorNumber == ownerID) {
                    PlayerColorImage.color = PlayerColors.GetPlayerColor(i);
                    break;
                }
            }
        }

        public void Initialize(int playerId, string playerName) {
            ownerID = playerId;
            PlayerNameText.text = playerName;
        }

        public void SetPlayerReady(bool playerReady) {
            PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Very Ready" : "Not Ready";
            PlayerReadyImage.enabled = playerReady;
        }
    }
}