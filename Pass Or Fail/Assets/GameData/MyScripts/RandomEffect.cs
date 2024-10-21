using System.Collections;
using UnityEngine;
public class RandomEffect : MonoBehaviour
{
    [SerializeField] private bool playSoundEffect = true;
    [SerializeField] private float delayToDisable = 2f;
    private int effectIndex = 0;
    private void OnEnable()
    {
        effectIndex = Random.Range(0, transform.childCount);
        transform.GetChild(effectIndex).gameObject.SetActive(true);
        if(playSoundEffect)
            SoundController.Instance.PlayWinSound();
        StartCoroutine(DelayToDisable());
    }
    private IEnumerator DelayToDisable()
    {
        yield return new WaitForSeconds(delayToDisable);
        transform.GetChild(effectIndex).gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}