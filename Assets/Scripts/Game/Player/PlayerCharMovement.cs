using Photon.Pun;
using UnityEngine;

namespace Impasta.Game {
	internal sealed class PlayerCharMovement: MonoBehaviour {
		#region Fields

		private bool canMove;
		private float horizAxis;
		private float vertAxis;
		private Rigidbody rigidbodyComponent;

		[SerializeField] private bool isFacingRight;
		[SerializeField] private DiffSpriteAnisWithSingleDelay script0;
		[SerializeField] private DiffSpriteAnisWithSingleDelay script1;
		[SerializeField] private float spd;

		#endregion

		#region Properties

		public bool CanMove {
			get {
				return canMove;
			}
			set {
				canMove = value;
			}
		}

		#endregion

		#region Ctors and Dtor

		public PlayerCharMovement() {
			canMove = false;
			horizAxis = 0.0f;
			vertAxis = 0.0f;
			rigidbodyComponent = null;

			isFacingRight = true;
			script0 = null;
			script1 = null;
			spd = 0.0f;
		}

		public Rigidbody RigidbodyComponent {
			get {
				return rigidbodyComponent;
			}
			private set {
				rigidbodyComponent = value;
			}
		}

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() {
			rigidbodyComponent = GetComponent<Rigidbody>();
		}

		private void Update() {
			if(!canMove){
				return;
			}

			horizAxis = Input.GetAxisRaw("Horizontal");
			vertAxis = Input.GetAxisRaw("Vertical");

			bool result0 = Mathf.Approximately(horizAxis, 0.0f);
			bool result1 = Mathf.Approximately(vertAxis, 0.0f);

			PhotonView.Get(this).RPC("UpdatePlayerSpriteAni", RpcTarget.All, name, result0, result1, horizAxis < 0.0f);
		}

		public void UpdateSpriteAni(bool result0, bool result1, bool isFacingLeft) {
			if(result0 && result1) {
				DisableSpriteAni();
				return;
			} else {
				script0.enabled = true;
				script1.enabled = true;
			}

			if(!result0) {
				bool isFacingRightPrev = isFacingRight;
				isFacingRight = !isFacingLeft;

				if(isFacingRight != isFacingRightPrev) { //Change moving dir
					script0.ChangeAndResetSpriteAni(System.Convert.ToInt32(isFacingRight));
					script1.ChangeAndResetSpriteAni(System.Convert.ToInt32(isFacingRight));
				}
			}
		}

		public void DisableSpriteAni() {
			script0.ResetSpriteAni();
			script0.enabled = false;
			script1.ResetSpriteAni();
			script1.enabled = false;
		}

		private void FixedUpdate() {
			if(!canMove) {
				return;
			}

			rigidbodyComponent.velocity = new Vector3(horizAxis, vertAxis, 0.0f).normalized * spd * Time.fixedDeltaTime;
		}

		#endregion
	}
}