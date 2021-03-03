using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    /************************************************************
     * 
     * GLOBAL VARS
     * 
     ************************************************************/
    public Transform target;
    public Transform pivot;
    public Vector3 offset;
    private Vector3 lookAt;

    public bool useOffsetValues;
    public float smoothSpeed;
    [Range(0f, 1f)] public float  ySmoothBlend;
    public float rotateSpeed;
    public float ySpeed;

    public float maxViewAngleSoft;
    public float maxViewAngleHard;
    public float minViewAngleSoft;
    public float minViewAngleHard;
    public bool invertY;

    public LayerMask collisionLayers;

    private Vector3 previousPosition;
    private Vector3 currentVelocity;
    private Quaternion previousRotation;
    //private float verticalLookAtOffset;
    private float yDiff;




    /************************************************************
     * 
     * START
     * 
     ************************************************************/
    void Start()
    {
        if (useOffsetValues)
        {
            offset = target.position - transform.position;
        }
         
        pivot.transform.position = target.transform.position;
        pivot.transform.parent = null;

        yDiff = transform.position.y - pivot.transform.position.y;

        Cursor.lockState = CursorLockMode.Locked;
    }



    /************************************************************
     * 
     * LATE UPDATE
     * 
     ************************************************************/
    void LateUpdate()
    {

        //Get X position of mouse and rotate the pivot
        HandleInput("Mouse X", "Mouse Y");

        //Limit viewing Angles
        LimitViewingAngles();

        //Move the Camera
        MoveCamera();
    }



    /************************************************************
     * LIMIT VIEWING ANGLES
     ************************************************************/
     void HandleInput(string xAxis, string yAxis)
    {
        float horizontal = Input.GetAxis(xAxis) * rotateSpeed;
        pivot.Rotate(0, horizontal, 0);

        //Get Y position of mouse and rotate pivot
        float vertical = Input.GetAxis(yAxis) * ySpeed;
        if (invertY)
        {
            pivot.Rotate(vertical, 0, 0);
        }
        else
        {
            pivot.Rotate(-vertical, 0, 0);
        }
    }



    /************************************************************
     * LIMIT VIEWING ANGLES
     ************************************************************/
    void LimitViewingAngles()
    {
        //Limit downward viewing angle
        if (pivot.rotation.eulerAngles.x < 180f)
        {
            //verticalLookAtOffset = (pivot.rotation.eulerAngles.x) / -50f;
            if (pivot.rotation.eulerAngles.x > maxViewAngleSoft)
            {
                float xCurr = pivot.rotation.eulerAngles.x;
                float xDiff = xCurr - maxViewAngleSoft;
                pivot.rotation = Quaternion.Euler(pivot.rotation.eulerAngles.x - (xDiff * Time.deltaTime * 3), pivot.rotation.eulerAngles.y, 0);
                if (pivot.rotation.eulerAngles.x > maxViewAngleHard)
                {
                    pivot.rotation = Quaternion.Euler(maxViewAngleHard, pivot.rotation.eulerAngles.y, 0);
                }
            }
        }

        //Limit upward viewing angle
        if (pivot.rotation.eulerAngles.x > 180f)
        {
            //verticalLookAtOffset = (pivot.rotation.eulerAngles.x - 360f) / -50f;
            if (pivot.rotation.eulerAngles.x < minViewAngleSoft)
            {
                float xCurr = pivot.rotation.eulerAngles.x;
                float xDiff = xCurr - minViewAngleSoft;
                pivot.rotation = Quaternion.Euler(pivot.rotation.eulerAngles.x - (xDiff * Time.deltaTime * 3), pivot.rotation.eulerAngles.y, 0);
                if (pivot.rotation.eulerAngles.x < minViewAngleHard)
                {
                    pivot.rotation = Quaternion.Euler(minViewAngleHard, pivot.rotation.eulerAngles.y, 0);
                }
            }
        }
    }



    /************************************************************
     * MOVE THE CAMERA  
     ************************************************************/
    void MoveCamera()
    {
        //Move camera based on rotation of target & original offset
        float yAngle = pivot.eulerAngles.y;
        float xAngle = pivot.eulerAngles.x;

        //update lookAt
        lookAt = new Vector3(target.position.x, target.position.y + /*verticalLookAtOffset*/ + yDiff, target.position.z);

        //Handle Camera rotation
        Quaternion rotation = Quaternion.Euler(xAngle, yAngle, 0);
        pivot.rotation = rotation;              //Set this to smoothedRotation to smooth camera rotation.

        //Handle camera position
        Vector3 desiredPosition = target.position - (pivot.rotation * offset);
        desiredPosition = HandleCollision(desiredPosition);
        Vector3 smoothedPosition = Vector3.SmoothDamp(previousPosition, desiredPosition, ref currentVelocity, smoothSpeed);
        //smoothedPosition.y = (smoothedPosition.y + desiredPosition.y) / 2.0f;
        smoothedPosition.y = (ySmoothBlend * smoothedPosition.y) + ((1.0f - ySmoothBlend) * desiredPosition.y);
        transform.position = smoothedPosition;   //Set this to smoothedPosition to smooth camera position.

        //Update Previous values
        previousPosition = transform.position;

        //Look at the lookAt thingy
        transform.LookAt(lookAt);
    }



    /************************************************************
     * DETECT COLLISIONS AND MOVE CAMERA ACCORDINGLY
     ************************************************************/
    Vector3 HandleCollision(Vector3 inputPosition)
    {
        RaycastHit hit;

        //If anything intersects the line between the target and the current camera position
        if (Physics.Linecast(lookAt, inputPosition, out hit, collisionLayers))
        {
            float minDistance = 1f;
            float maxDistance = offset.magnitude;
            float distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);      //Keep the camera within a certain distance,
            Vector3 newPos = (lookAt - hit.point).normalized * distance;                        //Find a new position along that line for the camera,
            Vector3 outputPosition = lookAt - newPos;                                           //and set the new camera position to that location.
            return outputPosition;                                                              //Then, return that new position.
        }

        return inputPosition;
    }
}