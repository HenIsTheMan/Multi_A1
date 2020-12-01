using UnityEngine;

namespace Impasta.Game {
	internal sealed class PlayerCharMovement: MonoBehaviour {
		#region Fields

		private bool isKillButtonPressed;
		private float horizAxis;
		private float vertAxis;
		private DiffSpriteAnisWithSingleDelay script0;
		private DiffSpriteAnisWithSingleDelay script1;
		private Rigidbody rigidbodyComponent;

		[SerializeField] private bool isFacingRight;
		[SerializeField] private float spd;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		public PlayerCharMovement() {
			isKillButtonPressed = false;
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
			if(Input.GetButtonDown("Kill")) {
				isKillButtonPressed = true;
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
			rigidbodyComponent.velocity = new Vector3(horizAxis, vertAxis, 0.0f).normalized * spd * Time.fixedDeltaTime;

			if(isKillButtonPressed) {
				//Kill logic??

				isKillButtonPressed = false;
			}
		}

		#endregion
	}
}