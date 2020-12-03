using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Impasta{
    internal sealed class PlayerColors: MonoBehaviour, IOnEventCallback {
        #region Fields

        private static Color[] colors;

        #endregion

        #region Properties

        public static Color[] Colors {
            get {
                return colors;
            }
            private set {
            }
        }

        #endregion

        #region Ctors and Dtor

        static PlayerColors() {
            colors = System.Array.Empty<Color>();
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void OnEnable() {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable() {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        #endregion

        public void OnEvent(EventData photonEvent) {
            EventCodes.EventCode eventCode = (EventCodes.EventCode)photonEvent.Code;
            switch(eventCode) {
                case EventCodes.EventCode.InitColorsEvent:
                    colors = (Color[])photonEvent.CustomData;
                    break;
            }
        }

        public static bool InitColors() {
            if(colors.Length == 0) {
                colors = new Color[]{
                    Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                    Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                    Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                    Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                    Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                    Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                    Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                    Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                    Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true),
                    Color.HSVToRGB(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f, true)
                };
                return true;
            } else {
                return false;
            }
        }

        public static Color GetPlayerColor(int index) {
            if(index >= 0 && index < colors.Length) {
                return colors[index];
            } else {
                Debug.LogWarning("<color=yellow>Var 'index' is out of range!</color>");
                return Color.black;
            }
        }
    }
}