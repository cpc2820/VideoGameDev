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

    public int blendShapeCount;
    public GameObject body;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public Mesh mesh;
    [Range(0, 10f)] public float maxDistance;

    public Rigidbody draggableRB;
    [Range(0, 10f)] public float massIdle;
    [Range(0, 100f)] public float massActive;

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
        draggableRB = GetComponent<Rigidbody>();
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
                draggableRB.mass = massActive;
                draggableHandle.layer = 14;
                draggableObject.layer = 14;
                draggableHandle.transform.position = (leftHandTransform.position + rightHandTransform.position) / 2.0f;
                return;
            }
        }
        skinnedMeshRenderer.SetBlendShapeWeight(0, 0f);
        draggableRB.mass = massIdle;
        draggableHandle.layer = 13;
        draggableObject.layer = 13;
    }
}

