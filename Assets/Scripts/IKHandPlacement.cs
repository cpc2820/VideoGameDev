﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKHandPlacement : MonoBehaviour
{


    /************************************************************
     * 
     * GLOBAL VARS
     * 
     ************************************************************/
    public GameObject player;
    public Animator anim;
    public LayerMask layerMask;
    public Transform handTarget;
    public float ikStrength;
    [Range(1, 20f)] public float ikStrengthDenominator;



    /************************************************************
     * 
     * START
     * 
     ************************************************************/
    void Start()
    {
        anim = player.GetComponent<Animator>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            SetIKWeights();
            if (handTarget != null)
            {
                ikStrength = 1.0f / (Mathf.Pow((transform.position - handTarget.transform.position).magnitude, 1.5f) * ikStrengthDenominator);
                anim.SetLayerWeight(1, ikStrength);
                HandIKHandler(AvatarIKGoal.LeftHand);
                HandIKHandler(AvatarIKGoal.RightHand);
            }
        }
    }


    /************************************************************
     * 
     * SET IK WEIGHTS
     * 
     ************************************************************/
    private void SetIKWeights()
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikStrength);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, ikStrength);
    }


    /************************************************************
     * 
     * HANDLE HAND IK
     * 
     ************************************************************/
    private void HandIKHandler(AvatarIKGoal hand)
    {
        anim.SetIKPosition(hand, handTarget.position);
    }

    public Transform getHandTarget()
    {
        return handTarget;
    }

    public void setHandTarget(Transform newHandTarget)
    {
        handTarget = newHandTarget;
    }
}

