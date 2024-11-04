using System.Collections.Generic;
using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using Zain_Meta.Meta_Scripts.DataRelated;
using Zain_Meta.Meta_Scripts.Helpers;
using Zain_Meta.Meta_Scripts.Managers;

namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public class CashGenerationSystem : MonoBehaviour
    {
        [SerializeField] private Utility utility;
        [SerializeField] private int itemName;
        [SerializeField] private bool keepPersistent;
        public int amountToGive;
        public List<Transform> cashMade = new();
        [SerializeField] private bool useVertical;
        [SerializeField] private CashOffsetData offsetData;
        [SerializeField] private Transform cashStackPos;
        private List<Vector3> _nullItemsPos = new();
        private float curXPos, curYPos, curZPos;
        private float yOffset, xOffset, zOffset;
        private float maxXVal, maxZVal;

        private int _nullListIndex;
        private Vector3 _dispersalVector;

        private void Awake()
        {
            xOffset = offsetData.xOffset;
            yOffset = offsetData.yOffset;
            zOffset = offsetData.zOffset;
            maxXVal = offsetData.maxXVal;
            maxZVal = offsetData.maxZVal;
            LoadData();
        }
        
        private void Update()
            {
                if(Input.GetKeyDown(KeyCode.A))
                {
                    AddCash(1,transform);
                }
            }

        private void LoadData()
        {
            if (!keepPersistent) return;
            var previousCashCount = PlayerPrefs.GetInt("cash" + itemName, 0);
            amountToGive = PlayerPrefs.GetInt("AmountToGive"+ itemName, amountToGive);
            for (var i = 0; i < previousCashCount; i++)
            {
                var cash = utility.SpawnCashAt(transform);
                cash.myCashSystem = this;
                cash.myAmount = amountToGive;
                cashMade.Add(cash.transform);
                _dispersalVector.x = curXPos;
                _dispersalVector.y = curYPos;
                _dispersalVector.z = curZPos;
                var transform1 = cash.transform;
                transform1.parent = cashStackPos;
                transform1.localPosition = _dispersalVector;
                transform1.localEulerAngles = Vector3.zero;
                if (useVertical)
                {
                    curYPos += yOffset;
                }
                else
                {
                    curZPos += zOffset;
                    if (curZPos >= maxZVal)
                    {
                        curZPos = 0;
                        curXPos += xOffset;
                        if (curXPos >= maxXVal)
                        {
                            curXPos = 0;
                            curYPos += yOffset;
                        }
                    }
                }
            }
        }

        private void SaveData()
        {
            PlayerPrefs.SetInt("cash" + itemName, cashMade.Count);
            PlayerPrefs.SetInt("AmountToGive" + itemName, amountToGive);
        }

        public void AddCash(int amount,Transform spawningPos)
        {
            SpawnCash(amount,spawningPos);
        }
        
        private void SpawnCash(int amount, Transform spawningPos)
        {
            for (var i = 0; i < amount; i++)
                AddCashStack(spawningPos);
        }
        
        private void AddCashStack(Transform spawnPos)
        {
            var cash = utility.SpawnCashAt(spawnPos);
            cash.myCashSystem = this;
            cash.myAmount = amountToGive;
            var transform1 = cash.transform;
            transform1.parent = cashStackPos;
            cashMade.Add(transform1);
            _dispersalVector.x = curXPos;
            _dispersalVector.y = curYPos;
            _dispersalVector.z = curZPos;
            cash.transform.DOLocalRotate(Vector3.zero, 0).SetEase(Ease.Linear);
            if (_nullItemsPos.Count != 0)
            {
                cash.transform.DOLocalMove(_nullItemsPos[0], .25f).SetEase(Ease.InOutSine);
                _nullItemsPos.RemoveAt(0);
                return;
            }

            cash.transform.DOLocalMove(_dispersalVector, .25f).SetEase(Ease.InOutSine);

            if (useVertical)
            {
                curYPos += yOffset;
            }
            else
            {
                curZPos += zOffset;
                if (curZPos >= maxZVal)
                {
                    curZPos = 0;
                    curXPos += xOffset;
                    if (curXPos >= maxXVal)
                    {
                        curXPos = 0;
                        curYPos += yOffset;
                    }
                }
            }

            if (keepPersistent)
                SaveData();
        }

        public void RemoveItemFromList(Transform item)
        {
            var index = cashMade.IndexOf(item);
            if (index != -1)
            {
                cashMade.RemoveAt(index);
                _nullItemsPos.Add(item.localPosition);

                if (cashMade.Count == 0)
                {
                    curXPos = curYPos = curZPos = 0;
                    _nullItemsPos.Clear();
                }

                LeanPool.Despawn(item);
                if (OnBoardingManager.TutorialComplete) return;
                OnBoardingManager.Instance.SetStateBasedOn(TutorialState.PickReceptionCash
                    , TutorialState.UnlockReceptionist);
            }

            SaveData();
        }
    }
}