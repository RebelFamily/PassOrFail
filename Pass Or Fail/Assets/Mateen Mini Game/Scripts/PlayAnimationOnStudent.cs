using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace PassOrFail.MiniGames
{
    [RequireComponent(typeof(Student))]
    public class PlayAnimationOnStudent : MonoBehaviour
    {
        private Student _student;
        [SerializeField] private Vector3 targetTurnRotation;
        [SerializeField] private Vector3 targetPosition;
        private bool _isPlayerGetSprayReward;
        private static readonly int Spray = Animator.StringToHash("Spray");
        private static readonly int Stop = Animator.StringToHash("Stop");

        private void Awake()
        {
            _student = GetComponent<Student>();
        }

        private void OnEnable()
        {
            Callbacks.OnRewardSpray += ReceiveSprayReward;
            EventManager.OnStopPlayerFight += StopPlayerFight;
        }

        private void OnDisable()
        {
            Callbacks.OnRewardSpray -= ReceiveSprayReward;
            EventManager.OnStopPlayerFight -= StopPlayerFight;
        }

        private void StopPlayerFight()
        {
            StartCoroutine(StopFight());
        }

        private IEnumerator StopFight()
        {
            if (_isPlayerGetSprayReward)
            {
                yield return new WaitForSeconds(1f);
                _student.GetAnimator().SetTrigger(Spray);
                Invoke(nameof(TurnPlayer),1.75f);
            }
            else
            {
                _student.GetAnimator().SetTrigger(Stop);
                Invoke(nameof(TurnPlayer),.75f);
                //PlayStopAnimation();
            }
        }
        private void ReceiveSprayReward()
        {
            _isPlayerGetSprayReward = true;
           
        }
        public void SetTrigger(string trigger)
        {
            _student.GetAnimator().SetTrigger(trigger);
            Invoke(nameof(TurnPlayer),.75f);
        }

        private void TurnPlayer()
        {
            transform.DOLocalRotate(targetTurnRotation, 1f).OnComplete((() =>
            {
                transform.DOLocalMove(targetPosition, 4f);
            }));
        }
    }
}