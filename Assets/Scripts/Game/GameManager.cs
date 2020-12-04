using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Impasta.Game{
    internal sealed class GameManager: MonoBehaviourPunCallbacks { //Singleton
        #region Fields

        public static GameManager globalInstance;

        [SerializeField] private Material otherSideMask;
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

            otherSideMask = null;
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
                    //Debug.Log("Setting text waiting for players! ", infoText);
                    infoText.text = "Waiting for other players...";
                }
            }

        }

        #endregion

        private int Bit(int x) {
            return 1 << x;
        }

        private void OnCountdownTimerExpired() {
            Camera playerCharCam = GameObject.Find("PlayerCharCam").GetComponent<Camera>();
            playerCharCam.cullingMask |= Bit(LayerMask.NameToLayer("Default"));
            playerCharCam.cullingMask &= ~Bit(LayerMask.NameToLayer("OtherSide"));

            StartGame();
        }

        private void StartGame() {
            ///Avoid on rejoin (JL was network-instantiated before)??

            ///Create roles
            if(PhotonNetwork.IsMasterClient) {
                PlayerUniversal.GenRoles();
            } else {
                RaiseEventOptions raiseEventOptions = new RaiseEventOptions {
                    Receivers = ReceiverGroup.MasterClient
                };
                PhotonNetwork.RaiseEvent((byte)EventCodes.EventCode.RetrievePlayerRolesEvent,
                    null, raiseEventOptions, ExitGames.Client.Photon.SendOptions.SendReliable);
            }

            Transform parentTransform = GameObject.Find("SceneTest").transform;
            GameObject playerChar = PhotonNetwork.Instantiate(
               "PlayerChar",
               Vector3.zero,
               Quaternion.identity,
               0
            );
            playerChar.transform.SetParent(parentTransform, true);

            GameObject playerCharCam = GameObject.Find("PlayerCharCam");
            playerCharCam.transform.position = new Vector3(playerChar.transform.position.x, playerChar.transform.position.y, gameObject.transform.position.z);
            playerCharCam.GetComponent<CamFollow>().TargetTransform = playerChar.transform;

            PlayerCharMovement playerCharMovement = playerChar.GetComponent<PlayerCharMovement>();
            playerCharMovement.CanMove = true;

            GameObject sceneLightMask = GameObject.Find("LightMask");
            sceneLightMask.GetComponent<MeshRenderer>().materials = new Material[1]{
                otherSideMask
            };
            LightCaster playerCharLightCaster = playerChar.GetComponent<LightCaster>();
            playerCharLightCaster.LightMask = sceneLightMask;

            /////Call after all clients have arr of roles??
            //_ = StartCoroutine(nameof(SpawnPlayerCharCoroutine));
        }

        //private System.Collections.IEnumerator SpawnPlayerCharCoroutine() {
        //    while(PlayerUniversal.Roles.Length == 0) {
        //        yield return null;
        //    }

        //    Transform parentTransform = GameObject.Find("SceneTest").transform;
        //    GameObject playerChar = PhotonNetwork.Instantiate(
        //       "PlayerChar",
        //       Vector3.zero,
        //       Quaternion.identity,
        //       0
        //    );
        //    playerChar.transform.SetParent(parentTransform, true);

        //    GameObject playerCharCam = GameObject.Find("PlayerCharCam");
        //    playerCharCam.transform.position = new Vector3(playerChar.transform.position.x, playerChar.transform.position.y, gameObject.transform.position.z);
        //    playerCharCam.GetComponent<CamFollow>().TargetTransform = playerChar.transform;

        //    PlayerCharMovement playerCharMovement = playerChar.GetComponent<PlayerCharMovement>();
        //    playerCharMovement.CanMove = true;

        //    GameObject sceneLightMask = GameObject.Find("LightMask");
        //    sceneLightMask.GetComponent<MeshRenderer>().materials = new Material[1]{
        //        otherSideMask
        //    };
        //    LightCaster playerCharLightCaster = playerChar.GetComponent<LightCaster>();
        //    playerCharLightCaster.LightMask = sceneLightMask;

        //    yield return null;
        //}

        private bool LevelLoadedForAllPlayers() {
            foreach(var p in PhotonNetwork.PlayerList) {
				if(p.CustomProperties.TryGetValue("PlayerLoadedLevel", out object playerLoadedLevel)) { //Inline var declaration
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