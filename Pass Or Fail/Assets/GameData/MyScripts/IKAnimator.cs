using UnityEngine;
public class IKAnimator : MonoBehaviour
{
    [System.Serializable]
    public class IKPoints
    {
        public Transform lookAtObject;
        public Transform rightHandObj, leftHandObj, rightElbowObj, leftElbowObj, rightFootObj, leftFootObj;
    }
	[SerializeField]
    protected Animator animator;
    public IKPoints ikPoints;
    public bool ikActive = false;
    public float lookAtWeight = 0f;
    public float rightHandWeight = 0f;
    public float rightElbowWeight = 0f;
    public float leftHandWeight = 0f;
    public float leftElbowWeight = 0f;
    public float rightFootWeight = 0f;
    public float leftFootWeight = 0f;
    private bool animateFlag = false;
    
    //a callback for calculating IK
    private void OnAnimatorIK(int layerIndex)
    {
        if (!animator)
            return;
        //if the IK is active, set the position and rotation directly to the goal. 
        if (ikActive)
        {
            //Set the look target position, if one has been assigned
            if (ikPoints.lookAtObject)
            {
                animator.SetLookAtWeight(lookAtWeight);
                animator.SetLookAtPosition(ikPoints.lookAtObject.position);
            }

            // Set the right hand target position and rotation, if one has been assigned
            if (ikPoints.rightHandObj)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, ikPoints.rightHandObj.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, ikPoints.rightHandObj.rotation);
            }
            if (ikPoints.rightElbowObj)
            {
                animator.SetIKHintPosition(AvatarIKHint.RightElbow, ikPoints.rightElbowObj.position);
                animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightElbowWeight);
            }
            // Set the left hand target position and rotation, if one has been assigned
            if (ikPoints.leftHandObj)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, ikPoints.leftHandObj.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, ikPoints.leftHandObj.rotation);
            }
            if (ikPoints.leftElbowObj)
            {
                animator.SetIKHintPosition(AvatarIKHint.LeftElbow, ikPoints.leftElbowObj.position);
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftElbowWeight);
            }
            if (ikPoints.rightFootObj)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, ikPoints.rightFootObj.position);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, ikPoints.rightFootObj.rotation);
            }
            if (ikPoints.leftFootObj)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, ikPoints.leftFootObj.position);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, ikPoints.leftFootObj.rotation);
            }
        }
        //if the IK is not active, set the position and rotation of the hand and head back to the original position
        else
        {
            //weight = 0f;
            if (ikPoints.rightHandObj != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
                animator.SetIKPosition(AvatarIKGoal.RightHand, ikPoints.rightHandObj.position);
                animator.SetIKRotation(AvatarIKGoal.RightHand, ikPoints.rightHandObj.rotation);
            }
            if (ikPoints.rightElbowObj)
            {
                animator.SetIKHintPosition(AvatarIKHint.RightElbow, ikPoints.rightElbowObj.position);
                animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightElbowWeight);
            }
            if (ikPoints.leftHandObj != null)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, ikPoints.leftHandObj.position);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, ikPoints.leftHandObj.rotation);
            }
            if (ikPoints.leftElbowObj)
            {
                animator.SetIKHintPosition(AvatarIKHint.LeftElbow, ikPoints.leftElbowObj.position);
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftElbowWeight);
            }
            if (ikPoints.rightFootObj)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rightFootWeight);
                animator.SetIKPosition(AvatarIKGoal.RightFoot, ikPoints.rightFootObj.position);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, ikPoints.rightFootObj.rotation);
            }
            if (ikPoints.leftFootObj)
            {
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, ikPoints.leftFootObj.position);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, ikPoints.leftFootObj.rotation);
            }
            //Set the look target position, if one has been assigned
            if (ikPoints.lookAtObject)
            {
                animator.SetLookAtWeight(lookAtWeight);
                animator.SetLookAtPosition(ikPoints.lookAtObject.position);
            }
        }
    }

    private void Update()
    {
        if (!animateFlag)
            return;
        if (ikActive)
        {
            leftHandWeight = Mathf.Lerp(leftHandWeight, 1f, Time.deltaTime);
            leftElbowWeight = Mathf.Lerp(leftElbowWeight, 1f, Time.deltaTime);
            rightHandWeight = Mathf.Lerp(rightHandWeight, 1f, Time.deltaTime);
            rightElbowWeight = Mathf.Lerp(rightElbowWeight, 1f, Time.deltaTime);
            rightFootWeight = Mathf.Lerp(rightFootWeight, 1f, Time.deltaTime);
            leftFootWeight = Mathf.Lerp(leftFootWeight, 1f, Time.deltaTime);
            lookAtWeight = Mathf.Lerp(lookAtWeight, 1f, Time.deltaTime);
        }
        else
        {
            leftHandWeight = Mathf.Lerp(leftHandWeight, 0f, Time.deltaTime);
            leftElbowWeight = Mathf.Lerp(leftElbowWeight, 0f, Time.deltaTime * 3f);
            rightHandWeight = Mathf.Lerp(rightHandWeight, 0f, Time.deltaTime);
            rightElbowWeight = Mathf.Lerp(rightElbowWeight, 0f, Time.deltaTime * 3f);
            rightFootWeight = Mathf.Lerp(rightFootWeight, 0f, Time.deltaTime);
            leftFootWeight = Mathf.Lerp(leftFootWeight, 0f, Time.deltaTime);
            lookAtWeight = Mathf.Lerp(lookAtWeight, 0f, Time.deltaTime);
        }
    }
}
