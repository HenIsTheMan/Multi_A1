using Hashtable = ExitGames.Client.Photon.Hashtable;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

namespace Impasta.Game{
    internal sealed class GameManager: MonoBehaviourPunCallbacks { //Singleton
        #region Fields

        public static GameManager globalInstance;

        [SerializeField] private Text infoText;

        ///Work around for Jagged arr to be serialized
        [System.Serializable]
        public class Prefab {
            public GameObject prefab;
        }
        public Prefab[] prefabs;

        #endregion

        #region Properties
        #endregion

        #region Ctors and Dtor

        private GameManager() {
            globalInstance = null;

            infoText = null;

            prefabs = null;
        }

        #endregion

        #region Unity User Callback Event Funcs

        private void Awake() {
            globalInstance = this;
        }

        public override void OnEnable() {
            base.OnEnable();
            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerExpired;
        }

        private void Start() {
            Hashtable props = new Hashtable {
            {"PlayerLoadedLevel", true}
        };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            ///Manually fill ResourceCache of DefaultPool
            if(PhotonNetwork.PrefabPool is DefaultPool pool && prefabs != null) {
                foreach(Prefab myPrefab in prefabs) {
                    GameObject prefab = myPrefab.prefab;
                    pool.ResourceCache.Add(prefab.name, prefab);
                }
            }
        }

        public override void OnDisable() {
            base.OnDisable();
            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerExpired;
        }

        #endregion

        #region Pun Callback Funcs

        public override void OnDisconnected(DisconnectCause cause) {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public override void OnLeftRoom() {
            PhotonNetwork.Disconnect();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) {
            if(changedProps.ContainsKey("PlayerLives")) {
                return;
            }
            if(!PhotonNetwork.IsMasterClient) {
                return;
            }

            int startTimestamp;
            bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);
            if(changedProps.ContainsKey("PlayerLoadedLevel")) {
                if(LevelLoadedForAllPlayers()) {
                    if(!startTimeIsSet) {
                        CountdownTimer.SetStartTime(); //Set timer start time when everyone has loaded the level
                    }
                } else {
                    Debug.Log("setting text waiting for players! ", infoText);
                    infoText.text = "Waiting for other players...";
                }
            }

        }

        #endregion

        private void OnCountdownTimerExpired() {
            StartGame();
        }

        private void StartGame() {
            GameObject playerChar = PhotonNetwork.Instantiate(
                "PlayerChar",
                new Vector3(0.0f, 0.0f, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, 0.0f),
                0
            ); //Avoid this call on rejoin (JL was network-instantiated before)??

            PlayerCharKill playerCharKill = playerChar.GetComponent<PlayerCharKill>();
            playerCharKill.IsImposter = true;

            PlayerCharMovement playerCharMovement = playerChar.GetComponent<PlayerCharMovement>();
            playerCharMovement.CanMove = true;

            PhotonNetwork.Instantiate(
                "PlayerChar",
                new Vector3(0.0f, 2.0f, 0.0f),
                Quaternion.Euler(0.0f, 0.0f, 0.0f),
                0
            );

            if(PhotonNetwork.IsMasterClient) {
                //SpawnGhosts();
            }
        }

        private bool LevelLoadedForAllPlayers() {
            foreach(Player p in PhotonNetwork.PlayerList) {
                object playerLoadedLevel;

                if(p.CustomProperties.TryGetValue("PlayerLoadedLevel", out playerLoadedLevel)) {
                    if((bool)playerLoadedLevel) {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }
    }
}