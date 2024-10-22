using System.Collections;
using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.PlayerRelated;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public class TriggerHandler : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform, playerDriftPos, moveOutPos;
        [SerializeField] private float changeInSize;

        private IReception _receptionProfile;

        //  private PlayerMovement _player;
        private Vector3 _actualSize;
        private bool _hasTriggered;
        private readonly YieldInstruction _delay = new WaitForSeconds(.5f);

        private void Start()
        {
            _actualSize = targetTransform.localScale;
            _receptionProfile = GetComponent<IReception>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ArcadeMovement player))
            {
                DOTween.Kill(targetTransform);

                targetTransform.DOScale(_actualSize, 0).SetEase(Ease.Linear);
                targetTransform.DOScale(changeInSize, .25f);
                _receptionProfile.StartServing();
                //  EventsManager.PlayerInTrigger(true);
                StartCoroutine(nameof(Delay_CO), player);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ArcadeMovement player))
            {
                DOTween.Kill(targetTransform);
                StopCoroutine(nameof(Delay_CO));
                targetTransform.DOScale(_actualSize, 0.1f).SetEase(Ease.Linear);
                _receptionProfile.StopServing();
                _hasTriggered = false;
                //  EventsManager.PlayerInTrigger(false);
            }
        }

        private IEnumerator Delay_CO(ArcadeMovement player)
        {
            yield return _delay;

            if (_hasTriggered) yield break;

            _hasTriggered = true;
            /*player.curTriggerTarget = playerDriftPos;
            player.isInATrigger = true;
            player.StopController(false);
            player.transform.DORotateQuaternion(playerDriftPos.rotation, .15f).SetEase(Ease.Linear);
            var position = playerDriftPos.position;
            player.transform.DOMoveX(position.x, .15f).SetEase(Ease.Linear);
            player.transform.DOMoveZ(position.z, .15f).SetEase(Ease.Linear).OnComplete(() =>
            {
                player.StopController(true);
            });*/
        }
    }
}