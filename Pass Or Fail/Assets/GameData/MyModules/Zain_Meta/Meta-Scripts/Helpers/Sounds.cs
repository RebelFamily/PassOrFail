using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Helpers
{
    [System.Serializable]
    public class Sounds
    {
        public string name;

        public AudioClip[] clip;
        public AudioSource source;

        [Range(0f, 1f)] public float volume = 0.5f;

        [
            Range(-3f, 3f)]
        public float pitch;

        public bool loop;
        public bool useRandomClip;
    }
}