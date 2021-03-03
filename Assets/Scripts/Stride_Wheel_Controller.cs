using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stride_Wheel_Controller : MonoBehaviour
{



    /************************************************************
     * 
     * GLOBAL VARS
     * 
     ************************************************************/
    public CharacterController controller;
    public GameObject playerContainter;
    public GameObject playerModel;
    public Animator anim;

    public float currentRotation;
    public float maxRotation;
    [Range(0f, 2f)] public float minDiameter;
    [Range(0f, 2f)] public float maxDiameter;
    public int animFrame;

    private float radius;
    private float circumference;
    private float velocity;
    private float newRotation;



    /************************************************************
     * 
     * START
     * 
     ************************************************************/
    void Start()
    {
        transform.position = playerContainter.transform.position;
    }



    /************************************************************
     * 
     * UPDATE
     * 
     ************************************************************/
    void Update()
    {
        UpdateMovement();
        UpdateScale();
        UpdatePosition();
        UpdateRotation();
        UpdateAnimation();
    }

    void UpdateMovement()
    {
        velocity = new Vector3(controller.velocity.x, 0f, controller.velocity.z).magnitude;
    }

    void UpdateScale()
    {
        float newDiameter = Mathf.Clamp(velocity/7f, minDiameter, maxDiameter);
        transform.localScale = new Vector3(newDiameter, 0f, newDiameter);
        radius = transform.localScale.x/2;
        circumference = 2 * Mathf.PI * radius;
    }

    void UpdatePosition()
    {
        transform.position = playerContainter.transform.position;
        transform.position += new Vector3(0f, radius, 0f);
    }

    void UpdateRotation()
    {
        if (controller.isGrounded)
        {
            newRotation = (newRotation + 2 * velocity / circumference) % maxRotation;
        } else
        {
            newRotation = (int)(newRotation / 180f) * 180f;
        }
        transform.rotation = Quaternion.Euler(newRotation, playerModel.transform.rotation.eulerAngles.y, 90f);
        currentRotation = newRotation;
    }

    void UpdateAnimation()
    {
        anim.SetFloat("CurrentRotation", currentRotation);
        /*
        if (currentRotation < 90)
        {
            anim.Play("motion frame 1");
        }
        else if (currentRotation < 180)
        {
            anim.Play("motion frame 2");
        }
        else if (currentRotation < 270)
        {
            anim.Play("motion frame 3");
        }
        else
        {
            anim.Play("motion frame 4");
        }
        */
    }
}
