using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Impasta.Game;

namespace Impasta.Lobby {
    internal sealed class LobbyMainPanel: MonoBehaviourPunCallbacks {
        #region Fields

        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;
        private Dictionary<int, GameObject> playerListEntries;

        [SerializeField] private string lvlName;

        [Header("Login")]
        [SerializeField] private GameObject LoginPanel;
        [SerializeField] private InputField PlayerNameInput;

        [Header("Selection")]
        [SerializeField] private GameObject SelectionPanel;

        [Header("CreateRoom")]
        [SerializeField] private GameObject CreateRoomPanel;
        [SerializeField] private InputField RoomNameInputField;
        [SerializeField] private InputField MaxPlayersInputField;

        [Header("JoinRandRoom")]
        [SerializeField] private GameObject JoinRandomRoomPanel;

        [Header("RoomList")]
        [SerializeField] private GameObject RoomListPanel;
        [SerializeField] private GameObject RoomListContent;
        [SerializeField] private GameObject RoomListEntryPrefab;

        [Header("InsideRoom")]
        [SerializeField] private GameObject InsideRoomPanel;
        [SerializeField] private Button StartGameButton;
        [SerializeField] private GameObject PlayerListEntryPrefab;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public LobbyMainPanel() {
            cachedRoomList = null;
            roomListEntries = null;
            playerListEntries = null;

            lvlName = "";

            LoginPanel = null;
            PlayerNameInput = null;

            SelectionPanel = null;

            CreateRoomPanel = null;
            RoomNameInputField = null;
            MaxPlayersInputField = null;

            JoinRandomRoomPanel = null;

            RoomListPanel = null;
            RoomListContent = null;
            RoomListEntryPrefab = null;

            InsideRoomPanel = null;
            StartGameButton = null;
            PlayerListEntryPrefab = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            PhotonNetwork.AutomaticallySyncScene = true;

            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();

            PlayerNameInput.text = "Player " + Random.Range(1000, 10000);
        }

        #endregion

        #region Pun Callback Funcs

        public override void OnConnectedToMaster() {
            SetActivePanel(SelectionPanel.name);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList) {
            ClearRoomListView();
            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }

        public override void OnLeftLobby() {
            cachedRoomList.Clear();
            ClearRoomListView();
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            SetActivePanel(SelectionPanel.name);
        }

        public override void OnJoinRoomFailed(short returnCode, string message) {
            SetActivePanel(SelectionPanel.name);
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            string roomName = "Room " + Random.Range(1000, 10000);
            RoomOptions options = new RoomOptions {MaxPlayers = 8};

            PhotonNetwork.CreateRoom(roomName, options, null);
        }

        public override void OnJoinedRoom() {
            if(PlayerColors.Colors.Length == 0) {
                if(PhotonNetwork.IsMasterClient) {
                    PlayerColors.InitColors();
                } else {
                    PhotonView.Get(this).RPC("RetrievePlayerColors", RpcTarget.MasterClient);
                }
            }

            _ = StartCoroutine("My1stEverCoroutine");
        }

        private System.Collections.IEnumerator My1stEverCoroutine(){
            while(PlayerColors.Colors.Length == 0) {
                yield return null;
            }

            SetActivePanel(InsideRoomPanel.name);

            if(playerListEntries == null) {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            foreach(Player p in PhotonNetwork.PlayerList) {
                GameObject entry = Instantiate(PlayerListEntryPrefab);
                entry.transform.SetParent(InsideRoomPanel.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<PlayerListEntry>().Initialize(p.ActorNumber, p.NickName);

                if(p.CustomProperties.TryGetValue("IsPlayerReady", out object isPlayerReady)) { //Inline var declaration
                    entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
                }

                entry.GetComponent<PlayerListEntry>().SetPlayerListEntryColors();

                playerListEntries.Add(p.ActorNumber, entry);
            }

            StartGameButton.gameObject.SetActive(CheckPlayersReady());

            Hashtable props = new Hashtable {
                {"PlayerLoadedLevel", false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            yield return null;
        }

        public override void OnLeftRoom() {
            SetActivePanel(SelectionPanel.name);

            foreach(GameObject entry in playerListEntries.Values) {
                Destroy(entry.gameObject);
            }

            playerListEntries.Clear();
            playerListEntries = null;
        }

		public override void OnPlayerEnteredRoom(Player newPlayer) {
            StartCoroutine(My2ndEverCoroutine(newPlayer));
		}

        private System.Collections.IEnumerator My2ndEverCoroutine(Player newPlayer) {
            while(PlayerColors.Colors.Length == 0) {
                yield return null;
            }

            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(InsideRoomPanel.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

            entry.GetComponent<PlayerListEntry>().SetPlayerListEntryColors();

            playerListEntries.Add(newPlayer.ActorNumber, entry);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());

            yield return null;
        }


        public override void OnPlayerLeftRoom(Player otherPlayer) {
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public override void OnMasterClientSwitched(Player newMasterClient) {
            if(PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber) {
                StartGameButton.gameObject.SetActive(CheckPlayersReady());
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {
            if (playerListEntries == null) {
                playerListEntries = new Dictionary<int, GameObject>();
            }

			if(playerListEntries.TryGetValue(targetPlayer.ActorNumber, out GameObject entry)) { //Inline var declaration
                if(changedProps.TryGetValue("IsPlayerReady", out object isPlayerReady)) { //Inline var declaration
					entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);
				}
			}

			StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        #endregion

        #region UI Callback Funcs

        public void OnBackButtonClicked() {
            if(PhotonNetwork.InLobby) {
                PhotonNetwork.LeaveLobby();
            }
            SetActivePanel(SelectionPanel.name);
        }

        public void OnCreateRoomButtonClicked() {
            string roomName = RoomNameInputField.text;
            roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

			byte.TryParse(MaxPlayersInputField.text, out byte maxPlayers); //Inline var declaration
            maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 10);
            RoomOptions options = new RoomOptions {MaxPlayers = maxPlayers, PlayerTtl = 10000 };

            PhotonNetwork.CreateRoom(roomName, options, null);
        }

        public void OnJoinRandomRoomButtonClicked() {
            SetActivePanel(JoinRandomRoomPanel.name);
            PhotonNetwork.JoinRandomRoom();
        }

        public void OnLeaveGameButtonClicked() {
            PhotonNetwork.LeaveRoom();
        }

        public void OnLoginButtonClicked() {
            string playerName = PlayerNameInput.text;

            if(!playerName.Equals("")) {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                if(!PhotonNetwork.IsConnected) {
                    PhotonNetwork.ConnectUsingSettings();
                }
            } else {
                Debug.LogWarning("<color=yellow>Player Name is invalid!</color>");
            }
        }

        public void OnRoomListButtonClicked() {
            if(!PhotonNetwork.InLobby) {
                PhotonNetwork.JoinLobby();
            }
            SetActivePanel(RoomListPanel.name);
        }

        public void OnStartGameButtonClicked() {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(lvlName);
        }

        #endregion

        private bool CheckPlayersReady() {
            if(!PhotonNetwork.IsMasterClient) {
                return false;
            }

            foreach(Player p in PhotonNetwork.PlayerList) {
				if(p.CustomProperties.TryGetValue("IsPlayerReady", out object isPlayerReady)) { //Inline var declaration
                    if(!(bool)isPlayerReady) {
						return false;
					}
				} else {
					return false;
				}
			}

            return true;
        }
        
        private void ClearRoomListView() {
            foreach (GameObject entry in roomListEntries.Values) {
                Destroy(entry.gameObject);
            }

            roomListEntries.Clear();
        }

        public void LocalPlayerPropertiesUpdated() {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        private void SetActivePanel(string activePanel) {
            LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
            SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
            CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
            JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
            RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name)); //UI should call OnRoomListButtonClicked() to activate this
            InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList) {
            foreach(RoomInfo info in roomList) {
                ///Remove room from cached room list if it got closed, became invisible or was marked as removed
                if(!info.IsOpen || !info.IsVisible || info.RemovedFromList) {
                    if(cachedRoomList.ContainsKey(info.Name)) {
                        cachedRoomList.Remove(info.Name);
                    }
                    continue;
                }

                if(cachedRoomList.ContainsKey(info.Name)) {
                    cachedRoomList[info.Name] = info; //Update cached room info
                } else {
                    cachedRoomList.Add(info.Name, info); //Add new room info to cache
                }
            }
        }

        private void UpdateRoomListView() {
            foreach(RoomInfo info in cachedRoomList.Values) {
                GameObject entry = Instantiate(RoomListEntryPrefab);
                entry.transform.SetParent(RoomListContent.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

                roomListEntries.Add(info.Name, entry);
            }
        }
    }
}