using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace Impasta {
    internal sealed class SingleSpriteAniWithDiffDelays: MonoBehaviour {
        #region Fields

        private float BT;
        private float elapsedTime;
        private int currFrameIndex;
        private SpriteRenderer spriteRenderer;

        [SerializeField] private float[] delays;
        [SerializeField] private Sprite[] frames;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public SingleSpriteAniWithDiffDelays() {
            BT = 0.0f;
            elapsedTime = 0.0f;
            currFrameIndex = 0;
            spriteRenderer = null;

            delays = Array.Empty<float>();
            frames = Array.Empty<Sprite>();
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Start() {
            Assert.IsTrue(currFrameIndex >= 0);
            Assert.AreEqual(delays.Length, frames.Length);
        }

        private void Update() {
            elapsedTime += Time.deltaTime; 

            if(BT <= elapsedTime) {
                currFrameIndex = (currFrameIndex + 1) % frames.Length;
                spriteRenderer.sprite = frames[currFrameIndex];
                BT = elapsedTime + delays[currFrameIndex];
            }
        }

        #endregion

        public void ResetSpriteAni() {
            currFrameIndex = 0;
            BT = elapsedTime = 0.0f;
        }
    }
}