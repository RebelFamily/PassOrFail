using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class Customization : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    private readonly List<GameObject> itemButtons = new List<GameObject>();
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private GameObject confirmationPopup;
    private Inventory.CustomizationType currentCustomizationType;
    private Inventory.ItemType currentItemType;
    private Inventory.Item currentItem, lastItem;
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
    [SerializeField] private Color[] subCategoriesColors;
    private Text nameText;
    private Canvas canvas;
    private void OnEnable()
    {
        if (!canvas)
            canvas = GetComponentInParent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Callbacks.OnRewardItem += RewardItem;
        SharedUI.Instance.metaUIManager.EnableSchools(false);
    }
    private void OnDisable()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        Callbacks.OnRewardItem -= RewardItem;
    }
    public Inventory GetInventory()
    {
        return inventory;
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
            case "ClassRoom":
                customizationArea.SetActive(false);
                classRoom.SetActive(true);
                subCategoriesButtons0.SetActive(false);
                subCategoriesButtons1.SetActive(true);
                SelectCategory(Inventory.CustomizationType.ClassRoom, Inventory.ItemType.Decorate);
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
                Invoke(nameof(SetTeachersUI), 0.1f);
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
                Invoke(nameof(InvokeSetupCharacter), 0.1f);
                break;
            case Inventory.CustomizationType.ClassRoom:
                currentItemType = itemType;
                nameBar.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        scrollView.GetComponent<Image>().color = subCategoriesColors[0];
        SetUI();
    }
    private void SetUI()
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
            btn.transform.Find("EquippedText").gameObject.SetActive(false);
            btn.transform.Find("SelectText").gameObject.SetActive(false);
            var btnComponent = btn.GetComponent<Button>();
            btnComponent.onClick.RemoveAllListeners();
            btn.transform.Find("Render").GetComponent<Image>().sprite = item.itemIcon;
            //Debug.Log("item: " + item.itemName + " : " + IsItemLocked(item));
            if (IsItemLocked(item))
            {
                if (item.canUnlockByAd)
                {
                    btn.GetComponent<Image>().sprite = inventory.GetAdBtnSprite();
                    var txt = btn.transform.Find("PriceText");
                    txt.gameObject.SetActive(false);
                    btnComponent.onClick.AddListener(() =>
                    {
                        UnlockingConfirmation(item);
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
                        BuyingConfirmation(item);
                    });
                }
            }
            else
            {
                btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                if (currentCustomizationType != Inventory.CustomizationType.ClassRoom)
                {
                    btn.transform.Find("SelectedText").gameObject.SetActive(false);
                    btn.transform.Find("SelectText").gameObject.SetActive(true);
                    btn.transform.Find("EquippedText").gameObject.SetActive(false);
                }
                else
                {
                    btn.transform.Find("EquippedText").gameObject.SetActive(true);
                    btn.transform.Find("SelectedText").gameObject.SetActive(false);
                    btn.transform.Find("SelectText").gameObject.SetActive(false);
                }
                btn.transform.Find("PriceText").gameObject.SetActive(false);
                btnComponent.onClick.AddListener(() =>
                {
                    SelectItem(item);
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
    }
    private void SetTeachersUI()
    {
        var btn = itemButtons[PlayerPrefsHandler.currentTeacher].transform;
        btn.transform.Find("SelectedText").gameObject.SetActive(true);
        btn.transform.Find("SelectText").gameObject.SetActive(false);
    }
    private void SelectItem(Inventory.Item newItem, bool calledByUnlockItem = false)
    {
        //Debug.Log("SelectItem: " + newItem.itemName);
        currentItem = newItem;
        switch (currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                characterIndex = newItem.itemId;
                EnableATeacher();
                for (var i = 0; i < items.Count; i++)
                {
                    var btn = itemButtons[i].GetComponent<Button>();
                    btn.transform.Find("EquippedText").gameObject.SetActive(false);
                    if (IsItemLocked(items[i]))
                    {
                        btn.transform.Find("SelectedText").gameObject.SetActive(false);
                        btn.transform.Find("SelectText").gameObject.SetActive(false);
                        if (calledByUnlockItem)
                        {
                            btn.onClick.RemoveAllListeners();
                            btn.GetComponent<Image>().sprite = inventory.GetAdBtnSprite();
                            btn.transform.Find("PriceText").gameObject.SetActive(false);
                            var i1 = i;
                            btn.onClick.AddListener(() => UnlockingConfirmation(items[i1]));
                        }
                    }
                    else
                    {
                        btn.transform.Find("SelectedText").gameObject.SetActive(false);
                        btn.transform.Find("SelectText").gameObject.SetActive(true);
                        if (calledByUnlockItem)
                        {
                            btn.transform.Find("PriceText").gameObject.SetActive(false);
                            btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                            btn.onClick.RemoveAllListeners();
                            var i1 = i;
                            btn.onClick.AddListener(() => SelectItem(items[i1]));
                        }
                    }
                }
                if (!IsItemLocked(currentItem))
                {
                    var btn = itemButtons[items.IndexOf(currentItem)].GetComponent<Button>();
                    btn.transform.Find("SelectedText").gameObject.SetActive(true);
                    btn.transform.Find("SelectText").gameObject.SetActive(false);
                    btn.transform.Find("PriceText").gameObject.SetActive(false);
                    btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                    PlayerPrefsHandler.currentTeacher = currentItem.itemId;
                }
                break;
            case Inventory.CustomizationType.Students:
                currentStudent.ApplyProp(currentItem);
                for (var i = 0; i < items.Count; i++)
                {
                    var btn = itemButtons[i].GetComponent<Button>();
                    btn.transform.Find("EquippedText").gameObject.SetActive(false);
                    if (IsItemLocked(items[i]))
                    {
                        btn.transform.Find("SelectedText").gameObject.SetActive(false);
                        btn.transform.Find("SelectText").gameObject.SetActive(false);
                        if (calledByUnlockItem)
                        {
                            btn.onClick.RemoveAllListeners();
                            if (items[i].canUnlockByAd)
                            {
                                btn.GetComponent<Image>().sprite = inventory.GetAdBtnSprite();
                                btn.transform.Find("PriceText").gameObject.SetActive(false);
                                var i1 = i;
                                btn.onClick.AddListener(() => UnlockingConfirmation(items[i1]));
                            }
                            else
                            {
                                btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                                btn.transform.Find("PriceText").gameObject.SetActive(true);
                                var i1 = i;
                                btn.onClick.AddListener(() => BuyingConfirmation(items[i1]));
                            }
                        }
                    }
                    else
                    {
                        btn.transform.Find("SelectedText").gameObject.SetActive(false);
                        btn.transform.Find("SelectText").gameObject.SetActive(true);
                        if (calledByUnlockItem)
                        {
                            btn.transform.Find("PriceText").gameObject.SetActive(false);
                            btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                            btn.onClick.RemoveAllListeners();
                            var i1 = i;
                            btn.onClick.AddListener(() => SelectItem(items[i1]));
                        }
                    }
                }

                if (currentItem != null)
                {
                    if (!IsItemLocked(currentItem))
                    {
                        var index = items.IndexOf(currentItem);
                        //Debug.Log("index: " + index);
                        if (index == -1)
                        {
                            return;
                        }

                        var btn = itemButtons[items.IndexOf(currentItem)].GetComponent<Button>();
                        btn.transform.Find("SelectedText").gameObject.SetActive(true);
                        btn.transform.Find("SelectText").gameObject.SetActive(false);
                        btn.transform.Find("PriceText").gameObject.SetActive(false);
                        btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                        lastItem = currentItem;
                        PlayerPrefsHandler.SetStudentCurrentProp(characterIndex, currentItem.itemId);
                    }
                }
                else
                    Debug.Log("Current Item is Null");
                break;
            case Inventory.CustomizationType.ClassRoom:
                classRoomCustomization.ApplyProp(currentItem);
                for (var i = 0; i < items.Count; i++)
                {
                    var btn = itemButtons[i].GetComponent<Button>();
                    btn.transform.Find("SelectedText").gameObject.SetActive(false);
                    btn.transform.Find("SelectText").gameObject.SetActive(false);
                    if (IsItemLocked(items[i]))
                    {
                        btn.transform.Find("EquippedText").gameObject.SetActive(false);
                        if (calledByUnlockItem)
                        {
                            btn.onClick.RemoveAllListeners();
                            if (items[i].canUnlockByAd)
                            {
                                btn.GetComponent<Image>().sprite = inventory.GetAdBtnSprite();
                                btn.transform.Find("PriceText").gameObject.SetActive(false);
                                var i1 = i;
                                btn.onClick.AddListener(() => UnlockingConfirmation(items[i1]));
                            }
                            else
                            {
                                btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                                btn.transform.Find("PriceText").gameObject.SetActive(true);
                                var i1 = i;
                                btn.onClick.AddListener(() => BuyingConfirmation(items[i1]));
                            }
                        }
                    }
                    else
                    {
                        btn.transform.Find("EquippedText").gameObject.SetActive(true);
                        if (calledByUnlockItem)
                        {
                            btn.transform.Find("PriceText").gameObject.SetActive(false);
                            btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                            btn.onClick.RemoveAllListeners();
                        }
                    }
                }
                break;
        }
    }
    private bool IsItemLocked(Inventory.Item item)
    {
        switch (currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                return PlayerPrefsHandler.IsTeacherLocked(item.itemId);
            case Inventory.CustomizationType.Students:
                return PlayerPrefsHandler.IsStudentPropLocked(characterIndex, item.itemId);
            case Inventory.CustomizationType.ClassRoom:
                return PlayerPrefsHandler.IsClassPropLocked(item.itemId);
            default:
                return false;
        }
    }
    private void UnlockingConfirmation(Inventory.Item newItem)
    {
        Debug.Log(newItem.itemId + " :UnlockingConfirmation: ");
        SoundController.Instance.PlayBtnClickSound();
        currentItem = newItem;
        confirmationPopup.transform.Find("CashIcon").gameObject.SetActive(false);
        confirmationPopup.transform.Find("AdIcon").gameObject.SetActive(true);
        var btn = confirmationPopup.transform.Find("Buy").GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(WatchRewardedAdToUnlockItem);
        confirmationPopup.SetActive(true);
        if(currentCustomizationType == Inventory.CustomizationType.Students)
            nameBar.SetActive(false);
        scrollView.gameObject.SetActive(false);
        SelectItem(newItem);
    }
    private void WatchRewardedAdToUnlockItem()
    {
        //Debug.Log("WatchRewardedAdToUnlockPen");
        SoundController.Instance.PlayBtnClickSound();
        Callbacks.rewardType = Callbacks.RewardType.RewardItem;
        AdsCaller.Instance.ShowRewardedAd();
    }
    private void BuyingConfirmation(Inventory.Item newItem)
    {
        Debug.Log(newItem.itemId + " :BuyingConfirmation : ");
        SoundController.Instance.PlayBtnClickSound();
        currentItem = newItem;
        confirmationPopup.transform.Find("CashIcon").gameObject.SetActive(true);
        confirmationPopup.transform.Find("AdIcon").gameObject.SetActive(false);
        confirmationPopup.transform.Find("CashIcon/PriceText").GetComponent<Text>().text = currentItem.itemPrice.ToString();
        var btn = confirmationPopup.transform.Find("Buy").GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(BuyItem);
        confirmationPopup.SetActive(true);
        if(currentCustomizationType == Inventory.CustomizationType.Students)
            nameBar.SetActive(false);
        scrollView.gameObject.SetActive(false);
        SelectItem(newItem);
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
        Debug.Log(itemIndexToUnlock + " : " + currentCustomizationType + " : " + currentItemType);
        lastItem = currentItem;
        switch (currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                PlayerPrefsHandler.UnlockTeacher(currentItem.itemId);
                SelectItem(currentItem, true);
                break;
            case Inventory.CustomizationType.Students:
                PlayerPrefsHandler.UnlockStudentProp(characterIndex, currentItem.itemId);
                PlayerPrefsHandler.SetStudentCurrentProp(characterIndex, currentItem.itemId);
                SelectItem(currentItem, true);
                break;
            case Inventory.CustomizationType.ClassRoom:
                PlayerPrefsHandler.UnlockClassProp(currentItem.itemId);
                SelectItem(currentItem, true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        confirmationPopup.SetActive(false);
        if(currentCustomizationType == Inventory.CustomizationType.Students)
            nameBar.SetActive(true);
        scrollView.gameObject.SetActive(true);
        SoundController.Instance.PlayBuySound();
    }
    private void RewardItem()
    {
        //Debug.Log("RewardItem");
        //UnlockItem();
        Invoke(nameof(UnlockItem), 0.5f);
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
                scrollView.GetComponent<Image>().color = subCategoriesColors[0];
            }
            currentItemType = Inventory.ItemType.Common;
        }
        else if (subCategory == Inventory.ItemType.Rare.ToString())
        {
            if (currentItemType != Inventory.ItemType.Rare)
            {
                items.Clear();
                items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Rare).ToList();
                scrollView.GetComponent<Image>().color = subCategoriesColors[1];
            }
            currentItemType = Inventory.ItemType.Rare;
        }
        else if (subCategory == Inventory.ItemType.Epic.ToString())
        {
            if (currentItemType != Inventory.ItemType.Epic)
            {
                items.Clear();
                items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Epic).ToList();
                scrollView.GetComponent<Image>().color = subCategoriesColors[2];
            }
            currentItemType = Inventory.ItemType.Epic;
        }
        else if (subCategory == Inventory.ItemType.Decorate.ToString())
        {
            if (currentItemType != Inventory.ItemType.Decorate)
            {
                items.Clear();
                items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Decorate).ToList();
                scrollView.GetComponent<Image>().color = subCategoriesColors[0];
            }
            currentItemType = Inventory.ItemType.Decorate;
        }
        else
        {
            if (currentItemType != Inventory.ItemType.Upgrade)
            {
                items.Clear();
                items = inventory.GetItems(currentCustomizationType, Inventory.ItemType.Upgrade).ToList();
                scrollView.GetComponent<Image>().color = subCategoriesColors[1];
            }
            currentItemType = Inventory.ItemType.Upgrade;
        }
        SetUI();
        SelectItem(lastItem);
    }
    private void EnableATeacher()
    {
        for (var i = 0; i < teachers.childCount; i++)
        {
            teachers.GetChild(i).gameObject.SetActive(false);
        }
        teachers.GetChild(characterIndex).gameObject.SetActive(true);
    }
    private void InvokeSetupCharacter()
    {
        SetupCharacter();   
    }
    private void SetupCharacter(bool newCharacter = false)
    {
        var currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(characterIndex);
        switch (currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                break;
            case Inventory.CustomizationType.Students:
                currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(characterIndex);
                if (currentPropNo != -1)
                {
                    lastItem = inventory.GetLastStudentProp(currentPropNo);
                    //Debug.Log("LastItem: " + lastItem.itemName);
                    SelectItem(lastItem);
                }
                else
                {
                    lastItem = null;
                }
                if (newCharacter)
                {
                    //Debug.Log( characterIndex + " :currentPropNo: " + currentPropNo);
                    for (var i = 0; i < items.Count; i++)
                    {
                        var btn = itemButtons[i].GetComponent<Button>();
                        if (IsItemLocked(items[i]))
                        {
                            btn.transform.Find("SelectedText").gameObject.SetActive(false);
                            btn.transform.Find("SelectText").gameObject.SetActive(false);
                            btn.onClick.RemoveAllListeners();
                            if (items[i].canUnlockByAd)
                            {
                                btn.GetComponent<Image>().sprite = inventory.GetAdBtnSprite();
                                btn.transform.Find("PriceText").gameObject.SetActive(false);
                                var i1 = i;
                                btn.onClick.AddListener(() => UnlockingConfirmation(items[i1]));
                            }
                            else
                            {
                                btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                                btn.transform.Find("PriceText").gameObject.SetActive(true);
                                var i1 = i;
                                btn.onClick.AddListener(() => BuyingConfirmation(items[i1]));
                            }
                            
                        }
                        else
                        {
                            btn.transform.Find("SelectedText").gameObject.SetActive(false);
                            btn.transform.Find("SelectText").gameObject.SetActive(true);
                            btn.transform.Find("PriceText").gameObject.SetActive(false);
                            btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                            btn.onClick.RemoveAllListeners();
                            var i1 = i;
                            btn.onClick.AddListener(() => SelectItem(items[i1]));
                        }
                    }

                    if (currentItem != null)
                    {
                        if (!IsItemLocked(currentItem))
                        {
                            var index = items.IndexOf(currentItem);
                            Debug.Log("index: " + index);
                            if (index == -1)
                            {
                                return;
                            }

                            var btn = itemButtons[items.IndexOf(currentItem)].GetComponent<Button>();
                            btn.transform.Find("SelectedText").gameObject.SetActive(true);
                            btn.transform.Find("SelectText").gameObject.SetActive(false);
                            btn.transform.Find("PriceText").gameObject.SetActive(false);
                            btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                        }
                    }
                    else
                        Debug.Log("Current Item is null");
                }
                break;
            case Inventory.CustomizationType.ClassRoom:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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
        SetupCharacter(true);
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
        SetupCharacter(true);
    }
    public void CloseTheConfirmationPopup()
    {
        SoundController.Instance.PlayBtnClickSound();
        scrollView.gameObject.SetActive(true);
        confirmationPopup.SetActive(false);
        if(currentCustomizationType == Inventory.CustomizationType.Students)
            nameBar.SetActive(true);
        var currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(characterIndex);
        switch (currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                SetTeachersUI();
                break;
            case Inventory.CustomizationType.Students:
                currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(characterIndex);
                if (currentPropNo != -1)
                {
                    if (lastItem == null)
                    {
                        lastItem = inventory.GetLastStudentProp(currentPropNo);
                    }
                    SelectItem(lastItem);
                }
                else
                {
                    currentItem = null;
                    currentStudent.ApplyProp(null);
                }
                break;
            case Inventory.CustomizationType.ClassRoom:
                classRoomCustomization.HideProp(currentItem);
                break;
            default:
                throw new ArgumentOutOfRangeException();
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
        SoundController.Instance.PlayBtnClickSound();
        SharedUI.Instance.metaUIManager.EnableSchools(true);
        SharedUI.Instance.SwitchMenu(PlayerPrefsHandler.MainMenu);
        customizationArea.SetActive(false);
        classRoom.SetActive(false);
    }
}
