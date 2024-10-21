using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class DanceActivity : MonoBehaviour
{
    [SerializeField] private GameObject perfects;
    [SerializeField] private DancingCouple[] dancingCouples;
    private bool isActivityStarted = false, isActivityFinished = false;
    private Image timerFiller;
    private int tempIndex = 0;
    private float tempDelay = 2f;
    public void StartActivity()
    {
        SoundController.Instance.PlayDanceMusic();
        var controls = SharedUI.Instance.gamePlayUIManager.controls;
        timerFiller = controls.GetTimerFiller();
        controls.EnableTouchPad(true);
        controls.EnableActivityUI(false);
        //StartCoroutine(StartKissing());
        //StartKunjarKhana();
        StartCoroutine(StartRomance());
        isActivityStarted = true;
    }
    private void Update()
    {
        if(!isActivityStarted || isActivityFinished) return;
        timerFiller.fillAmount += Time.deltaTime / 30f;
        if (!(timerFiller.fillAmount >= 1f)) return;
        isActivityFinished = true;
        timerFiller.fillAmount = 1f;
        GamePlayManager.Instance.LevelComplete(1f);
    }
    private IEnumerator StartRomance()
    {
        var delay = Random.Range(2f, 5f);
        var index = GetRandomIndex();
        yield return new WaitForSeconds(delay);
        if (!dancingCouples[index].IsInProgress())
        {
            dancingCouples[index].TryToKiss();
        }
        delay = Random.Range(2f, 5f);
        index = GetRandomIndex();
        yield return new WaitForSeconds(delay);
        if (!dancingCouples[index].IsInProgress())
        {
            dancingCouples[index].TryToKiss();
        }
        delay = Random.Range(2f, 5f);
        index = GetRandomIndex();
        yield return new WaitForSeconds(delay);
        if (!dancingCouples[index].IsInProgress())
        {
            dancingCouples[index].TryToKiss();
        }
        delay = Random.Range(2f, 5f);
        index = GetRandomIndex();
        yield return new WaitForSeconds(delay);
        if (!dancingCouples[index].IsInProgress())
        {
            dancingCouples[index].TryToKiss();
        }
        delay = Random.Range(2f, 5f);
        index = GetRandomIndex();
        yield return new WaitForSeconds(delay);
        if (!dancingCouples[index].IsInProgress())
        {
            dancingCouples[index].TryToKiss();
        }
    }
    public void StartKunjarKhana()
    {
        Invoke(nameof(InvokeKunjarKhana), Random.Range(2f, 5f));
    }
    private void InvokeKunjarKhana()
    {
        var index = GetRandomIndex();
        if (!dancingCouples[index].IsInProgress())
        {
            dancingCouples[index].TryToKiss();
        }
    }
    private IEnumerator StartKissing()
    {
        var delay = Random.Range(2f, 5f);
        var index = GetRandomIndex();
        yield return new WaitForSeconds(delay);
        if (dancingCouples[index].IsInProgress())
        {
            StartCoroutine(StartKissing());
            yield break;
        }
        dancingCouples[index].TryToKiss();
        yield return null;
        isActivityStarted = true;
        StartCoroutine(StartKissing());
    }
    private int GetRandomIndex()
    {
        var r = Random.Range(0, dancingCouples.Length);
        while (dancingCouples[r].IsInProgress())
        {
            r = Random.Range(0, dancingCouples.Length);
        }
        return r;
    }
    public bool IsActivityFinished()
    {
        return isActivityFinished;
    }
    public void ShowPerfects(Vector3 newPosition)
    {
        perfects.transform.position = new Vector3(newPosition.x, perfects.transform.position.y, newPosition.z);
        perfects.SetActive(true);  
    }
}