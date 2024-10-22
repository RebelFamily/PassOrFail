using UnityEngine;
using UnityEngine.UI;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public class AdmissionReception : MonoBehaviour, IReception
    {
        [SerializeField] private CashGenerationSystem myCashGeneration;
        [SerializeField] private bool hasReceptionist;
        [SerializeField] private Collider collisionTrigger;
        [SerializeField] private Image receptionServingFiller;
        [SerializeField] private float servingDelay;
        [SerializeField] private ReceptionPoint[] queuePoints;
        private bool _isPlayerTriggering;
        private float _curTimerToServe;

        private void Start()
        {
            collisionTrigger.enabled = !hasReceptionist;
        }


        private void Update()
        {
            if (hasReceptionist)
                ServeByHelper();
            else
                ServeByPlayer();
        }


        private void ServeByPlayer()
        {
            if (!CanStudentBeAdmitted())
            {
                receptionServingFiller.fillAmount = 0;
                return;
            }

            if (_curTimerToServe < .1f)
            {
                _curTimerToServe = servingDelay;
                myCashGeneration.AddCash(1, queuePoints[0].transform);
            }
            else
            {
                _curTimerToServe -= Time.deltaTime;
            }

            var normalValue = Mathf.InverseLerp(servingDelay, 0, _curTimerToServe);
            receptionServingFiller.fillAmount = normalValue;
        }

        private void ServeByHelper()
        {
        }

        public void StartServing()
        {
            if (hasReceptionist) return;
            _isPlayerTriggering = true;
            receptionServingFiller.fillAmount = 0;
            _curTimerToServe = servingDelay;
        }

        public void StopServing()
        {
            if (hasReceptionist) return;
            _isPlayerTriggering = false;
            receptionServingFiller.fillAmount = 0;
        }

        private bool CanStudentBeAdmitted()
        {
            if (!_isPlayerTriggering) return false;

            if (!queuePoints[0].IsOccupied()) return false;

            // check for if all the classes are filled and there is no space left
            return true;
        }
    }
}