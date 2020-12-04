using Photon.Pun;
using UnityEngine;
using Impasta.Game;

namespace Impasta {
    internal sealed class ClientJoinedRoomRPC: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public ClientJoinedRoomRPC() {

        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        [PunRPC] public void ClientJoinedRoom() {
            int colorsArrLen = PlayerColors.Colors.Length;
            Vector3[] vecs = new Vector3[colorsArrLen];
            for(int i = 0; i < colorsArrLen; ++i) {
                Color color = PlayerColors.Colors[i];
                vecs[i] = new Vector3(color.r, color.g, color.b);
            }

            PhotonView.Get(this).RPC("SetPlayerColors", RpcTarget.Others, vecs);
        }
    }
}