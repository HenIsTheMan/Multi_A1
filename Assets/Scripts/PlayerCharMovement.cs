using UnityEngine;

namespace Impasta.Game {
	internal sealed class PlayerCharMovement: MonoBehaviour {
		#region Fields

		private bool isKillButtonPressed;
		private float horizAxis;
		private float vertAxis;
		private Rigidbody rigidbodyComponent;

		[SerializeField] private float spd;

		#endregion

		#region Properties
		#endregion

		#region Ctors and Dtor

		public PlayerCharMovement() {
			isKillButtonPressed = false;
			horizAxis = 0.0f;
			vertAxis = 0.0f;
			rigidbodyComponent = null;

			spd = 0.0f;
		}

		#endregion

		#region Unity User Callback Event Funcs

		private void Awake() {
			rigidbodyComponent = GetComponent<Rigidbody>();
		}

		private void Update() {
			if(Input.GetButtonDown("Kill")) {
				isKillButtonPressed = true;
			}
			horizAxis = Input.GetAxisRaw("Horizontal");
			vertAxis = Input.GetAxisRaw("Vertical");
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