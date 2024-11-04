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
        private bool inPark, isMuted;

        private void Awake()
        {
            Instance = this;
        }

        public void PlaySound(string sound)
        {
            if (isMuted) return;
            var s = Array.Find(sounds, item => item.name == sound);
            s.source.loop = s.loop;
            s.source.clip = !s.useRandomClip ? s.clip[0] : s.clip[Random.Range(0, s.clip.Length)];
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            if (s.source.isPlaying) return;
            s.source.Play();
        }
        
        
        private bool GetSound(string sound)
        {
            var s = Array.Find(sounds, item => item.name == sound);
            return s.source.isPlaying;
        }
    }
}