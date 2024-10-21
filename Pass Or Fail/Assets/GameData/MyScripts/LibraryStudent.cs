using DG.Tweening;
using UnityEngine;
public class LibraryStudent : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool isGossiping = false, isAlreadyLost = false;
    private Transform target;
    public bool isAdded { get; set; } = false;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private ParticleSystem particles;
    private static readonly int Gossip = Animator.StringToHash("Gossip");
    private static readonly int Sad = Animator.StringToHash("Sad");
    public void StartGossiping()
    {
        animator.SetTrigger(Gossip);
    }
    public void SetGossipingFlag(bool flag)
    {
        boxCollider.enabled = flag;
        isGossiping = flag;
        particles.gameObject.SetActive(flag);
        /*if(flag)
            particles.Play();*/
    }
    public bool IsGossiping()
    {
        return isGossiping;
    }
    public void GetLost()
    {
        if(isAlreadyLost) return;
        isAlreadyLost = true;
        particles.gameObject.SetActive(false);
        animator.SetTrigger(Sad);
        target = transform.Find("RunAwayPoint");
        Invoke(nameof(Move), 1f);
    }
    private void Move()
    {
        transform.DOMove(target.position, 2f);
        SharedUI.Instance.gamePlayUIManager.controls.SetProgress();
    }
    public void DisableParticles()
    {
        particles.gameObject.SetActive(false);
    }
}