using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roombaAI : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject target;
    public Vector3 initialMovementDir;
    public int rayCastDist = 3;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(Vector3.forward * -10);
    }

    // Update is called once per frame
    void Update()
    {

        //have agent move in a straight line, once it hits an obstacle
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), rayCastDist))
        {
            Debug.Log("hit wall");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * rayCastDist, Color.yellow);
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.,Random.Range(-20.0f, 20.0f),0), Time.deltaTime * 1);
            //agent.SetDestination(transform.TransformDirection(transform.forward));
            agent.Move(transform.TransformDirection(Vector3.forward * -1));
        }
        else
        {
            Debug.Log("not hit wall");
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
    }
}
