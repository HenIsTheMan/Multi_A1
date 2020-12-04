using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class DebugInfo: MonoBehaviour {
        #region Fields

        [SerializeField] private Text FPSText;
        [SerializeField] private Text ElapsedTimeText;
        [SerializeField] private Text DtText;
        [SerializeField] private Text FixedDtText;
        [SerializeField] private Text SmoothDtText;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public DebugInfo() {
            FPSText = null;
            ElapsedTimeText = null;
            DtText = null;
            FixedDtText = null;
            SmoothDtText = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            UnityEngine.Assertions.Assert.IsNotNull(FPSText);
            UnityEngine.Assertions.Assert.IsNotNull(ElapsedTimeText);
            UnityEngine.Assertions.Assert.IsNotNull(DtText);
            UnityEngine.Assertions.Assert.IsNotNull(FixedDtText);
            UnityEngine.Assertions.Assert.IsNotNull(SmoothDtText);
        }

        private void Start() {
            FixedDtText.text = "FixedDt: " + Time.fixedDeltaTime;
        }

        private void Update() {
            FPSText.text = "FPS: " + 1.0f / Time.smoothDeltaTime;
            ElapsedTimeText.text = "Elapsed Time: " + Time.realtimeSinceStartup;
            DtText.text = "Dt: " + Time.deltaTime;
            SmoothDtText.text = "SmoothDt: " + Time.smoothDeltaTime;
        }

        #endregion
    }
}