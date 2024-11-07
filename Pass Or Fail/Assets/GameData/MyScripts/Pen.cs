using UnityEngine;
public class Pen : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Animator penCover;
    private const string AddBackPenCoverString = "AddBackPenCover";
    public void EnableAnimator(bool flag)
    {
        animator.enabled = flag;
    }
    public void RemovePenCover()
    {
        penCover.enabled = true;
    }
    public void AddBackPenCover()
    {
        penCover.Play(AddBackPenCoverString);   
    }
    public bool IsPenFilled()
    {
        var timeValue = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (timeValue is >= 0.75f and <= 1f)
        {
            //smallFront.SetActive(false);
            //bigFront.SetActive(true);
            return true;
        }
        else
        {
            return false;
        }
    }
}