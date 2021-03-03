using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootPlacement : MonoBehaviour
{


    /************************************************************
     * 
     * GLOBAL VARS
     * 
     ************************************************************/
    public GameObject player;
    public Animator anim;
    public CharacterController controller;
    public PlayerController playerController;

    public LayerMask layerMask;

    [Range (-1f, 1f)] public float DistanceToGround;
    [Range(0f, 10f)] public float pelvisVertSpeed;
    [Range(-1f, 1f)] public float pelvisOffset;
    [Range(0f, 2f)] public float pelvisBounceAmount;
    private float lastPelvisPositionY;



    /************************************************************
     * 
     * START
     * 
     ************************************************************/
    void Start()
    {
        anim = player.GetComponent<Animator>();
        controller = player.GetComponent<CharacterController>();
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (anim)
        {
            SetIKWeights();
            MovePelvisHeight();
            FootIKHandler(AvatarIKGoal.LeftFoot);
            FootIKHandler(AvatarIKGoal.RightFoot);

        }
    }



    /************************************************************
     * 
     * SET IK WEIGHTS
     * 
     ************************************************************/
     private void SetIKWeights()
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LeftFootWeight"));
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, anim.GetFloat("LeftFootWeight"));
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RightFootWeight"));
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, anim.GetFloat("RightFootWeight"));
    }


    /************************************************************
     * 
     * MOVE PELVIS HEIGHT
     * 
     ************************************************************/
    private void MovePelvisHeight()
    {
        if (anim.GetIKPosition(AvatarIKGoal.RightFoot) == Vector3.zero
         || anim.GetIKPosition(AvatarIKGoal.LeftFoot) == Vector3.zero
         || lastPelvisPositionY == 0)
        {
            lastPelvisPositionY = anim.bodyPosition.y;
            return;
        }

        float leftOffsetPos = anim.GetIKPosition(AvatarIKGoal.LeftFoot).y - transform.position.y;
        float rightOffsetPos = anim.GetIKPosition(AvatarIKGoal.RightFoot).y - transform.position.y;
        float totalOffset = (leftOffsetPos < rightOffsetPos) ? leftOffsetPos : rightOffsetPos;

        float percentMaxSpeed = playerController.currSpeed / playerController.sprintSpeed;
        float bounceAmount = pelvisBounceAmount - (percentMaxSpeed * percentMaxSpeed * pelvisBounceAmount);

        Vector3 newPelvisPos = anim.bodyPosition + Vector3.up * totalOffset * bounceAmount;
        newPelvisPos.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPos.y + pelvisOffset, pelvisVertSpeed);

        anim.bodyPosition = newPelvisPos;
        lastPelvisPositionY = anim.bodyPosition.y;
    }



    /************************************************************
     * 
     * HANDLE FOOT IK
     * 
     ************************************************************/
     private void FootIKHandler(AvatarIKGoal foot)
    {

        RaycastHit hit;
        Ray ray = new Ray(anim.GetIKPosition(foot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
        {

            //Position
            Vector3 footPosition = hit.point;
            footPosition.y += DistanceToGround;
            anim.SetIKPosition(foot, footPosition);

            //Rotation
            Vector3 rotAxis = Vector3.Cross(Vector3.up, hit.normal);
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            Quaternion newRotation = Quaternion.AngleAxis(angle * anim.GetIKRotationWeight(foot), rotAxis);
            anim.SetIKRotation(foot, newRotation * anim.GetIKRotation(foot));
        }
    }
}
