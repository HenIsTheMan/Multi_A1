using Impasta.Game;
using Photon.Pun;
using UnityEngine;

namespace Impasta {
    internal sealed class ReportRPC: MonoBehaviour {
        #region Fields

        private GameObject reportCanvas;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private ReportRPC() {
            reportCanvas = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Start() {
            reportCanvas = GameObject.Find("ReportCanvas");
        }

        #endregion

        [PunRPC] public void Report() {
            ///Return back to spawn pos
            int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            float angle = (360.0f / (float)System.Convert.ToDouble(PhotonNetwork.CurrentRoom.PlayerCount)) * Mathf.Deg2Rad * (float)System.Convert.ToDouble(index);
            float radius = 3.0f;

            ((GameObject)PhotonNetwork.LocalPlayer.TagObject).transform.position = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0.0f) * radius;

            ///Make all players unable to move during a report
            GameObject[] playerChars = GameObject.FindGameObjectsWithTag("Player");
            int playerCharsArrLen = playerChars.Length;
            for(int i = 0; i < playerCharsArrLen; ++i) {
                playerChars[i].GetComponent<PlayerCharMovement>().CanMove = false;
            }

            ///Despawn all player char dead bodies
            GameObject[] playerCharBodies = GameObject.FindGameObjectsWithTag("DeadPlayer");
            int playerCharBodiesArrLen = playerCharBodies.Length;

            for(int i = 0; i < playerCharBodiesArrLen; ++i) {
                playerCharBodies[i].SetActive(false);
            }

            reportCanvas.SetActive(true);
        }
    }
}