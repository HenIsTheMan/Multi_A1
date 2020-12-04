using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Impasta {
    internal sealed class DiffSpriteAnisWithSingleDelay: MonoBehaviour {
        #region Fields

        private float BT;
        private float elapsedTime;
        private int currFrameIndex;
        private SpriteRenderer spriteRenderer;

        [SerializeField] private float delay;
        [SerializeField] private int currSpriteAniIndex;

        ///Work around for Jagged arr to be serialized
        [Serializable] public class SpriteAni {
            public Sprite[] frames;
        }
        public SpriteAni[] spriteAnis;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public DiffSpriteAnisWithSingleDelay() {
            BT = 0.0f;
            elapsedTime = 0.0f;
            currFrameIndex = 0;
            spriteRenderer = null;

            delay = 0.0f;
            currSpriteAniIndex = 0;
            spriteAnis = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Start() {
            Assert.IsTrue(currFrameIndex >= 0);
            Assert.IsTrue(currSpriteAniIndex >= 0);
            Assert.IsTrue(currSpriteAniIndex <= spriteAnis.Length);
            Assert.IsTrue(currFrameIndex <= spriteAnis[currSpriteAniIndex].frames.Length);

            elapsedTime = BT - Time.deltaTime;
        }

        private void Update() {
            elapsedTime += Time.deltaTime;

            if(BT <= elapsedTime) {
                currFrameIndex = (currFrameIndex + 1) % spriteAnis[currSpriteAniIndex].frames.Length;
                spriteRenderer.sprite = spriteAnis[currSpriteAniIndex].frames[currFrameIndex];
                BT = elapsedTime + delay;
            }
        }

        #endregion

        public void ChangeAndResetSpriteAni(int spriteAniIndex) {
            Assert.IsTrue(spriteAniIndex <= spriteAnis.Length);
            currSpriteAniIndex = spriteAniIndex;

            ResetSpriteAni();
        }

        public void ResetSpriteAni() {
            currFrameIndex = 0;
            spriteRenderer.sprite = spriteAnis[currSpriteAniIndex].frames[currFrameIndex];
            BT = 0.0f;
            elapsedTime = BT - Time.deltaTime;
        }
    }
}