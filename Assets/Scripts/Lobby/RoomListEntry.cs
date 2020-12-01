using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Lobby {
    internal sealed class RoomListEntry: MonoBehaviour {
        #region Fields

        private string roomName;

        [SerializeField] private Text RoomNameText;
        [SerializeField] private Text RoomPlayersText;
        [SerializeField] private Button JoinRoomButton;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public RoomListEntry() {
            roomName = "";

            RoomNameText = null;
            RoomPlayersText = null;
            JoinRoomButton = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Start() {
            JoinRoomButton.onClick.AddListener(() => {
                if(PhotonNetwork.InLobby) {
                    PhotonNetwork.LeaveLobby();
                }
                PhotonNetwork.JoinRoom(roomName);
            });
        }

        #endregion

        public void Initialize(string name, byte currentPlayers, byte maxPlayers) {
            roomName = name;
            RoomNameText.text = name;
            RoomPlayersText.text = currentPlayers + " / " + maxPlayers;
        }
    }
}