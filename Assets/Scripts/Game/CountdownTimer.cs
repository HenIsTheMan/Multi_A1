using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Impasta.Game;
using System;

namespace Photon.Pun.UtilityScripts {
    internal sealed class CountdownTimer: MonoBehaviourPunCallbacks {
        private bool isTimerRunning;
        private int startTime;
        private string mainText = "";
        private const string countdownStartTime = "";

        [SerializeField] private float countdownTime;
        [SerializeField] private Text textComponent;

        public delegate void CountdownTimerHasExpired();

        public static event CountdownTimerHasExpired OnCountdownTimerHasExpired;

        public override void OnEnable() {
            base.OnEnable();
            Init();
        }

        public override void OnDisable() {
            base.OnDisable();
        }

        private void Update() {
            if(isTimerRunning) {
                float countdown = TimeRemaining();
                textComponent.text = mainText + countdown.ToString("n0");

                if(countdown > 0.0f) {
                    return;
                }
                OnTimerEnds();
            }
        }

        private void OnTimerRuns() {
            ///Create player roles
            if(PhotonNetwork.IsMasterClient) {
                PlayerUniversal.GenRoles();
            } else {
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                    Receivers = ReceiverGroup.MasterClient
                };
                PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode.RetrievePlayerRolesEvent,
                    null, raiseEventOptions, SendOptions.SendReliable);
            }

            _ = StartCoroutine(nameof(InitMainText));
        }

        private System.Collections.IEnumerator InitMainText() {
            while(PlayerUniversal.Roles.Length == 0) {
                yield return null;
            }

            isTimerRunning = true;
            enabled = true;

            bool isLocalClientImposter = PlayerUniversal.Roles[PhotonNetwork.LocalPlayer.ActorNumber - 1];
            int arrLen = PlayerUniversal.Roles.Length;

            if(isLocalClientImposter) {
                int humanCount = 0;

                for(int i = 0; i < arrLen; ++i) {
                    humanCount += Convert.ToInt32(!PlayerUniversal.Roles[i]);
                }

                if(humanCount == 1) {
                    mainText = "There is 1 human to die... ";
                } else {
                    mainText = "There are " + humanCount + " humans to die... ";
                }
                textComponent.color = Color.red;
            } else {
                int impastaCount = 0;

                for(int i = 0; i < arrLen; ++i) {
                    impastaCount += Convert.ToInt32(PlayerUniversal.Roles[i]);
                }

                if(impastaCount == 1) {
                    mainText = "There is 1 impasta in our midst... ";
                } else {
                    mainText = "There are " + impastaCount + " impastas in our midst... ";
                }
            }

            yield return null;
        }

        private void OnTimerEnds() {
            isTimerRunning = false;
            enabled = false;

            textComponent.text = string.Empty;

			OnCountdownTimerHasExpired?.Invoke(); //Delegate invocation
		}

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
            Init();
        }

        private void Init() {
            if(TryGetStartTime(out int propStartTime)) { //Inline var declaration
                startTime = propStartTime;
                Debug.Log("Init sets StartTime " + startTime + " server time now: " + PhotonNetwork.ServerTimestamp + " remain: " + TimeRemaining());

                if(TimeRemaining() > 0) {
                    OnTimerRuns();
                } else {
                    OnTimerEnds();
                }
            }
        }

        private float TimeRemaining() {
            int timer = PhotonNetwork.ServerTimestamp - startTime;
            return countdownTime - timer / 1000.0f;
        }

        public static bool TryGetStartTime(out int startTimestamp) {
            if(PhotonNetwork.CurrentRoom != null) {
                startTimestamp = PhotonNetwork.ServerTimestamp;

                if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(countdownStartTime, out object startTimeFromProps)) { //Inline var declaration
                    startTimestamp = (int)startTimeFromProps;
                    return true;
                }
                return false;
            } else {
                Debug.LogWarning("<color=yellow>PhotonNetwork.CurrentRoom is null!</color>");
                startTimestamp = 0;
                return true;
            }
        }

        public static void SetStartTime() {
            bool wasSet = TryGetStartTime(out _);

            Hashtable props = new Hashtable {
                {countdownStartTime, PhotonNetwork.ServerTimestamp}
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);

            Debug.Log("Set Custom Props for Time: " + props.ToStringFull() + " wasSet: " + wasSet);
        }
    }
}