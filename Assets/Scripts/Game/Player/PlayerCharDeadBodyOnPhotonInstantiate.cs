using Photon.Pun;
using UnityEngine;

namespace Impasta.Game {
    internal sealed class PlayerCharDeadBodyOnPhotonInstantiate: MonoBehaviour, IPunInstantiateMagicCallback {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor
        #endregion

        #region Unity User Callback Event Funcs
        #endregion

        public void OnPhotonInstantiate(PhotonMessageInfo info) {
            GameObject playerCharGhost = (GameObject)info.Sender.TagObject;
            name = playerCharGhost.name;

            Transform spriteChildTransform = transform.GetChild(0);
			Color newColor = playerCharGhost.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
			newColor.a = 1.0f;
			spriteChildTransform.GetComponent<SpriteRenderer>().color = newColor;

			//* Setting ghost player char depth
			int index = info.Sender.ActorNumber - 1;

			Transform childTransform0 = transform.Find("PlayerCharOutfitSprite");
			Transform childTransform1 = transform.Find("PlayerCharSprite");
			Vector3 childPos0 = childTransform0.position;
			Vector3 childPos1 = childTransform1.position;

			childTransform0.position = new Vector3(
				childPos0.x,
				childPos0.y,
				(1.0f + (float)System.Convert.ToDouble(info.Sender == PhotonNetwork.LocalPlayer ? PhotonNetwork.CurrentRoom.MaxPlayers : index) * 2.0f) / 10.0f
			);
			childTransform1.position = new Vector3(
				childPos1.x,
				childPos1.y,
				(2.0f + (float)System.Convert.ToDouble(info.Sender == PhotonNetwork.LocalPlayer ? PhotonNetwork.CurrentRoom.MaxPlayers : index) * 2.0f) / 10.0f
			);
			//*/
		}
    }
}