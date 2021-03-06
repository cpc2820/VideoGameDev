﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObjectController : MonoBehaviour
{


    /************************************************************
     * 
     * GLOBAL VARS
     * 
     ************************************************************/
    [Header("Player")]
    public GameObject player;
    private PlayerController playerController;
    private Animator anim;
    private IKHandPlacement ikScript;

    [Header("Character Body")]
    public GameObject body;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Mesh mesh;
    private int blendShapeCount;

    [Header("Character Hand Anchor")]
    public GameObject handAnchor;
    private HandAnchor handAnchorScript;

    [Header("Draggable Stuff")]
    public GameObject draggableObject;
    public GameObject draggableHandle;
    private float maxPickupDistance;
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private bool isBeingDragged;
    private Rigidbody handleRB;



    /************************************************************
     * 
     * START
     * 
     ************************************************************/
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        anim = player.GetComponent<Animator>();
        ikScript = player.GetComponent<IKHandPlacement>();
        skinnedMeshRenderer = body.GetComponent<SkinnedMeshRenderer>();
        mesh = body.GetComponent<SkinnedMeshRenderer>().sharedMesh;
        handAnchorScript = handAnchor.GetComponent<HandAnchor>();
        blendShapeCount = mesh.blendShapeCount;
        handleRB = GetComponent<Rigidbody>();
        maxPickupDistance = handAnchorScript.maxPickupDistance;
    }

    void Update()
    {
        distanceFromPlayer = (player.transform.position - gameObject.transform.position).magnitude;

        if (distanceFromPlayer <= maxPickupDistance)
        {
            ikScript.setHandTarget(transform);

            if (playerController.isDragging && !isBeingDragged && !handAnchorScript.isDragging)
            {
                updateDragging(100f, true);
                return;
            }
        }

        if (isBeingDragged && !playerController.isDragging)
        {
            updateDragging(0f, false);
        }
    }

    void FixedUpdate()
    {
        if (distanceFromPlayer <= maxPickupDistance &&
            isBeingDragged &&
            playerController.isDragging)
        {
            handleRB.MovePosition(handAnchorScript.newPosition);
            print("working");
            setLayer(14);
            return;
        }
        else 
        {
            setLayer(13);
        }
    }

    public void updateDragging(float blendShapeWeight, bool newBool)
    {
        skinnedMeshRenderer.SetBlendShapeWeight(0, blendShapeWeight);
        handleRB.isKinematic =          newBool;
        handAnchorScript.isDragging =   newBool;
        isBeingDragged =                newBool;
    }

    public void setLayer(int newLayer)
    {
        draggableHandle.layer = newLayer;
        draggableObject.layer = newLayer;
    }
}