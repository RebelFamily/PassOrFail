using System.Collections;
using UnityEngine;
public class PianoKey : MonoBehaviour
{
    [SerializeField] private int keyNo = 0;
    [SerializeField] private PianoLesson.PianoKeySide side;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Animator highlighter;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Color highlightColor = Color.red;
    private bool _showHighlighter = false;
    private float lerpSpeed = 1f;
    public int GetKeyNo()
    {
        return keyNo;
    }
    public PianoLesson.PianoKeySide GetPianoKeySide()
    {
        return side;
    }
    public void OnPressingKey()
    {
        //if (!audioSource.isPlaying)
            audioSource.Play();
    }
    public void HighlightKey(bool flag)
    {
        _showHighlighter = flag;
        if (flag)
        {
            //highlighter.enabled = true;
            //meshRenderer.material.color = Color.red;
            StartCoroutine(UpdateColor());
        }
        /*else
        {
            //highlighter.enabled = false;
            
            Invoke(nameof(SetDefaultColor), 1f);
        }*/
    }
    private void SetDefaultColor()
    {
        meshRenderer.material.color = Color.white;
    }
    private IEnumerator UpdateColor()
    {
        var lerpedColor = Color.white;
        var defaultColor = Color.white;
        float currentTime = 0;
        while (_showHighlighter)
        {
            lerpedColor = Color.Lerp(defaultColor, highlightColor,
                Mathf.PingPong(currentTime += (Time.deltaTime * lerpSpeed / 1), 1));
            meshRenderer.material.SetColor("_BaseColor", lerpedColor);
            yield return null;
        }
        meshRenderer.material.SetColor("_BaseColor", defaultColor);
    }
}