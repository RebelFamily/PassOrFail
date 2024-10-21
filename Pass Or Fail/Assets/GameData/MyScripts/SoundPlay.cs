using UnityEngine;
public class SoundPlay : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private void OnEnable()
    {
        if(!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound)) return;
        audioSource.Play();
    }
}