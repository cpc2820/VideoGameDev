using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObjectController : MonoBehaviour
{


    /************************************************************
     * 
     * GLOBAL VARS
     * 
     ************************************************************/
    public GameObject player;
    public PlayerController playerController;
    public Animator anim;
    public IKHandPlacement ikScript;
    public Transform rightHandTransform;
    public Transform leftHandTransform;
    public GameObject rightHand;
    public GameObject draggableHandle;
    public GameObject draggableObject;

    public Rigidbody handleRB;
    [Range(0, 10f)] public float idleMass;
    [Range(0, 100f)] public float activeMass;

    public int blendShapeCount;
    public GameObject body;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Mesh mesh;
    [Range(0, 10f)] public float maxDistance;
    
    
    public bool isBeingDragged;

    public float distanceFromPlayer;



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
        blendShapeCount = mesh.blendShapeCount;

        handleRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        distanceFromPlayer = (player.transform.position - transform.position).magnitude;

        if (distanceFromPlayer <= maxDistance)
        {
            ikScript.setHandTarget(transform);

            if (playerController.isDragging)
            {
                skinnedMeshRenderer.SetBlendShapeWeight(0, 99f);
                handleRB.mass = activeMass;
                handleRB.isKinematic = true;
                draggableHandle.layer = 14;
                draggableObject.layer = 14;
                return;
            }

            draggableHandle.layer = 13;
            draggableObject.layer = 13;
        }
        skinnedMeshRenderer.SetBlendShapeWeight(0, 0f);
        handleRB.isKinematic = false;
        handleRB.mass = idleMass;
    }

    void FixedUpdate()
    {
        if (distanceFromPlayer <= maxDistance && playerController.isDragging)
        {
            handleRB.MovePosition((handleRB.transform.position + ((leftHandTransform.position + rightHandTransform.position) / 2.0f)) / 2.0f);
        }
    }
}

