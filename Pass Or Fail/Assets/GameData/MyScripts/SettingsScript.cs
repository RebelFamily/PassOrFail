using System;
using UnityEngine;
public class SettingsScript : MonoBehaviour
{
    [SerializeField] private RectTransform[] buttons;
    private const string Off = "off", On = "on", Cross = "Cross", Tick = "Tick";
    private void OnEnable()
    {
        RefreshSoundSettings();
        RefreshMusicSettings();
        RefreshVibrationSettings();
    }
    public void SoundToggle()
    {
        SoundController.Instance.PlayBtnClickSound();
        PlayerPrefsHandler.SetSoundControllerBool(PlayerPrefsHandler.Sound, !PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound));
        RefreshSoundSettings();
        if (!PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound))
        {
            SoundController.Instance.StopSpecialSounds();
        }
    }
    public void MusicToggle()
    {
        SoundController.Instance.PlayBtnClickSound();
        if (PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Music))
        {
            PlayerPrefsHandler.SetSoundControllerBool(PlayerPrefsHandler.Music, false);
            SoundController.Instance.MuteBackgroundMusic();
        }
        else
        {
            PlayerPrefsHandler.SetSoundControllerBool(PlayerPrefsHandler.Music, true);
            if(GamePlayManager.Instance)
                SoundController.Instance.PlayGamePlayBackgroundMusic();
            else
                SoundController.Instance.PlayMetaBackgroundMusic();
        }
        RefreshMusicSettings();
    }
    public void VibrationToggle()
    {
        SoundController.Instance.PlayBtnClickSound();
        if (PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Vibration))
        {
            PlayerPrefsHandler.SetSoundControllerBool(PlayerPrefsHandler.Vibration, false);
        }
        else
        {
            PlayerPrefsHandler.SetSoundControllerBool(PlayerPrefsHandler.Vibration, true);
            SoundController.Instance.vibrationManager.TapVibrate();
        }
        RefreshVibrationSettings();
    }
    private void RefreshSoundSettings()
    {
        if (PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound))
        {
            SetButtonsEffect(1, On);
        }
        else
        {
            SetButtonsEffect(1, Off);
        }
    }
    private void RefreshMusicSettings()
    {
        if (PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Music))
        {
            SetButtonsEffect(0, On);
        }
        else
        {
            SetButtonsEffect(0, Off);
        }
    }
    private void RefreshVibrationSettings()
    {
        if (PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Vibration))
        {
            SetButtonsEffect(2, On);
        }
        else
        {
            SetButtonsEffect(2, Off);
        }
    }
    private void SetButtonsEffect(int index, string action = On)
    {
        if (action == On)
        {
            buttons[index].Find(Cross).gameObject.SetActive(false);
            buttons[index].Find(Tick).gameObject.SetActive(true);
        }
        else
        {
            buttons[index].Find(Cross).gameObject.SetActive(true);
            buttons[index].Find(Tick).gameObject.SetActive(false);
        }
    }
}