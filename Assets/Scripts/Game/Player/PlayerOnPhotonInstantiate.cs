using Photon.Pun;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class PlayerOnPhotonInstantiate: MonoBehaviour, IPunInstantiateMagicCallback {
        #region Fields

        private static int logicID;

        #endregion

        #region Properties

        public static int LogicID {
            get {
                return logicID;
            }
            set {
                logicID = value;
            }
        }

        #endregion

        #region Ctors and Dtor

        private PlayerOnPhotonInstantiate() {
        }

        static PlayerOnPhotonInstantiate() {
            logicID = -1;
        }

        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            switch(logicID) {
                case 0:
                    _ = StartCoroutine(InitPlayerAttribs(info));
                    break;
                case 1:
                    //humanBody.name = currClosestTargetPlayerCharKill.name;

                    //Transform childTransform = humanBody.transform.Find("PlayerCharOutfitSprite");
                    //SpriteRenderer childSpriteRenderer = childTransform.GetComponent<SpriteRenderer>();
                    //childSpriteRenderer.color -= humanPlayerChar.transform.Find("PlayerCharOutfitSprite").GetComponent<SpriteRenderer>().color;
                    break;
                default:
                    UnityEngine.Assertions.Assert.IsTrue(false);
                    break;
            }
        }

        private System.Collections.IEnumerator InitPlayerAttribs(PhotonMessageInfo info) { //In case
            while(PlayerUniversal.Roles.Length == 0) {
                yield return null;
            }

            int index = info.Sender.ActorNumber - 1;

            name = "PlayerChar" + index;

            //* Setting player char depth
            float offset = info.Sender == PhotonNetwork.LocalPlayer
                ? (float)System.Convert.ToDouble(PhotonNetwork.CurrentRoom.MaxPlayers) / 10.0f
                : (float)System.Convert.ToDouble(index) / 10.0f;

            Transform spriteChildTransform0 = gameObject.transform.GetChild(0);
            spriteChildTransform0.position = new Vector3(
                spriteChildTransform0.position.x,
                spriteChildTransform0.position.y,
                spriteChildTransform0.position.z - offset

            );

            Transform spriteChildTransform1 = gameObject.transform.GetChild(1);
            spriteChildTransform1.position = new Vector3(
                spriteChildTransform1.position.x,
                spriteChildTransform1.position.y,
                spriteChildTransform1.position.z - offset
            );
            //*/

            bool isLocalClientImposter = PlayerUniversal.Roles[PhotonNetwork.LocalPlayer.ActorNumber - 1];
            bool isImposter = PlayerUniversal.Roles[index];

            gameObject.GetComponent<PlayerCharKill>().IsImposter = isImposter;

            Transform canvasChildTransform = gameObject.transform.Find("PlayerNameCanvas");
            Transform grandchildTransform = canvasChildTransform.Find("PlayerNameText");

            UnityEngine.UI.Text textComponent = grandchildTransform.GetComponent<UnityEngine.UI.Text>();
            textComponent.text = name + ' ' + info.Sender.NickName;
            if(isLocalClientImposter && isImposter) {
                textComponent.color = Color.red;
            }

            float angle = (360.0f / (float)System.Convert.ToDouble(PhotonNetwork.CurrentRoom.PlayerCount)) * Mathf.Deg2Rad * (float)System.Convert.ToDouble(index);
            float radius = 3.0f;
            gameObject.transform.position = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0.0f) * radius;

            yield return null;
        }
    }
}