using System.Collections;
using DG.Tweening;
using UnityEngine;
public class CashEffect : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform target;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform[] cash;
    private int cashIndex = 0;
    [SerializeField] private bool isPlaying;
    private readonly WaitForSeconds _waitForSeconds = new WaitForSeconds(0.02f);
    [SerializeField] private AnimationCurve animationCurve;
    private void OnEnable()
    {
        //AnimateCash();
        StartCoroutine(ShowCashEffect());
    }
    private void AnimateCash()
    {
        //cash[cashIndex].position = target.position;
        var screenPos = RectTransformUtility.WorldToScreenPoint(null, start.position);
        var targetPositionForCashPile = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
        cash[cashIndex].position = targetPositionForCashPile;
        cash[cashIndex].transform.DOMove(target.position, 0.2f).SetEase(animationCurve).OnComplete(() =>
        {
            cashIndex++;
            if (cashIndex >= cash.Length) cashIndex = 0;
            AnimateCash();
        });
    }
    private IEnumerator ShowCashEffect()
    {
        var screenPos = RectTransformUtility.WorldToScreenPoint(null, start.position);
        var targetPositionForCashPile = camera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10f));
        cash[cashIndex].position = targetPositionForCashPile;
        cash[cashIndex].transform.DOMove(target.position, 0.2f).SetEase(Ease.Linear);
        while (isPlaying)
        {
            cashIndex++;
            if (cashIndex >= cash.Length) cashIndex = 0;
            yield return _waitForSeconds;
            cash[cashIndex].position = targetPositionForCashPile;
            cash[cashIndex].transform.DOMove(target.position, 0.2f).SetEase(Ease.Linear);
        }
    }
    public void SetTarget( Transform newTarget)
    {
        target = newTarget;
    }
}