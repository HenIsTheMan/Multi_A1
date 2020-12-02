﻿using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;

    bool m_IsPlayerAtExit;
    private static bool m_IsPlayerCaught;
    float m_Timer;
    bool m_HasAudioPlayed;
    
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer ()
    {
        m_IsPlayerCaught = true;
    }

    void Update ()
    {
        if(m_IsPlayerAtExit){
            EndLevel (exitBackgroundImageCanvasGroup, false, exitAudio);
        } else if(m_IsPlayerCaught){
            EndLevel (caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    void EndLevel (CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }
            
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                StartCoroutine(DisconnectAndLoad());
            }
            else
            {
                Application.Quit ();
            }
        }
    }

    IEnumerator DisconnectAndLoad() {
        PhotonNetwork.Disconnect();
        //PhotonNetwork.LeaveRoom();

		while(PhotonNetwork.IsConnected) {
			yield return null;
		}
		//while(PhotonNetwork.InRoom) {
        //  yield return null;
        //}

        SceneManager.LoadScene(0);
    }

    //private IEnumerator EndOfGame(string winner, int score) {
    //    float timer = 5.0f;

    //    while(timer > 0.0f) {
    //        InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));

    //        yield return new WaitForEndOfFrame();

    //        timer -= Time.deltaTime;
    //    }

    //    PhotonNetwork.LeaveRoom();
    //}
}
