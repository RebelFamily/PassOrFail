using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class CharactersCustomization : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    private readonly List<GameObject> itemButtons = new List<GameObject>();
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private GameObject confirmationPopup;
    private Inventory.CustomizationType currentCustomizationType;
    private Inventory.ItemType currentItemType;
    private Inventory.Item currentItem;
    private List<Inventory.Item> items = new List<Inventory.Item>();
    private int characterIndex = 0;
    [SerializeField] private GameObject customizationArea, classRoom, subCategoriesButtons0, subCategoriesButtons1;
    [SerializeField] private Transform teachers, students, studentsCamera, studentsCameraPositions, classRoomCamera;
    [SerializeField] private Button nextCharacterBtn, previousCharacterBtn;
    [SerializeField] private GameObject nameBar;
    private static readonly string[] StudentsNames = new[] {"Robin", "Tommy", "Nami", "Lucky", "Mano", "Rocky", "Kate", "Jack"};
    private int itemIndexToUnlock = 0, itemPrice = 0, buttonIndex = 0;
    [SerializeField] private StudentCustomization currentStudent;
    [SerializeField] private ClassRoomCustomization classRoomCustomization;
    private Text nameText;
    private Canvas canvas;
    private void Start()
    {
        PlayerPrefsHandler.UnlockTeacher(PlayerPrefsHandler.currentTeacher);
    }
    private void OnEnable()
    {
        if (!canvas)
            canvas = GetComponentInParent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Callbacks.OnRewardItem += RewardItem;
    }
    private void OnDisable()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Callbacks.OnRewardItem -= RewardItem;
    }
    public void SelectCategory(string category)
    {
        switch (category)
        {
            case "Teachers":
                customizationArea.SetActive(true);
                classRoom.SetActive(false);
                subCategoriesButtons0.SetActive(false);
                subCategoriesButtons1.SetActive(false);
                SelectCategory(Inventory.CustomizationType.Teachers);
                break;
            case "Students":
                customizationArea.SetActive(true);
                classRoom.SetActive(false);
                subCategoriesButtons0.SetActive(true);
                subCategoriesButtons1.SetActive(false);
                SelectCategory(Inventory.CustomizationType.Students);
                break;
        }
    }
    private void SelectCategory(Inventory.CustomizationType category, Inventory.ItemType itemType = Inventory.ItemType.Common)
    {
        currentCustomizationType = category;
        switch (category)
        {
            case Inventory.CustomizationType.Teachers:
                nameBar.SetActive(false);
                characterIndex = PlayerPrefsHandler.currentTeacher;
                teachers.gameObject.SetActive(true);
                students.gameObject.SetActive(false);
                studentsCamera.position = studentsCameraPositions.GetChild(0).position;
                EnableATeacher();
                break;
            case Inventory.CustomizationType.Students:
                currentItemType = itemType;
                nameBar.SetActive(true);
                characterIndex = 0;
                if(!nameText)
                    nameText = nameBar.transform.Find("NameText").GetComponent<Text>();
                nameText.text = StudentsNames[characterIndex];
                teachers.gameObject.SetActive(false);
                students.gameObject.SetActive(true);
                studentsCamera.position = studentsCameraPositions.GetChild(0).position;
                SetupCharacter();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        SetUI();
    }
    private void EnableATeacher()
    {
        for (var i = 0; i < teachers.childCount; i++)
        {
            teachers.GetChild(i).gameObject.SetActive(false);
        }
        teachers.GetChild(characterIndex).gameObject.SetActive(true);
    }
    private void SetUI(bool selectItem = false)
    {
        var prefab = inventory.GetButtonPrefab();
        items.Clear();
        items = inventory.GetItems(currentCustomizationType, currentItemType).ToList();
        for (var i = 0; i < items.Count; i++)
        {
            var item = items[i];
            GameObject btn;
            if (itemButtons.Count - 1 >= i)
            {
                btn = itemButtons[i];
            }
            else
            {
                btn = Instantiate(prefab, scrollView.content);
                itemButtons.Add(btn);
            }
            btn.transform.Find("SelectedText").gameObject.SetActive(false);
            var btnComponent = btn.GetComponent<Button>();
            btnComponent.onClick.RemoveAllListeners();
            btn.transform.Find("Render").GetComponent<Image>().sprite = item.itemIcon;
            if (IsItemLocked(i))
            {
                if (item.canUnlockByAd)
                {
                    btn.GetComponent<Image>().sprite = inventory.GetAdBtnSprite();
                    var txt = btn.transform.Find("PriceText");
                    txt.gameObject.SetActive(false);
                    var i1 = i;
                    btnComponent.onClick.AddListener(() =>
                    {
                        UnlockingConfirmation(item.itemId, i1);
                    });
                }
                else
                {
                    btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                    var txt = btn.transform.Find("PriceText");
                    txt.GetComponent<Text>().text = item.itemPrice.ToString();
                    txt.gameObject.SetActive(true);
                    var i1 = i;
                    btnComponent.onClick.AddListener(() =>
                    {
                        BuyingConfirmation(item.itemId, i1);
                    });
                }
            }
            else
            {
                btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                var selectedText = btn.transform.Find("SelectedText");
                selectedText.gameObject.SetActive(false);
                btn.transform.Find("SelectText").gameObject.SetActive(true);
                btn.transform.Find("PriceText").gameObject.SetActive(false);
                var i1 = i;
                btnComponent.onClick.AddListener(() =>
                {
                    SelectItem(item.itemId, i1, true);
                });
            }
            itemButtons[i].SetActive(true);
        }
        if (itemButtons.Count > items.Count)
        {
            for (var i = items.Count; i < itemButtons.Count; i++)
            {
                itemButtons[i].gameObject.SetActive(false);
            }   
        }
        //itemButtons[PlayerPrefsHandler.currentTeacher].GetComponent<Image>().sprite = inventory.GetSelectedBgSprite();
        //Debug.Log(selectItem + " : " + currentCustomizationType + " : " + currentItemType);
        if (selectItem)
        {
            switch (currentCustomizationType)
            {
                case Inventory.CustomizationType.Teachers:
                    SelectItem(PlayerPrefsHandler.currentTeacher);
                    break;
                case Inventory.CustomizationType.Students:
                    var index = PlayerPrefsHandler.GetStudentCurrentProp(characterIndex);
                    if (index == -1)
                        break;
                    currentStudent.ApplyProp(GetItemToSelect(index));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
    public void SelectItem(int id, int index = 0, bool refreshText = false)
    {
        //Debug.Log("SelectItem: " + index);
        switch (currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                characterIndex = id;
                EnableATeacher();
                break;
            case Inventory.CustomizationType.Students:
                currentItem = GetItemToSelect(id);
                currentStudent.ApplyProp(currentItem);
                if (refreshText)
                {
                    itemIndexToUnlock = id;
                    buttonIndex = index;
                    Invoke(nameof(SetTexts), 0.05f);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        /*HideAllSelectedTexts();
        if (!selectedText) return;
        selectedText.transform.parent.Find("SelectText").gameObject.SetActive(false);
        selectedText.gameObject.SetActive(true);*/
    }
    private void SetTexts()
    {
        Debug.Log(buttonIndex + " : " + currentCustomizationType + " : " + itemIndexToUnlock);
        //if(buttonIndex == -1) buttonIndex = 0;
        var limit = items.Count;
        for (var i = 0; i < limit; i++)
        {
            itemButtons[i].transform.Find("SelectedText").gameObject.SetActive(false);
            var selectText = itemButtons[i].transform.Find("SelectText").gameObject;
            if (IsItemLocked(i))
            {
                selectText.SetActive(false);
            }
            else
            {
                selectText.SetActive(true);
            }
        }
        var btn = itemButtons[buttonIndex].transform;
        var btnComponent = btn.GetComponent<Button>();
        btnComponent.onClick.RemoveAllListeners();
        if (IsItemLocked(buttonIndex))
        {
            btn.Find("SelectedText").gameObject.SetActive(false);
            btn.Find("SelectText").gameObject.SetActive(false);
            itemIndexToUnlock = items[buttonIndex].itemId;
            if(currentItem == null) return;
            if (currentItem.canUnlockByAd)
            {
                btn.GetComponent<Image>().sprite = inventory.GetAdBtnSprite();
                var txt = btn.transform.Find("PriceText");
                txt.gameObject.SetActive(false);
                btnComponent.onClick.AddListener(() =>
                {
                    UnlockingConfirmation(itemIndexToUnlock, buttonIndex);
                });
            }
            else
            {
                btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                var txt = btn.transform.Find("PriceText");
                txt.GetComponent<Text>().text = items[buttonIndex].itemPrice.ToString();
                txt.gameObject.SetActive(true);
                btnComponent.onClick.AddListener(() =>
                {
                    BuyingConfirmation(itemIndexToUnlock, buttonIndex);
                });
            }
        }
        else
        {
            btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
            var selectedText = btn.transform.Find("SelectedText");
            selectedText.gameObject.SetActive(true);
            btn.transform.Find("SelectText").gameObject.SetActive(false);
            btn.transform.Find("PriceText").gameObject.SetActive(false);
            btnComponent.onClick.AddListener(() =>
            {
                SelectItem(itemIndexToUnlock, buttonIndex, true);
            });
        }
    }
    private bool IsItemLocked(int index)
    {
        switch (currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                return PlayerPrefsHandler.IsTeacherLocked(items[index].itemId);
            case Inventory.CustomizationType.Students:
                return PlayerPrefsHandler.IsStudentPropLocked(characterIndex, items[index].itemId);
            default:
                return false;
        }
    }
    private Inventory.Item GetItemToSelect(int id)
    {
        //Debug.Log("GetItemToSelect: " + id + " : " + items.Count);
        foreach (var t in items)
        {
            if (t.itemId == id)
                return t;
        }
        return null;
    }
    private Inventory.Item GetLastItem(int id)
    {
        var items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Common);
        foreach (var t in items)
        {
            if (t.itemId == id)
                return t;
        }
        items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Rare);
        foreach (var t in items)
        {
            if (t.itemId == id)
                return t;
        }
        items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Epic);
        foreach (var t in items)
        {
            if (t.itemId == id)
                return t;
        }
        return null;
    }
    private int GetItemId(int index, Inventory.ItemType itemType = Inventory.ItemType.Common)
    {
        return items[index].itemId;;
    }
    public void NextCharacter()
    {
        SoundController.Instance.PlayBtnClickSound();
        if (characterIndex < studentsCameraPositions.childCount - 1)
        {
            characterIndex++;
            nextCharacterBtn.interactable = false;
            previousCharacterBtn.interactable = false;
            currentStudent = students.GetChild(characterIndex).GetComponent<StudentCustomization>();
        }
        else
            return;
        studentsCamera.DOMove(studentsCameraPositions.GetChild(characterIndex).position, 0.5f).OnComplete(() =>
        {
            nameText.text = StudentsNames[characterIndex];
            nextCharacterBtn.interactable = true;
            previousCharacterBtn.interactable = true;
        });
        SetupCharacter();
    }
    public void PreviousCharacter()
    {
        SoundController.Instance.PlayBtnClickSound();
        if (characterIndex > 0)
        {
            characterIndex--;
            nextCharacterBtn.interactable = false;
            previousCharacterBtn.interactable = false;
            currentStudent = students.GetChild(characterIndex).GetComponent<StudentCustomization>();
        }
        else
            return;
        studentsCamera.DOMove(studentsCameraPositions.GetChild(characterIndex).position, 0.5f).OnComplete(() =>
        {
            nameText.text = StudentsNames[characterIndex];
            nextCharacterBtn.interactable = true;
            previousCharacterBtn.interactable = true;
        });
        SetupCharacter();
    }
    private void SetupCharacter()
    {
        var currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(characterIndex);
        Debug.Log("currentPropNo: " + currentPropNo);
        switch (currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                break;
            case Inventory.CustomizationType.Students:
                if (currentPropNo != -1)
                {
                    itemIndexToUnlock = currentPropNo;
                    items.Clear();
                    items = inventory.GetItems(currentCustomizationType, currentItemType).ToList();
                    currentItem = GetItemToSelect(currentPropNo);
                    if (currentItem == null) // item does not exists in the current items list
                    {
                        currentStudent.ApplyProp(GetLastItem(currentPropNo));
                        buttonIndex = 0;
                        itemIndexToUnlock = 0;
                    }
                    else
                    {
                        buttonIndex = items.IndexOf(currentItem);
                        currentStudent.ApplyProp(currentItem);
                    }
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Invoke(nameof(SetTexts), 0.05f);
    }
    private void UnlockingConfirmation(int itemIndex, int btnIndex)
    {
        Debug.Log(itemIndex + " :UnlockingConfirmation : " + btnIndex);
        SoundController.Instance.PlayBtnClickSound();
        itemIndexToUnlock = itemIndex;
        buttonIndex = btnIndex;
        currentItem = GetItemToSelect(itemIndex);
        confirmationPopup.transform.Find("CashIcon").gameObject.SetActive(false);
        confirmationPopup.transform.Find("AdIcon").gameObject.SetActive(true);
        var btn = confirmationPopup.transform.Find("Buy").GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(WatchRewardedAdToUnlockItem);
        confirmationPopup.SetActive(true);
        scrollView.gameObject.SetActive(false);
        SelectItem(itemIndex);
    }
    private void WatchRewardedAdToUnlockItem()
    {
        //Debug.Log("WatchRewardedAdToUnlockPen");
        SoundController.Instance.PlayBtnClickSound();
        Callbacks.rewardType = Callbacks.RewardType.RewardItem;
        AdsCaller.Instance.ShowRewardedAd();
    }
    private void BuyingConfirmation(int itemIndex, int btnIndex)
    {
        Debug.Log(itemIndex + " :BuyingConfirmation : " + btnIndex);
        SoundController.Instance.PlayBtnClickSound();
        itemIndexToUnlock = itemIndex;
        buttonIndex = btnIndex;
        currentItem = GetItemToSelect(itemIndex);
        Debug.Log(currentItem.itemName);
        confirmationPopup.transform.Find("CashIcon").gameObject.SetActive(true);
        confirmationPopup.transform.Find("AdIcon").gameObject.SetActive(false);
        confirmationPopup.transform.Find("CashIcon/PriceText").GetComponent<Text>().text = currentItem.itemPrice.ToString();
        var btn = confirmationPopup.transform.Find("Buy").GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(BuyItem);
        confirmationPopup.SetActive(true);
        scrollView.gameObject.SetActive(false);
        SelectItem(itemIndex);
    }
    private void BuyItem()
    {
        Debug.Log("BuyItem");
        if (currentItem.itemPrice <= PlayerPrefsHandler.currency)
        {
            CurrencyCounter.Instance.DeductCurrency(currentItem.itemPrice, itemButtons[buttonIndex].transform);
            UnlockItem();
        }
    }
    private void UnlockItem()
    {
        //Debug.Log(itemIndexToUnlock + " : " + currentCustomizationType + " : " + currentItemType);
        switch (currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                PlayerPrefsHandler.UnlockTeacher(itemIndexToUnlock);
                break;
            case Inventory.CustomizationType.Students:
                PlayerPrefsHandler.UnlockStudentProp(characterIndex, itemIndexToUnlock);
                PlayerPrefsHandler.SetStudentCurrentProp(characterIndex, itemIndexToUnlock);
                Invoke(nameof(SetTexts), 0.05f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        confirmationPopup.SetActive(false);
        scrollView.gameObject.SetActive(true);
        SoundController.Instance.PlayBuySound();
        SetUI(true);
    }
    private void RewardItem()
    {
        Debug.Log("RewardItem");
        UnlockItem();
    }
    public void SelectSubCategory(string subCategory)
    {
        SoundController.Instance.PlayBtnClickSound();
        if (subCategory == Inventory.ItemType.Common.ToString())
        {
            if (currentItemType != Inventory.ItemType.Common)
            {
                items.Clear();
                items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Common).ToList();
            }
            currentItemType = Inventory.ItemType.Common;
            SetUI(true);
            Invoke(nameof(SetTexts), 0.05f);
        }
        else if (subCategory == Inventory.ItemType.Rare.ToString())
        {
            if (currentItemType != Inventory.ItemType.Rare)
            {
                items.Clear();
                items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Rare).ToList();
            }
            currentItemType = Inventory.ItemType.Rare;
            SetUI(true);
            Invoke(nameof(SetTexts), 0.05f);
        }
        else if (subCategory == Inventory.ItemType.Epic.ToString())
        {
            if (currentItemType != Inventory.ItemType.Epic)
            {
                items.Clear();
                items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Epic).ToList();
            }
            currentItemType = Inventory.ItemType.Epic;
            SetUI(true);
            Invoke(nameof(SetTexts), 0.05f);
        }
        else if (subCategory == Inventory.ItemType.Decorate.ToString())
        {
            if (currentItemType != Inventory.ItemType.Decorate)
            {
                items.Clear();
                items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Decorate).ToList();
            }
            currentItemType = Inventory.ItemType.Decorate;
            SetUI(true);
            Invoke(nameof(SetTexts), 0.05f);
        }
        else
        {
            if (currentItemType != Inventory.ItemType.Upgrade)
            {
                items.Clear();
                items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Upgrade).ToList();
            }
            currentItemType = Inventory.ItemType.Upgrade;
            SetUI(true);
            //Invoke(nameof(SetTexts), 0.5f);
        }
    }
    public void CloseTheCustomization()
    {
        SoundController.Instance.PlayBtnClickSound();
        studentsCamera.GetComponent<Animator>().Play($"CustomizationCameraExit");
        classRoomCamera.GetComponent<Animator>().Play($"CustomizationCameraExit");
    }
    public void CloseTheMenu()
    {
        SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.MainMenu);
        customizationArea.SetActive(false);
        classRoom.SetActive(false);
    }
    public void CloseTheConfirmationPopup()
    {
        SoundController.Instance.PlayBtnClickSound();
        scrollView.gameObject.SetActive(true);
        confirmationPopup.SetActive(false);
        var currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(characterIndex);
        switch (currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(characterIndex);
                break;
            case Inventory.CustomizationType.Students:
                currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(characterIndex);
                if (currentPropNo != -1)
                {
                    itemIndexToUnlock = currentPropNo;
                    items.Clear();
                    items = inventory.GetItems(currentCustomizationType, currentItemType).ToList();
                    currentItem = GetItemToSelect(currentPropNo);
                    if (currentItem == null) // item does not exists in the current items list
                    {
                        currentStudent.ApplyProp(GetLastItem(currentPropNo));
                        buttonIndex = 0;
                        itemIndexToUnlock = 0;
                    }
                    else
                    {
                        buttonIndex = items.IndexOf(currentItem);
                        currentStudent.ApplyProp(currentItem);
                    }
                    
                    Debug.Log(buttonIndex + " :currentProp: " + currentPropNo + " : " /*+ currentItem.itemName*/);
                }
                else
                {
                    currentItem = null;
                    buttonIndex = 0;
                    itemIndexToUnlock = 0;
                    currentStudent.ApplyProp(null);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}