//Copyright (c) Ling Guan Yu (193541T, NYP SIDM GDT 1904)

using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class TestConnect: MonoBehaviourPunCallbacks {
    #region Fields
    #endregion

    #region Properties
    #endregion

    #region Ctors and Dtor

    public TestConnect():
        base()
    {
    }

    #endregion

    #region Unity User Callback Event Funcs

    private void Start() {
        print("Connecting to server...");
        PhotonNetwork.GameVersion = "4.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    #endregion

    public override void OnConnectedToMaster() {
        print("Connected to server");
    }

    public override void OnDisconnected(DisconnectCause cause) {
        print("Disconnected to server: " + cause.ToString());
    }
}