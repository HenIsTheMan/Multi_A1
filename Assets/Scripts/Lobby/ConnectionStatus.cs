using Photon.Pun;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

internal class ConnectionStatus: MonoBehaviour {
    #region Fields

    [SerializeField] private string leftText;
    [SerializeField] private Text connectionStatus;

    #endregion

    #region Properties
    #endregion

    #region Ctors and Dtor

    public ConnectionStatus() {
        leftText = "";
        connectionStatus = null;
    }

    #endregion

    #region Unity User Callback Event Funcs

    private void Start() {
        Assert.IsNotNull(connectionStatus);
    }

    private void Update() {
        connectionStatus.text = leftText + PhotonNetwork.NetworkClientState;
    }

    #endregion
}