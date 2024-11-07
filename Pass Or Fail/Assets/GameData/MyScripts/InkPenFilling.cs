using DG.Tweening;
using UnityEngine;
public class InkPenFilling : MonoBehaviour, IMiniGame, IMiniGameInput
{
    private int _penIndex = 0;
    [SerializeField] private Pen[] pens;
    [SerializeField] private Transform[] pensDefaultPoint;
    [SerializeField] private Transform penReadyPoint, penFillingPoint;
    [SerializeField] private Animator inkPot;
    [SerializeField] private GameObject canvas, perfects, indications;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AudioSource audioSource;
    public void StartMiniGame()
    {
        Invoke(nameof(GetReadyToFill), 1f);
        SharedUI.Instance.gamePlayUIManager.controls.EnableProgressBar(true);
    }
    public void EndMiniGame()
    {
        GamePlayManager.Instance.LevelComplete(0.5f);
    }
    private void EnableCanvas(bool flag)
    {
        canvas.SetActive(flag);
    }
    private void GetReadyToFill()
    {
        var t = pens[_penIndex].transform;
        t.parent = penReadyPoint;
        t.DOLocalMove(Vector3.zero, 0.25f).OnComplete(() =>
        {
            EnableCanvas(true);
            pens[_penIndex].RemovePenCover();
        });
        t.DOLocalRotate(Vector3.zero, 0.25f);
    }
    public void EndPen()
    {
        var t = pens[_penIndex].transform;
        t.DOKill();
        pens[_penIndex].EnableAnimator(false);
        pens[_penIndex].AddBackPenCover();
        particles.Stop();
        EnableCanvas(false);
        t.parent = pensDefaultPoint[_penIndex];
        _penIndex++;
        Invoke(nameof(IsMiniGameEnded), 0.5f);
        t.DOLocalMove(Vector3.zero, 0.25f);
        t.DOLocalRotate(Vector3.zero, 0.25f);
    }
    private void IsMiniGameEnded()
    {
        SharedUI.Instance.gamePlayUIManager.controls.SetProgress();
        if (_penIndex > 2)
        {
            EndMiniGame();
            return;
        }
        GetReadyToFill();
    }
    private void FillPen()
    {
        var t = pens[_penIndex].transform;
        t.DOKill();
        t.parent = penFillingPoint;
        t.DOLocalMove(Vector3.zero, 0.25f).OnComplete(() =>
        {
            //sharpener.SetBool(Sharp, true);
            pens[_penIndex].EnableAnimator(true);
            particles.Play();
            PlayAudio(true);
        });
        t.DOLocalRotate(Vector3.zero, 0.25f);
    }
    private void ReleasePen()
    {
        var t = pens[_penIndex].transform;
        t.DOKill();
        pens[_penIndex].EnableAnimator(false);
        particles.Stop();
        PlayAudio(false);
        if (pens[_penIndex].IsPenFilled())
        {
            EnableCanvas(false);
            perfects.SetActive(true);
            pens[_penIndex].AddBackPenCover();
            t.parent = pensDefaultPoint[_penIndex];
            _penIndex++;
            Invoke(nameof(IsMiniGameEnded), 0.5f);
        }
        else
        {
            indications.SetActive(true);
            t.parent = penReadyPoint;
        }
        t.DOLocalMove(Vector3.zero, 0.25f);
        t.DOLocalRotate(Vector3.zero, 0.25f);
    }
    public void MiniGameMouseDown()
    {
        FillPen();
    }
    public void MiniGameMouseUp()
    {
        ReleasePen();
    }
    private void PlayAudio(bool flag)
    {
        if(!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound)) return;
        if(flag)
            audioSource.Play();
        else
            audioSource.Pause();
    }
}