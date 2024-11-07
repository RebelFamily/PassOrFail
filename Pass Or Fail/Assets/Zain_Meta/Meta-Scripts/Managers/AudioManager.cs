using System;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Helpers;
using Random = UnityEngine.Random;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public Sounds[] sounds;
        private bool _isMuted;

        private void Awake()
        {
            Instance = this;
          //  _isMuted = !PlayerPrefsHandler.GetSoundControllerBool(PlayerPrefsHandler.Sound);
        }

        public void PlaySound(string sound)
        {
            if (_isMuted) return;
            var s = Array.Find(sounds, item => item.name == sound);
            s.source.loop = s.loop;
            s.source.clip = !s.useRandomClip ? s.clip[0] : s.clip[Random.Range(0, s.clip.Length)];
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            if (s.source.isPlaying) return;
            s.source.Play();
        }
    }
}