using Hashtable = ExitGames.Client.Photon.Hashtable;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Impasta.Game{
    internal sealed class GameManager: MonoBehaviourPunCallbacks { //Singleton
        public enum EventCode: byte {
            NotAnEvent,
            PlayerCharsCreatedEvent,
            Amt
        };


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
            ///Avoid on rejoin (JL was network-instantiated before)??

            //* Spawn local instances of player char for each client
            Transform parentTransform = GameObject.Find("SceneTest").transform;
            int arrLen = PhotonNetwork.PlayerList.Length;
            Dictionary<int, string> playerCharNames = new Dictionary<int, string>();

            for(int i = 0; i < arrLen; ++i) {
                GameObject playerChar = PhotonNetwork.Instantiate(
                    "PlayerChar",
                    Vector3.zero,
                    Quaternion.identity,
                    0
                );
                playerChar.transform.SetParent(parentTransform, true);

                if(PhotonNetwork.IsMasterClient) {
                    Vector3 pos = Vector3.zero;
                    switch(i) {
                        case 0:
                            pos = new Vector3(0.0f, 3.0f, 0.0f);
                            break;
                        case 1:
                            pos = new Vector3(2.0f, 2.0f, 0.0f);
                            break;
                        case 2:
                            pos = new Vector3(4.0f, 1.0f, 0.0f);
                            break;
                    }
                    playerChar.transform.position = pos;
                }

                playerChar.name = "PlayerChar_" + i;
                playerCharNames.Add(PhotonNetwork.PlayerList[i].ActorNumber, playerChar.name);
            }
            //*/

            //* Assign local PlayerCam instance to correct local instance of player char for each client
            if(PhotonNetwork.IsMasterClient) {
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                    Receivers = ReceiverGroup.All
                }; //Will receive event on local client too
                PhotonNetwork.RaiseEvent((byte)EventCode.PlayerCharsCreatedEvent, playerCharNames, raiseEventOptions, ExitGames.Client.Photon.SendOptions.SendReliable);
            }
            //*/
        }

        private bool LevelLoadedForAllPlayers() {
            foreach(var p in PhotonNetwork.PlayerList) {
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

        public static void SpawnDeadBody(in Vector3 pos) {
            GameObject ghost = PhotonNetwork.Instantiate(
                "PlayerChar",
                pos,
                Quaternion.Euler(0.0f, 0.0f, 90.0f),
                0
            );

            CapsuleCollider ghostCapsuleCollider = ghost.GetComponent<CapsuleCollider>();
            ghostCapsuleCollider.enabled = false;
        }
    }
}