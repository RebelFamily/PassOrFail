using System;
using UnityEngine;
public class StudentCustomization : MonoBehaviour
{
    [SerializeField] private int studentId = 0;
    [SerializeField] private Transform propParent;
    private GameObject currentProp;
    [SerializeField] private PropPosition[] propPositions;
    [SerializeField] private bool inCustomization = false;
    private Inventory inventory;
    private Animator animator;
    private static readonly int Apply = Animator.StringToHash("Apply");

    private void Start()
    {
        if (!GamePlayManager.Instance) return;
        inventory = GamePlayManager.Instance.environmentManager.GetInventory();
        var propNo = PlayerPrefsHandler.GetStudentCurrentProp(studentId);
        if(propNo == -1) return;
        ApplyProp(inventory.GetLastStudentProp(propNo));
    }
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
            if(currentProp) currentProp.SetActive(false);
            currentProp = null;
            return;
        }
        if(currentProp) currentProp.SetActive(false);
        var tempProp = GetPropIfExists(prop);
        if (tempProp)
        {
            currentProp = tempProp;
            currentProp.SetActive(true);
        }
        else
        {
            currentProp = Instantiate(prop.itemPrefab, propParent);
        }
        var propPosition = propPositions[prop.itemId];
        currentProp.transform.localPosition = propPosition.pos;
        currentProp.transform.localScale = propPosition.size;
        if (inCustomization)
        {
            SoundController.Instance.PlayBtnClickSound();
            if (!animator) animator = GetComponent<Animator>();
            animator.SetTrigger(Apply);
        }
    }
    [Serializable]
    public class PropPosition
    {
        public Vector3 pos;
        public Vector3 size;
    }
}