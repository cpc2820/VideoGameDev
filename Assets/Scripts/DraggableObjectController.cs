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
    public GameObject draggableHandle;
    public GameObject draggableObject;
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
    }

    void Update()
    {
        distanceFromPlayer = (player.transform.position - transform.position).magnitude;

        if (distanceFromPlayer <= maxDistance)
        {
            ikScript.setHandTarget(transform);

            if (playerController.isDragging)
            {
                draggableHandle.layer = 14;
                draggableObject.layer = 14;
                draggableHandle.transform.position = (leftHandTransform.position + rightHandTransform.position) / 2.0f;
                return;
            }

            draggableHandle.layer = 13;
            draggableObject.layer = 13;
        }
    }
}

