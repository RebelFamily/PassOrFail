using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.Components;
using Zain_Meta.Meta_Scripts.DataRelated;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    public class PlayerStackingSystem : MonoBehaviour
    {
        [SerializeField] private Transform stackingPos;
        [SerializeField] private float stackingDelay;
        [SerializeField] private ArcadeMovement movement;
        [SerializeField] private Animator playerAnim;
        [SerializeField] private StackOffsetData stackOffsetData;
        [SerializeField] private Ease easeType;
        [SerializeField] private GameObject maxLimitText;
        [SerializeField] private List<Transform> itemsStacked = new();
        [SerializeField] private int stackLimit;
        private Vector3 _positioningVector;
        private float _curY;

        private bool _isTeaching;
        private readonly YieldInstruction _delay = new WaitForSeconds(.25f);
        private readonly YieldInstruction _delayLong = new WaitForSeconds(.5f);

        public void StartStacking(StackingHandler stackingHandler)
        {
            StartCoroutine(nameof(Stacking_CO), stackingHandler);
        }

        public void StartDropping(StackingHandler stackingHandler)
        {
            StartCoroutine(nameof(UnstackingAll_CO), stackingHandler);
        }

        private void CheckForItemsInStack()
        {
            playerAnim.SetLayerWeight(1, itemsStacked.Count > 0 ? 1 : 0);
            maxLimitText.SetActive(itemsStacked.Count >= stackLimit);
        }

        private void OnEnable()
        {
            EventsManager.OnTriggerTeaching += HideCoffeeStack;
        }

        private void OnDisable()
        {
            EventsManager.OnTriggerTeaching -= HideCoffeeStack;
        }

        private void HideCoffeeStack(bool toHide, Vector3 arg2, Vector3 arg3, ClassroomProfile arg4)
        {
            if (toHide)
            {
                stackingPos.gameObject.SetActive(false);
                _isTeaching = true;
                playerAnim.SetLayerWeight(1, 0);
                maxLimitText.SetActive(false);
            }
            else
            {
                stackingPos.gameObject.SetActive(true);
                _isTeaching = false;
            }
        }

        private void LateUpdate()
        {
            if (_isTeaching) return;
            CheckForItemsInStack();
        }

        private IEnumerator Stacking_CO(StackingHandler handler)
        {
            while (handler.isPlayerTriggering)
            {
                //check if player has capacity
                if (itemsStacked.Count >= stackLimit)
                {
                    maxLimitText.SetActive(true);
                    break;
                }

                maxLimitText.SetActive(false);
                if (handler.HasItemsInStack())
                {
                    var lastItemInHandler = handler.GetLastStackedItem();
                    if (lastItemInHandler)
                    {
                        itemsStacked.Add(lastItemInHandler);
                        lastItemInHandler.transform.parent = stackingPos;
                        DOTween.Kill(lastItemInHandler);
                        lastItemInHandler.transform.DOLocalMove(_positioningVector, stackingDelay)
                            .SetEase(easeType);
                        _positioningVector.y += stackOffsetData.yOffset;
                        lastItemInHandler.transform.DOLocalRotate(Vector3.zero, 0);
                        lastItemInHandler.transform.DOScale(Vector3.one, 0);
                    }
                }

                yield return null;
            }
        }

        private IEnumerator UnstackingAll_CO(StackingHandler handler)
        {
            while (handler.isPlayerTriggering)
            {
                if (!movement.IsMoving())
                {
                    if (handler.isReadyToAccept)
                    {
                        maxLimitText.SetActive(false);
                        if (itemsStacked.Count <= 0)
                        {
                            _positioningVector.y = 0;
                            break;
                        }

                        if (!handler.IsStackFull())
                        {
                            var itemInStack = GetLastItem();
                            handler.AddToStack(itemInStack);
                        }

                        yield return _delay;
                    }
                }
                else
                {
                    yield return _delayLong;
                }

                yield return null;
                if (!handler.isPlayerTriggering)
                    break;
            }
        }

        private Transform GetLastItem()
        {
            var lastItem = itemsStacked[itemsStacked.Count - 1];
            itemsStacked.RemoveAt(itemsStacked.Count - 1);
            _positioningVector.y -= stackOffsetData.yOffset;
            if (_positioningVector.y < 0)
                _positioningVector.y = 0;
            return lastItem;
        }
    }
}