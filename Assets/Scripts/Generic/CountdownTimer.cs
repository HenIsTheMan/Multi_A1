using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Photon.Pun.UtilityScripts {
    internal sealed class CountdownTimer: MonoBehaviourPunCallbacks {
        private bool isTimerRunning;
        private int startTime;
        private const string CountdownStartTime = "";

        [SerializeField] private float countdownTime;

        [Header("TextComponent ")]
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
                this.textComponent.text = string.Format("Game starts in {0} seconds", countdown.ToString("n0"));

                if(countdown > 0.0f)
                    return;

                OnTimerEnds();
            }
        }

        private void OnTimerRuns() {
            isTimerRunning = true;
            enabled = true;
        }

        private void OnTimerEnds() {
            isTimerRunning = false;
            enabled = false;

            textComponent.text = string.Empty;

            if(OnCountdownTimerHasExpired != null) {
                OnCountdownTimerHasExpired();
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
            Init();
        }

        private void Init() {
            if(TryGetStartTime(out int propStartTime)) { //Inline var declaration
                startTime = propStartTime;
                Debug.Log("Init sets StartTime " + startTime + " server time now: " + PhotonNetwork.ServerTimestamp + " remain: " + TimeRemaining());

                isTimerRunning = TimeRemaining() > 0;
                if(isTimerRunning) {
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

                if(PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CountdownStartTime, out object startTimeFromProps)) { //Inline var declaration
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
                {CountdownStartTime, PhotonNetwork.ServerTimestamp}
            };
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);

            Debug.Log("Set Custom Props for Time: " + props.ToStringFull() + " wasSet: " + wasSet);
        }
    }
}