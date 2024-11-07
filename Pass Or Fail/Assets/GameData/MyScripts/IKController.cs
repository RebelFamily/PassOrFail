using UnityEngine;

public class IKController : MonoBehaviour
{
    public Animator animator;

    // Targets for IK
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform leftFootTarget;
    public Transform rightFootTarget;
    public Transform headTarget;
    public Transform spineTarget;  // Target for spine adjustment
    public Transform leftElbowTarget;
    public Transform rightElbowTarget;
    public Transform leftKneeTarget;
    public Transform rightKneeTarget;

    // Weights for IK
    [Range(0, 1)] public float handWeight = 1.0f;
    [Range(0, 1)] public float footWeight = 1.0f;
    [Range(0, 1)] public float headWeight = 1.0f;
    [Range(0, 1)] public float elbowWeight = 1.0f;
    [Range(0, 1)] public float kneeWeight = 1.0f;
    [Range(0, 1)] public float spineWeight = 1.0f;
    [Range(0, 1)] public float spineLayerWeight = 1.0f; // Weight for the spine layer

    void OnAnimatorIK(int layerIndex)
    {
        if (animator)
        {
            // Set Hand IK
            if (leftHandTarget)
            {
                animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, handWeight);
                animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, handWeight);
            }

            if (rightHandTarget)
            {
                animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, handWeight);
                animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, handWeight);
            }

            // Set Foot IK
            if (leftFootTarget)
            {
                animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget.position);
                animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, footWeight);
                animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTarget.rotation);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, footWeight);
            }

            if (rightFootTarget)
            {
                animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTarget.position);
                animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, footWeight);
                animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTarget.rotation);
                animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, footWeight);
            }

            // Set Elbow IK
            if (leftElbowTarget)
            {
                animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowTarget.position);
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, elbowWeight);
            }

            if (rightElbowTarget)
            {
                animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightElbowTarget.position);
                animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, elbowWeight);
            }

            // Set Knee IK
            if (leftKneeTarget)
            {
                animator.SetIKHintPosition(AvatarIKHint.LeftKnee, leftKneeTarget.position);
                animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, kneeWeight);
            }

            if (rightKneeTarget)
            {
                animator.SetIKHintPosition(AvatarIKHint.RightKnee, rightKneeTarget.position);
                animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, kneeWeight);
            }

            // Set Head IK
            if (headTarget)
            {
                animator.SetLookAtPosition(headTarget.position);
                animator.SetLookAtWeight(headWeight);
            }

            // Adjusting the spine
            if (spineTarget)
            {
                Transform spineTransform = animator.GetBoneTransform(HumanBodyBones.Spine);
                if (spineTransform)
                {
                    // Calculate the target position and rotation
                    Vector3 targetPosition = Vector3.Lerp(spineTransform.position, spineTarget.position, spineWeight);
                    Quaternion targetRotation = Quaternion.Slerp(spineTransform.rotation, spineTarget.rotation, spineWeight);

                    // Apply adjustments to spine position and rotation
                    spineTransform.position = targetPosition;
                    spineTransform.rotation = targetRotation;

                    // Set the spine layer weight
                    animator.SetLayerWeight(animator.GetLayerIndex("SpineLayer"), spineLayerWeight);
                }
            }
        }
    }
}