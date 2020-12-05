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

            Transform spriteChildTransform = gameObject.transform.GetChild(0);
            spriteChildTransform.GetComponent<SpriteRenderer>().color = playerCharGhost.transform.GetChild(0).GetComponent<SpriteRenderer>().color;

			//* Setting ghost player char depth
			float offset = (float)System.Convert.ToDouble(info.Sender.ActorNumber - 1) / 10.0f;

			Transform childTransform0 = transform.Find("PlayerCharOutfitSprite");
			Transform childTransform1 = transform.Find("PlayerCharSprite");
			Vector3 childPos0 = childTransform0.position;
			Vector3 childPos1 = childTransform1.position;

			childTransform0.position = new Vector3(
				childPos0.x,
				childPos0.y,
				childPos0.z + offset
			);
			childTransform1.position = new Vector3(
				childPos1.x,
				childPos1.y,
				childPos1.z + offset
			);
			//*/
		}
    }
}