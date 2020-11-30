using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WaypointPatrol: MonoBehaviour{
    public NavMeshAgent navMeshAgent;
    public List<Transform> waypoints = new List<Transform>(); //Use List so can add stuff during runtime

    int m_CurrentWaypointIndex;

    void Start ()
    {
        //navMeshAgent.SetDestination (waypoints[0].position);
    }

    public void StartAI(){
        enabled = true;
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    void Update ()
    {
        //Debug.Log("moving!! " + navMeshAgent.remainingDistance + ' ' + navMeshAgent.stoppingDistance);

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance){
            //Debug.Log("Stopping");

            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Count;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
        }
	}
}
