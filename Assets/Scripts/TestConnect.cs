//Copyright (c) Ling Guan Yu (193541T, NYP SIDM GDT 1904)

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TestConnect: MonoBehaviourPunCallbacks {
    #region Fields

    [SerializeField] private GameSettings gameSettings;

    #endregion

    #region Properties
    #endregion

    #region Ctors and Dtor

    public TestConnect():
        base()
    {
        gameSettings = null;
    }

    #endregion

    #region Unity User Callback Event Funcs

    private void Start() {
        print("Connecting to server...");
        PhotonNetwork.GameVersion = gameSettings.GameVer; //Local ver
        PhotonNetwork.NickName = gameSettings.Nickname;
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    public override void OnConnectedToMaster() {
        Debug.Log("<color=green>Connected to server</color>", this);
        print("User: " + PhotonNetwork.LocalPlayer.NickName); //Server ver
    }

    public override void OnDisconnected(DisconnectCause cause) {
        Debug.Log("<color=green>Disconnected from server: " + cause + "</color>", this);
    }
}