using UnityEngine;
using System;

namespace Impasta.Game {
    internal sealed class SpriteAni: MonoBehaviour {
        #region Fields

        private float BT;
        private int currFrameIndex;
        private SpriteRenderer spriteRenderer;

        [SerializeField] private float delay;
        [SerializeField] private float elapsedTime;
        [SerializeField] private Sprite[] frames;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        public SpriteAni() {
            frames = Array.Empty<Sprite>();
            currFrameIndex = 0;
            BT = 0.0f;
            delay = 0.0f;
            elapsedTime = 0.0f;

            spriteRenderer = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Start() {
        }

        private void Update() {
            elapsedTime += Time.deltaTime;

            if(BT <= elapsedTime) {
                spriteRenderer.sprite = frames[++currFrameIndex % frames.Length];
                BT = elapsedTime + delay;
            }
        }

        #endregion
    }
}