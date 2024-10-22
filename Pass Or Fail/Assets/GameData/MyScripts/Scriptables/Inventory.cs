using System;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "Inventory", menuName = "ScriptableObjects/Inventory", order = 1)]
public class Inventory : ScriptableObject
{
    public enum CustomizationType
    {
        Teachers,
        Students
    }
    public enum ItemType
    {
        Common,
        Rare,
        Epic,
        Decorate,
        Upgrade
    }
    [Serializable] public class Item
    {
        public int itemId;
        public string itemName;
        public bool canUnlockByAd = false;
        public Sprite itemIcon;
        public int itemPrice;
        public GameObject itemPrefab;
        public ItemType itemType;
    }
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Item[] characters, commonItems, rareItems, epicItems, decorationItems, upgradeItems;
    [SerializeField] private Sprite buttonSimpleSprite, buttonAdSprite;
    public GameObject GetButtonPrefab()
    {
        return buttonPrefab;
    }
    public Item[] GetItems(CustomizationType customizationType, ItemType itemsType = ItemType.Common)
    {
        switch (customizationType)
        {
            case CustomizationType.Teachers:
                return characters;
            case CustomizationType.Students:
                switch (itemsType)
                {
                    case ItemType.Common:
                        return commonItems;
                    case ItemType.Rare:
                        return rareItems;
                    case ItemType.Epic:
                        return epicItems;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(itemsType), itemsType, null);
                }
            default:
                throw new ArgumentOutOfRangeException(nameof(customizationType), customizationType, null);
        }
        return characters;
    }
    public Sprite GetSimpleBtnSprite()
    {
        return buttonSimpleSprite;
    }
    public Sprite GetAdBtnSprite()
    {
        return buttonAdSprite;
    }
    public Item GetLastStudentProp(int itemId)
    {
        foreach (var t in commonItems)
        {
            if (itemId == t.itemId)
                return t;
        }
        foreach (var t in rareItems)
        {
            if (itemId == t.itemId)
                return t;
        }
        foreach (var t in epicItems)
        {
            if (itemId == t.itemId)
                return t;
        }
        return null;
    }
}