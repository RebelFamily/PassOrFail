using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zain_Meta.Meta_Scripts.DataRelated;

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
        [SerializeField] private float delayOnFirstPick;
        private Vector3 _positioningVector;

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

        private void LateUpdate()
        {
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
                        lastItemInHandler.transform.DOLocalMove(_positioningVector, stackingDelay)
                            .SetEase(easeType);
                        lastItemInHandler.transform.DOLocalRotate(Vector3.zero, 0);
                        lastItemInHandler.transform.DOScale(Vector3.one, 0);
                        _positioningVector.y += stackOffsetData.yOffset;
                    }
                }

                yield return _delay;
            }
        }

        private IEnumerator UnstackingAll_CO(StackingHandler handler)
        {
            yield return new WaitForSeconds(delayOnFirstPick);
            while (handler.isPlayerTriggering)
            {
                if (!movement.IsMoving())
                {
                    if (handler.isReadyToAccept)
                    {
                        maxLimitText.SetActive(false);
                        if (itemsStacked.Count <= 0)
                        {
                            break;
                        }

                        // remove item from player stack and give it to handler         

                        if (!handler.IsStackFull())
                        {
                            var itemInStack = GetLastItem();

                            handler.AddToStack(itemInStack);
                            _positioningVector.y -= stackOffsetData.yOffset;
                            if (_positioningVector.y < 0)
                                _positioningVector.y = 0;
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
            return lastItem;
        }
    }
}