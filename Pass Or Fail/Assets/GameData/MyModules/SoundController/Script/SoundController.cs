using UnityEngine;
using Sirenix.OdinInspector;
public class SoundController : MonoBehaviour
{
    [HideInInspector] public VibrationManager vibrationManager;
    public static SoundController Instance;
    private void Awake()
    {
        if (Instance != null)
            return;
        Instance = this;
        DontDestroyOnLoad (gameObject);
        vibrationManager = GetComponent<VibrationManager>();
    }
    [TabGroup("AudioClips")]
    [AssetsOnly]
    [InlineEditor(InlineEditorModes.SmallPreview)]
    [SerializeField] private AudioClip btnClickSound, metaBackgroundMusic, gamePlayBackgroundMusic, buySound,
        gameCompleteSound, winSound, correctGradingSound, wrongGradingSound, danceMusic, fillingSound;
    [TabGroup("AudioSources"), Required]
    [SceneObjectsOnly]
    [SerializeField] private AudioSource soundAudioSource, bgAudioSource, specialBgAudioSource;
    public void PlayMetaBackgroundMusic()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Music))
            return;
        bgAudioSource.clip = metaBackgroundMusic;
        bgAudioSource.volume = 0.2f;
        bgAudioSource.Play ();
    }
    public void PlayGamePlayBackgroundMusic()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Music))
            return;
        bgAudioSource.clip = gamePlayBackgroundMusic;
        bgAudioSource.volume = 0.6f;
        bgAudioSource.Play ();
    }
	public void MuteBackgroundMusic(){
		bgAudioSource.clip = null;
		bgAudioSource.Stop ();
	}
    public void PlayBtnClickSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound))
            return;
        soundAudioSource.clip = btnClickSound;
        soundAudioSource.Play ();
    }
    public void PlayFillingSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound))
            return;
        if(soundAudioSource.isPlaying) return;
        soundAudioSource.clip = fillingSound;
        soundAudioSource.Play ();
    }
    public void PlayBuySound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound))
            return;
        soundAudioSource.clip = buySound;
        soundAudioSource.Play ();
    }
    public void PlayWinSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound))
            return;
        soundAudioSource.clip = winSound;
        soundAudioSource.Play ();
        vibrationManager.TapPeekVibrate();
    }
    public void PlayCorrectGradingSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound))
            return;
        soundAudioSource.clip = correctGradingSound;
        soundAudioSource.Play ();
        vibrationManager.TapPeekVibrate();
    }
    public void PlayWrongGradingSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound))
            return;
        soundAudioSource.clip = wrongGradingSound;
        soundAudioSource.Play ();
        vibrationManager.TapPeekVibrate();
    }
    public void PlayGameCompleteSound()
    {
        if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound))
            return;
        soundAudioSource.clip = gameCompleteSound;
        soundAudioSource.Play();
        vibrationManager.TapVibrate();
    }
    public void PlayDanceMusic()
    {
        if(!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Music)) return;
        bgAudioSource.clip = danceMusic;
        bgAudioSource.volume = 0.4f;
        bgAudioSource.Play ();
    }
    public void StopSpecialSounds()
    {
        if(!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound)) return;
        specialBgAudioSource.Stop();
    }
    public void SetSpecialSoundsVolume(float vol)
    {
        specialBgAudioSource.volume = vol;
    }
}