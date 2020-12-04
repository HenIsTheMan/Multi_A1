using Photon.Pun;
using UnityEngine;

namespace Impasta{
    internal sealed class SetPlayerColorsRPC: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private SetPlayerColorsRPC() {
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

        [PunRPC] public void SetPlayerColors(Vector3[] vecs) {
            int arrLen = vecs.Length;
            PlayerColors.Colors = new Color[arrLen];

            for(int i = 0; i < arrLen; ++i) {
                Vector3 vec = vecs[i];
                PlayerColors.Colors[i] = new Color(vec.x, vec.y, vec.z, 1.0f);
            }
        }
    }
}