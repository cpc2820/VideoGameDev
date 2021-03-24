using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class roombaAIWaypoint : MonoBehaviour
{
    public enum AIState
    {
        Patrol,
        Chase
    }
    public AIState aiState;
    public GameObject[] waypoints;
    public int currWaypoint;
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject player;
    public float fieldOfViewAngle = 15.0f;
    public bool showRoombaViews;
    public int viewDistance = 20;
    public int followTimeMax = 1000;
    public NavMeshHit hit;
    private int currFollowTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        currWaypoint = -1;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        aiState = AIState.Patrol;
        setNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDir = player.transform.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        //Vector3.Angle only returns positive value, use cross product to see if needs to be negative
        Vector3 cross = Vector3.Cross(targetDir, transform.forward);
        if (cross.y > 0)
        {
            angle = -angle;
        }

        if (showRoombaViews)
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * viewDistance, Color.white); //straight
            Debug.DrawRay(transform.position, transform.TransformDirection(Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward) * viewDistance, Color.cyan); //player
            Debug.DrawRay(transform.position, transform.TransformDirection(Quaternion.AngleAxis(fieldOfViewAngle, Vector3.up) * Vector3.forward) * viewDistance, Color.magenta); //edge of view
            Debug.DrawRay(transform.position, transform.TransformDirection(Quaternion.AngleAxis(-fieldOfViewAngle, Vector3.up) * Vector3.forward) * viewDistance, Color.magenta); //edge of view
        }
        if (angle < fieldOfViewAngle && targetDir.magnitude <= viewDistance) //player within distance and within angle
        {
            //now check if raycast returns player
            if (!agent.Raycast(player.transform.position, out hit)) //agent.Raycast returns false if it has a clear line of sight between it and the player
            {
                aiState = AIState.Chase;
            }
        }
        switch (aiState)
        {
            case AIState.Patrol:
                currFollowTime = 0;
                agent.speed = 3.5f;
                agent.SetDestination(waypoints[currWaypoint].transform.position);
                break;
            case AIState.Chase:
                agent.speed = 8.0f;
                agent.SetDestination(player.transform.position);
                currFollowTime++;
                if (currFollowTime >= followTimeMax)
                {
                    aiState = AIState.Patrol;
                }
                break;
        }
        if (agent.remainingDistance <= 0.6 && !agent.pathPending)
        {
            setNextWaypoint();
        }
    }

    private void setNextWaypoint()
    {
        if (waypoints.Length == 0)
        {
            Debug.Log("waypoints array is empty");
        }
        else
        {
            currWaypoint = (currWaypoint + 1) % waypoints.Length;
        }
    }
}
