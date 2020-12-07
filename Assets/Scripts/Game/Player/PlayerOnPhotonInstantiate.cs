using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class PlayerOnPhotonInstantiate: MonoBehaviour, IPunInstantiateMagicCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private PlayerOnPhotonInstantiate() {
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            _ = StartCoroutine(InitPlayerAttribs(info));
        }

        private System.Collections.IEnumerator InitPlayerAttribs(PhotonMessageInfo info) { //In case
            while(PlayerUniversal.Roles.Length == 0) {
                yield return null;
            }

            int index = info.Sender.ActorNumber - 1;
            info.Sender.TagObject = gameObject;
            name = "PlayerChar" + index;

            GameObject voteButton = GameObject.Find("VoteButton" + index);
            voteButton.GetComponent<RawImage>().color = PlayerUniversal.Colors[index];

            //* Setting player char depth
            Transform spriteChildTransform0 = gameObject.transform.GetChild(0);
            Transform spriteChildTransform1 = gameObject.transform.GetChild(1);

            spriteChildTransform0.position = new Vector3(
                spriteChildTransform0.position.x,
                spriteChildTransform0.position.y,
                -(2.0f + (float)System.Convert.ToDouble(info.Sender == PhotonNetwork.LocalPlayer ? PhotonNetwork.CurrentRoom.MaxPlayers : index) * 2.0f) / 10.0f
            );

            spriteChildTransform1.position = new Vector3(
                spriteChildTransform1.position.x,
                spriteChildTransform1.position.y,
                -(1.0f + (float)System.Convert.ToDouble(info.Sender == PhotonNetwork.LocalPlayer ? PhotonNetwork.CurrentRoom.MaxPlayers : index) * 2.0f) / 10.0f
            );
            //*/

            bool isLocalClientImposter = PlayerUniversal.Roles[PhotonNetwork.LocalPlayer.ActorNumber - 1];
            bool isImposter = PlayerUniversal.Roles[index];

            gameObject.GetComponent<PlayerCharKill>().IsImposter = isImposter;

            Transform canvasChildTransform = gameObject.transform.Find("PlayerNameCanvas");
            Transform grandchildTransform = canvasChildTransform.Find("PlayerNameText");

            UnityEngine.UI.Text textComponent = grandchildTransform.GetComponent<UnityEngine.UI.Text>();
            textComponent.text = info.Sender.NickName;
            if(isLocalClientImposter && isImposter) {
                textComponent.color = Color.red;
            }

            //* Assigning tasks
            PlayerCharTasks playerCharTasks = gameObject.GetComponent<PlayerCharTasks>();

            if(!isLocalClientImposter && info.Sender == PhotonNetwork.LocalPlayer) {
                playerCharTasks.TotalAmtOfTasks = Random.Range(5, 10);

                GameObject[] taskBlocks = GameObject.FindGameObjectsWithTag("TaskBlock");
                ShuffleElements.Shuffle(taskBlocks);
                byte arrLen = (byte)taskBlocks.Length;

                for(byte i = 0; i < arrLen; ++i) {
                    taskBlocks[i].GetComponent<TaskBlock>().MyTaskStatus = i < (byte)playerCharTasks.TotalAmtOfTasks
                        ? TaskStatuses.TaskStatus.NotDone
                        : TaskStatuses.TaskStatus.None;
                }

                object[] data = new object[]{
                    name,
                    playerCharTasks.TotalAmtOfTasks
                };
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                    Receivers = ReceiverGroup.Others
                };
                PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode.SetPlayerTotalAmtOfTasksEvent,
                    data, raiseEventOptions, ExitGames.Client.Photon.SendOptions.SendReliable);
            }
            //*/

            float angle = (360.0f / (float)System.Convert.ToDouble(PhotonNetwork.CurrentRoom.PlayerCount)) * Mathf.Deg2Rad * (float)System.Convert.ToDouble(index);
            float radius = 3.0f;
            transform.position = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0.0f) * radius;

            yield return null;
        }
    }
}