//Copyright (c) Ling Guan Yu (193541T, NYP SIDM GDT 1904)

using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem: MonoBehaviour {
    #region Fields

    [SerializeField] private Text myText;

    #endregion

    #region Properties
    #endregion

    #region Ctors and Dtor
    
    public RoomListItem() {
        myText = null;
    }

    #endregion

    #region Unity User Callback Event Funcs
    #endregion

    public void SetRoomInfo(RoomInfo info) {
        myText.text = $"{info.Name} ({info.PlayerCount}/{info.MaxPlayers})"; //Interpolated str
    }
}