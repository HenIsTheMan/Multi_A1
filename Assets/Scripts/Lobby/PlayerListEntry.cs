using ExitGames.Client.Photon;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
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

        private void OnEnable() {
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }

        private void Start() {
            if(PhotonNetwork.LocalPlayer.ActorNumber != ownerID) {
                PlayerReadyButton.gameObject.SetActive(false);
            } else {
                Hashtable initialProps = new Hashtable() {{"IsPlayerReady", isPlayerReady}, {"PlayerLives", "PlayerMaxLives"} };
                PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
                PhotonNetwork.LocalPlayer.SetScore(0);

                PlayerReadyButton.onClick.AddListener(() => {
                    isPlayerReady = !isPlayerReady;
                    SetPlayerReady(isPlayerReady);

                    Hashtable props = new Hashtable() {{"IsPlayerReady", isPlayerReady}};
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
            ownerID = playerId;
            PlayerNameText.text = playerName;
        }

        private void OnPlayerNumberingChanged() {
            if(PhotonNetwork.IsMasterClient && PlayerColors.InitColors()) {
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                    Receivers = ReceiverGroup.All
                }; //Will receive event on local client too

                int colorsArrLen = PlayerColors.Colors.Length;
                Vector3[] vecs = new Vector3[colorsArrLen];
                for(int i = 0; i < colorsArrLen; ++i) {
                    Color color = PlayerColors.Colors[i];
                    vecs[i] = new Vector3(color.r, color.b, color.g);
                }

                PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode.InitColorsEvent, vecs, raiseEventOptions, SendOptions.SendReliable);
            }

            int playerListArrLen = PhotonNetwork.PlayerList.Length;
            for(int i = 0; i < playerListArrLen; ++i) {
                if(PhotonNetwork.PlayerList[i].ActorNumber == ownerID) {
                    PlayerColorImage.color = PlayerColors.GetPlayerColor(i);
                    break;
                }
            }
        }

        public void SetPlayerReady(bool playerReady) {
            PlayerReadyButton.GetComponentInChildren<Text>().text = playerReady ? "Very Ready" : "Not Ready";
            PlayerReadyImage.enabled = playerReady;
        }
    }
}