using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Zain_Meta.Meta_Scripts.DataRelated;

namespace Zain_Meta.Meta_Scripts.PlayerRelated
{
    public class StackingHandler : MonoBehaviour
    {
        public List<Transform> stackedItems = new();
        [SerializeField] private Transform stackPivot;
        [SerializeField] private float tweenDelay;
        [SerializeField] private StackOffsetData offsetData;
        [SerializeField] private bool hasLimit;
        [SerializeField] private bool hideAfterReachingLimit;
        [SerializeField] private int limitAmount;
        [SerializeField] private UnityEvent actionAfterAddition, actionAfterRemoval;
        public bool isReadyToAccept;
        private float _curXPos, _curYPos, _curZPos;

        private Vector3 _positionVector;
        public bool isPlayerTriggering;

        public void AddToStack(Transform stackingItem)
        {
            stackedItems.Add(stackingItem);
            _positionVector.x = _curXPos;
            _positionVector.y = _curYPos;
            _positionVector.z = _curZPos;
            stackingItem.transform.parent = stackPivot;
            stackingItem.DOLocalMove(_positionVector, tweenDelay).OnComplete(() => { actionAfterAddition?.Invoke(); });
            stackingItem.DOLocalRotate(Vector3.zero, 0).SetEase(Ease.Linear);
            stackingItem.DOScale(Vector3.one, 0).SetEase(Ease.Linear);
            _curYPos += offsetData.yOffset;
            if (IsStackFull())
                isReadyToAccept = false;
        }

        public Transform GetLastStackedItem()
        {
            var lastItem = stackedItems[^1];
            stackedItems.RemoveAt(stackedItems.Count - 1);

            _curYPos -= offsetData.yOffset;

            actionAfterRemoval?.Invoke();
            return lastItem;
        }

        public bool HasItemsInStack()
        {
            if (stackedItems.Count > 0) return true;
            return false;
        }

        public bool IsStackFull()
        {
            if (!hasLimit) return false;
            return stackedItems.Count >= limitAmount;
        }
    }
}