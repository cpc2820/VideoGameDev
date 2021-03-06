﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnchor : MonoBehaviour
{


    /**************************************************************************
     * 
     *                             GLOBAL VARS
     * 
     *************************************************************************/
    [Header("Hand game objects")]
    public GameObject leftHand;
    public GameObject rightHand;

    [Header("Variables")]
    [Range(0, 1f)] public float positionUpdateSpeed;
    [Range(0, 1f)] public float rotationUpdateSpeed;
    [Range(0, 10f)] public float maxPickupDistance;

    [Header("Is the character holding something?")]
    public bool isDragging;

    [Header("Transforms")]
    public Vector3 newPosition;
    private Vector3 lastPosition;
    private Quaternion newRotation;
    private Quaternion lastRotation;


    /**************************************************************************
     * 
     *                                 START
     * 
     *************************************************************************/
    void Start()
    {
        newPosition = getNewPosition();
        newRotation = getNewRotation();
        setLast();
    }

    /**************************************************************************
     * 
     *                                UPDATE
     * 
     *************************************************************************/
    void Update()
    {
        setLast();
        newPosition = updatePosition();
        newRotation = updateRotation();
    }

    /**************************************************************************
     * 
     *                      GET NEW POSITION & ROTATION
     * 
     *************************************************************************/
    public Vector3 getNewPosition()
    {
        return Vector3.Lerp(leftHand.transform.position, rightHand.transform.position, 0.5f);
    }

    public Quaternion getNewRotation()
    {
        return Quaternion.Lerp(leftHand.transform.rotation, rightHand.transform.rotation, 0.5f);
    }

    /**************************************************************************
     * 
     *                        UPDATE POSITION & ROTATION
     * 
     *************************************************************************/
    public Vector3 updatePosition()
    {
        return Vector3.Lerp(lastPosition, getNewPosition(), positionUpdateSpeed);
    }

    public Quaternion updateRotation()
    {
        return Quaternion.Lerp(lastRotation, getNewRotation(), rotationUpdateSpeed);
    }

    /**************************************************************************
     * 
     *                     SET LAST POSITION & ROTATION
     * 
     *************************************************************************/
    public void setLast()
    {
        lastPosition = newPosition;
        lastRotation = newRotation;
    }
}
