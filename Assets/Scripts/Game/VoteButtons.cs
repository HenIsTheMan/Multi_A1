﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Impasta.Game {
    internal sealed class VoteButtons: MonoBehaviour {
        #region Fields

        private int arrLen;
        [SerializeField] private GameObject[] voteButtons;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private VoteButtons() {
            arrLen = 0;
            voteButtons = Array.Empty<GameObject>();
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Start(){
            arrLen = voteButtons.Length;
        }

        private void Update() {
            for(int i = 0; i < arrLen; ++i) {
                if(PlayerUniversal.Votes.Length > i) {
                    voteButtons[i].transform.Find("Text").GetComponent<Text>().text = PlayerUniversal.Votes[i].ToString();
                }
            }
        }

        #endregion
    }
}