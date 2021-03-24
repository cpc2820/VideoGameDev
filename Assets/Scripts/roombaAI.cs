using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roombaAI : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);
        Debug.Log("chasing target");
    }
}
