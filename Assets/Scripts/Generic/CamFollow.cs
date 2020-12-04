using UnityEngine;

namespace Impasta {
    internal sealed class CamFollow: MonoBehaviour {
        #region Fields

        private Vector3 offset;
        private Transform targetTransform;

        [SerializeField] private float time;

        #endregion

        #region Properties

        public Transform TargetTransform {
            get {
                return targetTransform;
            }
            set {
                targetTransform = value;
                offset = targetTransform.position - transform.position;
            }
        }

        #endregion

        #region Ctors and Dtor

        public CamFollow() {
            offset = Vector3.zero;
            targetTransform = null;

            time = 0.0f;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void FixedUpdate() {
            if(targetTransform != null) {
                transform.position = Vector3.Lerp(transform.position, targetTransform.position - offset, Time.fixedDeltaTime * time);
            }
        }

        #endregion
    }
}