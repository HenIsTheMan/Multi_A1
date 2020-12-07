using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class MsgSentByGhostEventHandler: MonoBehaviour, IOnEventCallback {
        #region Fields

        [SerializeField] private GameObject msgListItemPrefab;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private MsgSentByGhostEventHandler() {
            msgListItemPrefab = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void OnEnable() {
            PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
        }

        private void OnDisable() {
            PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
        }

        #endregion

        public void OnEvent(EventData photonEvent) {
            if(photonEvent.Code == (byte)EventCodes.EventCode.MsgSentByGhostEvent && ((GameObject)PhotonNetwork.LocalPlayer.TagObject).GetComponent<PlayerCharKill>().IsDead) {
                GameObject ghostMsgListItem = Instantiate(msgListItemPrefab, GameObject.Find("Content").transform);
                object[] data = (object[])photonEvent.CustomData;

                Text textComponent = ghostMsgListItem.transform.Find("Text").GetComponent<Text>();
				textComponent.text = (string)data[0] + ": " + MsgListItemOnPhotonInstantiate.Msg;

                Vector3 colorVec3 = (Vector3)data[1];
                textComponent.color = new Color(colorVec3.x, colorVec3.y, colorVec3.z, 0.7f);

                MsgListItemOnPhotonInstantiate.Msg = string.Empty; //So next msg list item will only be instantiated when msg is set again
			}
        }
    }
}