//Copyright (c) Ling Guan Yu (193541T, NYP SIDM GDT 1904)

using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class RoomList: MonoBehaviourPunCallbacks {
    #region Fields

    [SerializeField] private RoomListItem roomListItem;
    [SerializeField] private Transform content;

    #endregion

    #region Properties
    #endregion

    #region Ctors and Dtor
    
    public RoomList() {
        roomListItem = null;
        content = null;
    }

    #endregion

    #region Unity User Callback Event Funcs
    #endregion

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        foreach(RoomInfo info in roomList) {
            Instantiate(roomListItem, content).SetRoomInfo(info);
        }
    }
}