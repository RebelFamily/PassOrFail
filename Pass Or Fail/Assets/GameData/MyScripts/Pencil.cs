using UnityEngine;
public class Pencil : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject smallFront, bigFront;
    public void EnableAnimator(bool flag)
    {
        animator.enabled = flag;
    }
    public bool IsPencilSharped()
    {
        var timeValue = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (timeValue is >= 0.2f and <= 0.3f or >= 0.6f and <= 0.7f or >= 0.85f and <= 0.95f)
        {
            smallFront.SetActive(false);
            bigFront.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }
}