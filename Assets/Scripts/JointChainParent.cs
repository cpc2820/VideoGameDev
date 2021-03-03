using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointChainParent : MonoBehaviour
{

    [Range(0, 100f)] public float multiplier;
    private Transform thisParent;
    private Rigidbody thisRigidBody;
    private Vector3 parentPositionLastFrame = Vector3.zero;


    private void Awake()
    {
        thisParent = transform.parent;
        thisRigidBody = transform.GetComponent<Rigidbody>();
    }


    void Update()
    {
        thisRigidBody.AddForce((parentPositionLastFrame - thisParent.position) * multiplier);
        parentPositionLastFrame = thisParent.position;
    }
}
