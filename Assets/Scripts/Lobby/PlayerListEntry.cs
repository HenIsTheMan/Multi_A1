using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace Impasta.Lobby {
    internal class PlayerListEntry: MonoBehaviour {
        #region Fields

        private int ownerId;
        private bool isPlayerReady;

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

        }

        #endregion

        #region Unity User Callback Event Funcs

        private void OnEnable() {
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }

        private void Start() {
            if(PhotonNetwork.LocalPlayer.ActorNumber != ownerId) {
                PlayerReadyButton.gameObject.SetActive(false);
            } else {
                Hashtable initialProps = new Hashtable() {{JLGame.PLAYER_READY, isPlayerReady}, {JLGame.PLAYER_LIVES, JLGame.PLAYER_MAX_LIVES}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
                PhotonNetwork.LocalPlayer.SetScore(0);

                PlayerReadyButton.onClick.AddListener(() => {
                    isPlayerReady = !isPlayerReady;
                    SetPlayerReady(isPlayerReady);

                    Hashtable props = new Hashtable() {{JLGame.PLAYER_READY, isPlayerReady}};
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    if(PhotonNetwork.IsMasterClient) {
                        FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
                    }
                });
            }
        }

        private void OnDisable() {
            PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
        }

        #endregion

        public void Initialize(int playerId, string playerName) {
            ownerId = playerId;
            PlayerNameText.text = playerName;
        }

        private void OnPlayerNumberingChanged() {
            foreach(Player p in PhotonNetwork.PlayerList) {
                if(p.ActorNumber == ownerId) {
                    PlayerColorImage.color = JLGame.GetColor(p.GetPlayerNumber());
                }
            }
        }

        public void SetPlayerReady(bool playerReady) {
            PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Ready!" : "Ready?";
            PlayerReadyImage.enabled = playerReady;
        }
    }
}