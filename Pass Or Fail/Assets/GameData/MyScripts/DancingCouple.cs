using DG.Tweening;
using UnityEngine;
public class DancingCouple : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController animatorController0, animatorController1;
    [SerializeField] private Transform character0, character1;
    [SerializeField] private Animator animator0, animator1;
    [SerializeField] private float goingCloseSpeed = 4f;
    [SerializeField] private GameObject particles;
    [SerializeField] private BoxCollider boxCollider;
    private const float DefaultPosition0 = -0.35f;
    private const float DefaultPosition1 = 0.35f;
    private const float KissPosition0 = -0.15f;
    private const float KissPosition1 = 0.15f;
    private bool inProgress = false;
    private Gender gender;
    private DanceActivity danceActivity;
    private const string BASIC_PATH = "Characters/Default/", KISS_ANIMATION = "Kiss", SALSA_ANIMATION = "Salsa";
    private void Start()
    {
        danceActivity = transform.parent.GetComponent<DanceActivity>();
        var index = 0;
        gender = Gender.FemaleStudent;
        var genderNo = Random.Range(0, 2);
        gender = genderNo > 0 ? Gender.MaleStudent : Gender.FemaleStudent;
        index = Random.Range(0, 4);
        var path = BASIC_PATH + gender + index;
        var character = Instantiate(Resources.Load<GameObject>( path), character0);
        animator0 = character.GetComponent<Animator>();
        animator0.runtimeAnimatorController = animatorController0;
        genderNo = Random.Range(0, 2);
        gender = genderNo > 0 ? Gender.MaleStudent : Gender.FemaleStudent;
        index = Random.Range(0, 4);
        path = BASIC_PATH + gender + index;
        character = Instantiate(Resources.Load<GameObject>( path), character1);
        animator1 = character.GetComponent<Animator>();
        animator1.runtimeAnimatorController = animatorController1;
    }
    public void TryToKiss()
    {
        //Debug.Log("TryToKiss");
        inProgress = true;
        character0.DOLocalMoveZ(KissPosition0, goingCloseSpeed).OnComplete(() =>
        {
            particles.SetActive(true);
            animator0.Play(KISS_ANIMATION, 1);
            boxCollider.enabled = true;
        });
        character1.DOLocalMoveZ(KissPosition1, goingCloseSpeed).OnComplete(() =>
        {
            animator1.Play(KISS_ANIMATION, 1);
        });
    }
    private void BackToNormal()
    {
        if(danceActivity.IsActivityFinished()) return;
        if(!inProgress) return;
        inProgress = false;
        boxCollider.enabled = false;
        character0.DOKill();
        character1.DOKill();
        particles.SetActive(false);
        animator0.Play(SALSA_ANIMATION, 1);
        animator1.Play(SALSA_ANIMATION, 1);
        character0.DOLocalMoveZ(DefaultPosition0, 1f);
        character1.DOLocalMoveZ(DefaultPosition1, 1f);
        danceActivity.StartKunjarKhana();
    }
    public bool IsInProgress()
    {
        return inProgress;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(PlayerPrefsHandler.Detector)) return;
        danceActivity.ShowPerfects(transform.position);
        BackToNormal();
    }
}