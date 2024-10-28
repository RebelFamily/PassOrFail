using Attractor.Scripts;
using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    public class PlayerCashSystem : MonoBehaviour
    {
        [SerializeField] private ArcadeMovement arcadeController;
        [SerializeField] private ParticleAttractorSpherical cashParticles;
        [SerializeField] private Transform particleAttractionPos;
        public Transform spawnPos;
        private CashManager _cashManager;
        private float _curSmoothTimer;
        
        private void Start()
        {
            _cashManager = CashManager.Instance;
        }


        public void AddCashToPlayerStack(int len)
        {
            PlayCashVfx(len);
            DOVirtual.DelayedCall(.75f, () => { StackingCoroutine(len); });
        }

        private void PlayCashVfx(int emissionCount)
        {
            cashParticles.follow = false;
            cashParticles.target = spawnPos;
            cashParticles.transform.position = particleAttractionPos.position;
            var particle = cashParticles.GetComponent<ParticleSystem>();
            var cashEmission = particle.emission.GetBurst(0);
            /*if (emissionCount > emissionCountLimit)
                emissionCount = emissionCountLimit;*/
            cashEmission.count = emissionCount;
            particle.emission.SetBurst(0, cashEmission);
            particle.Play();
            DOVirtual.DelayedCall(.5f, () => { cashParticles.follow = true; });
        }

        private void StackingCoroutine(int len)
        {
            Vibration.VibratePop();
            _cashManager.AddCash(5 * len);
        }

        public ArcadeMovement GetController() => arcadeController;
    }
}