using UnityEngine;

namespace Attractor.Scripts
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleAttractorSpherical : MonoBehaviour
    {
        private ParticleSystem ps;
        private ParticleSystem.Particle[] mParticles;
        public Transform target;
        public float speed = 5f;
        private int numParticlesAlive;
        public bool follow;

        private void Start()
        {
            ps = GetComponent<ParticleSystem>();
            if (!GetComponent<Transform>())
            {
                GetComponent<Transform>();
            }
        }

        private void LateUpdate()
        {
            if (!target) return;
            if (!follow) return;
            mParticles = new ParticleSystem.Particle[ps.main.maxParticles];
            numParticlesAlive = ps.GetParticles(mParticles);
            var step = speed * Time.deltaTime;
            for (var i = 0; i < numParticlesAlive; i++)
            {
                mParticles[i].position = Vector3.SlerpUnclamped(mParticles[i].position, target.position, step);
            }

            ps.SetParticles(mParticles, numParticlesAlive);
        }
    }
}