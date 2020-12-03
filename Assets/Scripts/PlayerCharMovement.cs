using UnityEngine;

namespace Impasta.Game {
	internal sealed class PlayerCharMovement: MonoBehaviour {
		#region Fields

		private bool canMove;
		private float horizAxis;
		private float vertAxis;
		private DiffSpriteAnisWithSingleDelay script0;
		private DiffSpriteAnisWithSingleDelay script1;
		private Rigidbody rigidbodyComponent;

		[SerializeField] private bool isFacingRight;
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
			canMove = true;
			horizAxis = 0.0f;
			vertAxis = 0.0f;
			script0 = null;
			script1 = null;
			rigidbodyComponent = null;

			isFacingRight = true;
			spd = 0.0f;
		}

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() {
			Transform childTransform = gameObject.transform.Find("PlayerCharOutfitSprite");
			UnityEngine.Assertions.Assert.IsNotNull(childTransform);
			script0 = childTransform.GetComponent<DiffSpriteAnisWithSingleDelay>();

			Transform grandchildTransform = childTransform.Find("PlayerCharSprite");
			UnityEngine.Assertions.Assert.IsNotNull(grandchildTransform);
			script1 = grandchildTransform.GetComponent<DiffSpriteAnisWithSingleDelay>();

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
			if(result0 && result1) {
				script0.ResetSpriteAni();
				script0.enabled = false;
				script1.ResetSpriteAni();
				script1.enabled = false;
				return;
			} else {
				script0.enabled = true;
				script1.enabled = true;
			}

			if(!result0) {
				bool isFacingRightPrev = isFacingRight;
				isFacingRight = horizAxis > 0.0f;

				if(isFacingRight != isFacingRightPrev) { //Change moving dir
					script0.ChangeAndResetSpriteAni(System.Convert.ToInt32(isFacingRight));
					script1.ChangeAndResetSpriteAni(System.Convert.ToInt32(isFacingRight));
				}
			}
		}

		private void FixedUpdate() {
			if(!canMove) {
				return;
			}

			rigidbodyComponent.velocity = new Vector3(horizAxis, vertAxis, 0.0f).normalized * spd * Time.fixedDeltaTime;
		}

		private void OnCollisionEnter(Collision otherCollision) {
			if(otherCollision.gameObject.CompareTag("Player")) {
				otherCollision.rigidbody.isKinematic = true; //Can freeze pos instead for diff effect
			}
		}

		private void OnCollisionExit(Collision otherCollision) {
			if(otherCollision.gameObject.CompareTag("Player")) {
				otherCollision.rigidbody.isKinematic = false; //Can unfreeze pos instead for diff effect
			}
		}

		#endregion
	}
}