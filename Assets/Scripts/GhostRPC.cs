using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GhostRPC: MonoBehaviour{
	[PunRPC]
	public void CallCaughtPlayer(){
		GetComponentInChildren<Observer>().CaughtPlayer();
	}
}
