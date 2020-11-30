using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Observer : MonoBehaviour
{
    private Transform player;
    public GameEnding gameEnding;

    bool m_IsPlayerInRange;

    private PhotonView photonView;

    private void Start(){
        photonView = transform.parent.GetComponent<PhotonView>();
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.name == "JohnLemon(Clone)"){
            player = other.transform;
            m_IsPlayerInRange = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if(other.gameObject.name == "JohnLemon(Clone)"){
            m_IsPlayerInRange = false;
        }
    }

    void Update ()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit raycastHit;
            
            if (Physics.Raycast (ray, out raycastHit))
            {
                if (raycastHit.collider.transform == player)
                {
                    photonView.RPC("CallCaughtPlayer", RpcTarget.All);
                }
            }
        }
    }

    public void CaughtPlayer(){
        gameEnding.CaughtPlayer();
    }
}
