using Lean.Pool;
using UnityEngine;

namespace Zain_Meta.Meta_Scripts.Managers
{
    public class ParticlesManager : MonoBehaviour
    {
        public static ParticlesManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField] private ParticleSystem teacherUnlockVfx;

        public void PlayTeacherUnlockVfxFor(Transform teacherPivot)
        {
            var particles = LeanPool.Spawn(teacherUnlockVfx, teacherPivot);
            particles.transform.localPosition = Vector3.zero;
            particles.Play();
        }
    }
}