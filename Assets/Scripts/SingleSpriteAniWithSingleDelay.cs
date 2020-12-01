using UnityEngine;
using UnityEngine.Assertions;
using System;

namespace Impasta.Game {
    internal sealed class SingleSpriteAniWithSingleDelay: MonoBehaviour {
        #region Fields

        private float BT;
        private float elapsedTime;
        private int currFrameIndex;
        private SpriteRenderer spriteRenderer;

        [SerializeField] private float delay;
        [SerializeField] private Sprite[] frames;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public SingleSpriteAniWithSingleDelay() {
            BT = 0.0f;
            elapsedTime = 0.0f;
            currFrameIndex = 0;
            spriteRenderer = null;

            delay = 0.0f;
            frames = Array.Empty<Sprite>();
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Start() {
            Assert.IsTrue(currFrameIndex >= 0);
        }

        private void Update() {
            elapsedTime += Time.deltaTime;

            if(BT <= elapsedTime) {
                currFrameIndex = (currFrameIndex + 1) % frames.Length;
                spriteRenderer.sprite = frames[currFrameIndex];
                BT = elapsedTime + delay;
            }
        }

        #endregion

        public void ResetSpriteAni() {
            currFrameIndex = 0;
            BT = elapsedTime = 0.0f;
        }
    }
}