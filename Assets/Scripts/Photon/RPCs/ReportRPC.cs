using Impasta.Game;
using Photon.Pun;
using Photon.Realtime;
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
            reportCanvas = GameObject.Find("ReportCanvasWrapper").transform.GetChild(0).gameObject;
        }

        #endregion

        [PunRPC] public void Report() {
            //* Return back to spawn pos
            int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            float angle = (360.0f / (float)System.Convert.ToDouble(PhotonNetwork.CurrentRoom.PlayerCount)) * Mathf.Deg2Rad * (float)System.Convert.ToDouble(index);
            float radius = 3.0f;

            ((GameObject)PhotonNetwork.LocalPlayer.TagObject).transform.position = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0.0f) * radius;
            //*/

            //* Make all players static during a report
            PlayerCharMovement playerCharMovement = ((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerCharMovement>();
            playerCharMovement.CanMove = false;
            playerCharMovement.RigidbodyComponent.velocity = Vector3.zero;

            GameObject[] playerChars = GameObject.FindGameObjectsWithTag("Player");
            int playerCharsArrLen = playerChars.Length;

            for(int i = 0; i < playerCharsArrLen; ++i) {
                if(PhotonNetwork.IsMasterClient) {
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                        Receivers = ReceiverGroup.All
                    };
                    PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode.DisablePlayerSpriteAniEvent,
                        playerChars[i].name, raiseEventOptions, ExitGames.Client.Photon.SendOptions.SendReliable);
                }
            }
            //*/

            ///Despawn all player char dead bodies
            GameObject[] playerCharBodies = GameObject.FindGameObjectsWithTag("DeadPlayer");
            int playerCharBodiesArrLen = playerCharBodies.Length;

            for(int i = 0; i < playerCharBodiesArrLen; ++i) {
                playerCharBodies[i].SetActive(false);
            }

            reportCanvas.SetActive(true);
            ((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerCharReport>().VoteStart();
        }
    }
}