using DG.Tweening;
using UnityEngine;
public class PencilSharpener : MonoBehaviour, IMiniGame, IMiniGameInput, IMiniGameUI
{
    private int _pencilIndex = 0;
    [SerializeField] private Pencil[] pencils;
    [SerializeField] private Transform[] pencilsDefaultPoint;
    [SerializeField] private Transform pencilReadyPoint, pencilSharpingPoint;
    [SerializeField] private Animator sharpener;
    [SerializeField] private GameObject canvas, perfects, indications;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AudioSource audioSource;
    private static readonly int Sharp = Animator.StringToHash("Sharp");
    public void StartMiniGame()
    {
        Invoke(nameof(GetReadyToSharp), 1f);
        SharedUI.Instance.gamePlayUIManager.controls.EnableProgressBar(true);
    }
    public void EndMiniGame()
    {
        //Debug.Log("Ending Pencil Sharpener Mini Game");
        GamePlayManager.Instance.LevelComplete(0.5f);
    }
    public void EnableCanvas(bool flag)
    {
        canvas.SetActive(flag);
    }
    private void GetReadyToSharp()
    {
        var t = pencils[_pencilIndex].transform;
        t.parent = pencilReadyPoint;
        t.DOLocalMove(Vector3.zero, 0.25f).OnComplete(() =>
        {
            EnableCanvas(true);
        });
        t.DOLocalRotate(Vector3.zero, 0.25f);
    }
    private void SharpPencil()
    {
        var t = pencils[_pencilIndex].transform;
        t.DOKill();
        t.parent = pencilSharpingPoint;
        t.DOLocalMove(Vector3.zero, 0.25f).OnComplete(() =>
        {
            sharpener.SetBool(Sharp, true);
            pencils[_pencilIndex].EnableAnimator(true);
            particles.Play();
            PlayAudio(true);
        });
        t.DOLocalRotate(Vector3.zero, 0.25f);
    }
    private void ReleasePencil()
    {
        var t = pencils[_pencilIndex].transform;
        t.DOKill();
        sharpener.SetBool(Sharp, false);
        pencils[_pencilIndex].EnableAnimator(false);
        particles.Stop();
        PlayAudio(false);
        //Debug.Log("IsPencilSharped: " + pencils[_pencilIndex].IsPencilSharped());
        if (pencils[_pencilIndex].IsPencilSharped())
        {
            EnableCanvas(false);
            perfects.SetActive(true);
            t.parent = pencilsDefaultPoint[_pencilIndex];
            _pencilIndex++;
            Invoke(nameof(IsMiniGameEnded), 0.5f);
        }
        else
        {
            indications.SetActive(true);
            t.parent = pencilReadyPoint;
        }
        t.DOLocalMove(Vector3.zero, 0.25f);
        t.DOLocalRotate(Vector3.zero, 0.25f);
    }
    public void EndPencil()
    {
        var t = pencils[_pencilIndex].transform;
        t.DOKill();
        sharpener.SetBool(Sharp, false);
        pencils[_pencilIndex].EnableAnimator(false);
        particles.Stop();
        EnableCanvas(false);
        t.parent = pencilsDefaultPoint[_pencilIndex];
        _pencilIndex++;
        Invoke(nameof(IsMiniGameEnded), 0.5f);
        t.DOLocalMove(Vector3.zero, 0.25f);
        t.DOLocalRotate(Vector3.zero, 0.25f);
    }
    private void IsMiniGameEnded()
    {
        SharedUI.Instance.gamePlayUIManager.controls.SetProgress();
        if (_pencilIndex > 2)
        {
            EndMiniGame();
            return;
        }
        GetReadyToSharp();
    }
    public void MiniGameMouseDown()
    {
        //Debug.Log("Pencil Sharpener Mini Game Mouse Down");
        SharpPencil();
    }
    public void MiniGameMouseUp()
    {
        //Debug.Log("Pencil Sharpener Mini Game Mouse Up");
        ReleasePencil();
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