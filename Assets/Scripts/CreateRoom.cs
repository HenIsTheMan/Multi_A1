//Copyright (c) Ling Guan Yu (193541T, NYP SIDM GDT 1904)

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom: MonoBehaviourPunCallbacks {
    #region Fields

    [SerializeField] private Text roomName;

    #endregion

    #region Properties
    #endregion

    #region Ctors and Dtor
    
    public CreateRoom() {
        roomName = null;
    }

    #endregion

    #region Unity User Callback Event Funcs
    #endregion

    public void OnCreateRoomButtonPressed() {
        if(!PhotonNetwork.IsConnected) {
            Debug.Log("<color=red>Err: PhotonNetwork is not connected!</color>", this);
            return;
        }

		RoomOptions options = new RoomOptions {
			MaxPlayers = 10
		};

        if(roomName.text != "") {
            PhotonNetwork.JoinOrCreateRoom(roomName.text, options, TypedLobby.Default, null);
            roomName.text = "";
        } else {
            Debug.Log("<color=purple>Info: Room name text is empty!</color>", this);
        }
    }

    public override void OnCreatedRoom() {
        Debug.Log("Room creation success!", this);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        Debug.Log("Room creation failure (" + returnCode + "): " + message, this);
    }
}