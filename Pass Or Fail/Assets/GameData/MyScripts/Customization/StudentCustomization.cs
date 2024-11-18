using System;
using UnityEngine;
public class StudentCustomization : MonoBehaviour
{
    [SerializeField] private int studentId = 0;
    [SerializeField] private Transform propParent;
    private GameObject _currentProp;
    [SerializeField] private PropPosition[] propPositions;
    [SerializeField] private bool inCustomization = false;
    private Inventory _inventory;
    private Animator _animator;
    private static readonly int Apply = Animator.StringToHash("Apply");
    /*private void Start()
    {
        if (!GamePlayManager.Instance) return;
        _inventory = GamePlayManager.Instance.environmentManager.GetInventory();
        var propNo = PlayerPrefsHandler.GetStudentCurrentProp(studentId);
        if(propNo == -1) return;
        ApplyProp(_inventory.GetLastStudentProp(propNo));
    }*/
    private GameObject GetPropIfExists(Inventory.Item prop)
    {
        if (propParent.childCount == 0) return null;
        for (var i = 0; i < propParent.childCount; i++)
        {
            var prop1 = propParent.GetChild(i);
            if (prop1.name.Contains(prop.itemName))
                return prop1.gameObject;
        }
        return null;
    }
    public void ApplyProp(Inventory.Item prop)
    {
        if (prop == null)
        {
            if(_currentProp) _currentProp.SetActive(false);
            _currentProp = null;
            return;
        }
        if(_currentProp) _currentProp.SetActive(false);
        var tempProp = GetPropIfExists(prop);
        if (tempProp)
        {
            _currentProp = tempProp;
            _currentProp.SetActive(true);
        }
        else
        {
            _currentProp = Instantiate(prop.itemPrefab, propParent);
        }
        var propPosition = propPositions[prop.itemId];
        _currentProp.transform.localPosition = propPosition.pos;
        _currentProp.transform.localScale = propPosition.size;
        if (inCustomization)
        {
            SoundController.Instance.PlayBtnClickSound();
            if (!_animator) _animator = GetComponent<Animator>();
            _animator.SetTrigger(Apply);
        }
    }
    [Serializable]
    public class PropPosition
    {
        public Vector3 pos;
        public Vector3 size;
    }
}