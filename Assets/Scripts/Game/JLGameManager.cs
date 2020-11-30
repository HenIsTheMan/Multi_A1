﻿using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public sealed class JLGameManager: MonoBehaviourPunCallbacks{
    [SerializeField] private List<GameObject> prefabs;

    public static JLGameManager Instance = null;
    public Text InfoText;
    public GameObject wayPoints;

    #region UNITY

    private void Awake() {
        Instance = this;
    }

    public override void OnEnable() {
        base.OnEnable();
        CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    }

    private void Start() {
        Hashtable props = new Hashtable {
            {JLGame.PLAYER_LOADED_LEVEL, true}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

		///Manually fill ResourceCache of DefaultPool
		if(PhotonNetwork.PrefabPool is DefaultPool pool && prefabs != null) {
			foreach(GameObject prefab in prefabs) {
				pool.ResourceCache.Add(prefab.name, prefab);
			}
		}
	}

    public override void OnDisable()
    {
        base.OnDisable();

        CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
    }

    #endregion

    #region COROUTINES

    private IEnumerator EndOfGame(string winner, int score)
    {
        float timer = 5.0f;

        while (timer > 0.0f)
        {
            InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));

            yield return new WaitForEndOfFrame();

            timer -= Time.deltaTime;
        }

        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region PUN CALLBACKS

    public override void OnDisconnected(DisconnectCause cause)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DemoAsteroids-LobbyScene");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            //StartCoroutine(SpawnAsteroid());
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer){
        CheckEndOfGame();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
{
    if (changedProps.ContainsKey(JLGame.PLAYER_LIVES))
    {
        CheckEndOfGame();
        return;
    }

    if (!PhotonNetwork.IsMasterClient)
    {
        return;
    }


    // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
    int startTimestamp;
    bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

    if (changedProps.ContainsKey(JLGame.PLAYER_LOADED_LEVEL))
    {
        if (CheckAllPlayerLoadedLevel())
        {
            if (!startTimeIsSet)
            {
                CountdownTimer.SetStartTime();
            }
        }
        else
        {
            // not all players loaded yet. wait:
            Debug.Log("setting text waiting for players! ", this.InfoText);
            InfoText.text = "Waiting for other players...";
        }
    }

}

    #endregion

    private void StartGame(){
        Vector3 position = new Vector3(-9.8f, 0.0f, -3.2f);
        Quaternion rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        PhotonNetwork.Instantiate("JohnLemon", position, rotation, 0); //Avoid this call on rejoin (JL was network-instantiated before)

        if(PhotonNetwork.IsMasterClient){
            SpawnGhosts();
        }
    }

    private void SpawnGhosts(){
        //Pos taken from the existing ghosts
        Vector3[] posArr;
        posArr = new[]{
            new Vector3(-5.3f, 0.0f, -3.1f),
            new Vector3(1.5f, 0.0f, 4.0f),
            new Vector3(3.2f, 0.0f, 6.5f),
            new Vector3(7.4f, 0.0f, -3.0f)
        };

        int posArrLength = posArr.Length;
        for(int i = 0; i < posArrLength; ++i){
            GameObject obj = PhotonNetwork.InstantiateRoomObject("Ghost", posArr[i], Quaternion.identity); //Create ghost obj
            obj.transform.SetParent(GameObject.Find("Enemies").transform);

            WaypointPatrol waypointPatrol = obj.GetComponent<WaypointPatrol>();

            waypointPatrol.waypoints.Add(wayPoints.transform.GetChild(i * 2));
            waypointPatrol.waypoints.Add(wayPoints.transform.GetChild(i * 2 + 1));

            waypointPatrol.StartAI();
        }
    }

    private bool CheckAllPlayerLoadedLevel()
{
    foreach (Player p in PhotonNetwork.PlayerList)
    {
        object playerLoadedLevel;

        if (p.CustomProperties.TryGetValue(JLGame.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
        {
            if ((bool)playerLoadedLevel)
            {
                continue;
            }
        }

        return false;
    }

    return true;
}

    private void CheckEndOfGame()
    {
    //bool allDestroyed = true;

    //foreach (Player p in PhotonNetwork.PlayerList)
    //{
    //    object lives;
    //    if (p.CustomProperties.TryGetValue(JLGame.PLAYER_LIVES, out lives))
    //    {
    //        if ((int)lives > 0)
    //        {
    //            allDestroyed = false;
    //            break;
    //        }
    //    }
    //}

    //if (allDestroyed)
    //{
    //    if (PhotonNetwork.IsMasterClient)
    //    {
    //        StopAllCoroutines();
    //    }

    //    string winner = "";
    //    int score = -1;

    //    foreach (Player p in PhotonNetwork.PlayerList)
    //    {
    //        if (p.GetScore() > score)
    //        {
    //            winner = p.NickName;
    //            score = p.GetScore();
    //        }
    //    }

    //    StartCoroutine(EndOfGame(winner, score));
    //}
    }

    private void OnCountdownTimerIsExpired()
    {
        StartGame();
    }
}