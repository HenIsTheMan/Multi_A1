using Photon.Pun;
using UnityEngine;

namespace Impasta {
    internal sealed class ReportRPC: MonoBehaviour {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private ReportRPC() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        [PunRPC] public void Report() {
            ///Return back to spawn pos
            int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            float angle = (360.0f / (float)System.Convert.ToDouble(PhotonNetwork.CurrentRoom.PlayerCount)) * Mathf.Deg2Rad * (float)System.Convert.ToDouble(index);
            float radius = 3.0f;
            ((GameObject)PhotonNetwork.LocalPlayer.TagObject).transform.position = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0.0f) * radius;

            ///Despawn all player char dead bodies
            GameObject[] playerCharBodies = GameObject.FindGameObjectsWithTag("DeadPlayer");
            int arrLen = playerCharBodies.Length;

            for(int i = 0; i < arrLen; ++i) {
                playerCharBodies[i].SetActive(false);
            }
        }
    }
}