using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class Customization : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    private readonly List<GameObject> _itemButtons = new List<GameObject>();
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private GameObject confirmationPopup;
    private Inventory.CustomizationType _currentCustomizationType;
    private Inventory.ItemType _currentItemType;
    private Inventory.Item _currentItem, _lastItem;
    private List<Inventory.Item> _items = new List<Inventory.Item>();
    private int _characterIndex = 0;
    [SerializeField] private GameObject customizationArea, subCategoriesButtons0, subCategoriesButtons1;
    [SerializeField] private Transform teachers, students, studentsCamera, studentsCameraPositions, classRoomCamera;
    [SerializeField] private Button nextCharacterBtn, previousCharacterBtn;
    [SerializeField] private GameObject nameBar;
    private static readonly string[] StudentsNames = new[] {"Robin", "Tommy", "Nami", "Lucky", "Mano", "Rocky", "Kate", "Jack"};
    private int itemIndexToUnlock = 0, itemPrice = 0, buttonIndex = 0;
    [SerializeField] private StudentCustomization currentStudent;
    [SerializeField] private Color[] subCategoriesColors;
    private Text _nameText;
    private Canvas _canvas;
    private const string NameTextString = "NameText", 
        SelectedTextString = "SelectedText",
        EquippedTextString = "EquippedText",
        SelectTextString = "SelectText",
        PriceTextString = "PriceText",
        CashIconString = "CashIcon",
        AdIconString = "AdIcon",
        BuyString = "Buy",
        PriceTextPath = "CashIcon/PriceText",
        RenderString = "Render";
    private void OnEnable()
    {
        if (!_canvas)
            _canvas = GetComponentInParent<Canvas>();
        _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        Callbacks.OnRewardItem += RewardItem;
        SharedUI.Instance.metaUIManager.EnableSchools(false);
    }
    private void OnDisable()
    {
        _canvas.renderMode = RenderMode.ScreenSpaceCamera;
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
                subCategoriesButtons0.SetActive(false);
                subCategoriesButtons1.SetActive(false);
                SelectCategory(Inventory.CustomizationType.Teachers);
                break;
            case "Students":
                customizationArea.SetActive(true);
                subCategoriesButtons0.SetActive(true);
                subCategoriesButtons1.SetActive(false);
                SelectCategory(Inventory.CustomizationType.Students);
                break;
        }
    }
    private void SelectCategory(Inventory.CustomizationType category, Inventory.ItemType itemType = Inventory.ItemType.Common)
    {
        _currentCustomizationType = category;
        switch (category)
        {
            case Inventory.CustomizationType.Teachers:
                nameBar.SetActive(false);
                _characterIndex = PlayerPrefsHandler.currentTeacher;
                teachers.gameObject.SetActive(true);
                students.gameObject.SetActive(false);
                studentsCamera.position = studentsCameraPositions.GetChild(0).position;
                
                EnableATeacher();
                Invoke(nameof(SetTeachersUI), 0.1f);
                break;
            case Inventory.CustomizationType.Students:
                _currentItemType = itemType;
                nameBar.SetActive(true);
                _characterIndex = 0;
                if(!_nameText)
                    _nameText = nameBar.transform.Find(NameTextString).GetComponent<Text>();
                _nameText.text = StudentsNames[_characterIndex];
                teachers.gameObject.SetActive(false);
                students.gameObject.SetActive(true);
                studentsCamera.position = studentsCameraPositions.GetChild(0).position;
                Invoke(nameof(InvokeSetupCharacter), 0.1f);
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
        _items.Clear();
        _items = inventory.GetItems(_currentCustomizationType, _currentItemType).ToList();
        for (var i = 0; i < _items.Count; i++)
        {
            var item = _items[i];
            GameObject btn;
            if (_itemButtons.Count - 1 >= i)
            {
                btn = _itemButtons[i];
            }
            else
            {
                btn = Instantiate(prefab, scrollView.content);
                _itemButtons.Add(btn);
            }
            btn.transform.Find(SelectedTextString).gameObject.SetActive(false);
            btn.transform.Find(EquippedTextString).gameObject.SetActive(false);
            btn.transform.Find(SelectTextString).gameObject.SetActive(false);
            var btnComponent = btn.GetComponent<Button>();
            btnComponent.onClick.RemoveAllListeners();
            btn.transform.Find(RenderString).GetComponent<Image>().sprite = item.itemIcon;
            //Debug.Log("item: " + item.itemName + " : " + IsItemLocked(item));
            if (IsItemLocked(item))
            {
                if (item.canUnlockByAd)
                {
                    btn.GetComponent<Image>().sprite = inventory.GetAdBtnSprite();
                    var txt = btn.transform.Find(PriceTextString);
                    txt.gameObject.SetActive(false);
                    btnComponent.onClick.AddListener(() =>
                    {
                        UnlockingConfirmation(item);
                    });
                }
                else
                {
                    btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                    var txt = btn.transform.Find(PriceTextString);
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
                btn.transform.Find(EquippedTextString).gameObject.SetActive(true);
                btn.transform.Find(SelectedTextString).gameObject.SetActive(false);
                btn.transform.Find(SelectTextString).gameObject.SetActive(false);
                btn.transform.Find(PriceTextString).gameObject.SetActive(false);
                btnComponent.onClick.AddListener(() =>
                {
                    SelectItem(item);
                });
            }
            _itemButtons[i].SetActive(true);
        }
        if (_itemButtons.Count > _items.Count)
        {
            for (var i = _items.Count; i < _itemButtons.Count; i++)
            {
                _itemButtons[i].gameObject.SetActive(false);
            }   
        }
    }
    private void SetTeachersUI()
    {
        var btn = _itemButtons[PlayerPrefsHandler.currentTeacher].transform;
        btn.transform.Find(SelectedTextString).gameObject.SetActive(true);
        btn.transform.Find(SelectTextString).gameObject.SetActive(false);
    }
    private void SelectItem(Inventory.Item newItem, bool calledByUnlockItem = false)
    {
        //Debug.Log("SelectItem: " + newItem.itemName);
        _currentItem = newItem;
        switch (_currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                _characterIndex = newItem.itemId;
                EnableATeacher();
                for (var i = 0; i < _items.Count; i++)
                {
                    var btn = _itemButtons[i].GetComponent<Button>();
                    btn.transform.Find(EquippedTextString).gameObject.SetActive(false);
                    if (IsItemLocked(_items[i]))
                    {
                        btn.transform.Find(SelectedTextString).gameObject.SetActive(false);
                        btn.transform.Find(SelectTextString).gameObject.SetActive(false);
                        if (calledByUnlockItem)
                        {
                            btn.onClick.RemoveAllListeners();
                            btn.GetComponent<Image>().sprite = inventory.GetAdBtnSprite();
                            btn.transform.Find(PriceTextPath).gameObject.SetActive(false);
                            var i1 = i;
                            btn.onClick.AddListener(() => UnlockingConfirmation(_items[i1]));
                        }
                    }
                    else
                    {
                        btn.transform.Find(SelectedTextString).gameObject.SetActive(false);
                        btn.transform.Find(SelectTextString).gameObject.SetActive(true);
                        if (calledByUnlockItem)
                        {
                            btn.transform.Find(PriceTextString).gameObject.SetActive(false);
                            btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                            btn.onClick.RemoveAllListeners();
                            var i1 = i;
                            btn.onClick.AddListener(() => SelectItem(_items[i1]));
                        }
                    }
                }
                if (!IsItemLocked(_currentItem))
                {
                    var btn = _itemButtons[_items.IndexOf(_currentItem)].GetComponent<Button>();
                    btn.transform.Find(SelectedTextString).gameObject.SetActive(true);
                    btn.transform.Find(SelectTextString).gameObject.SetActive(false);
                    btn.transform.Find(PriceTextString).gameObject.SetActive(false);
                    btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                    PlayerPrefsHandler.currentTeacher = _currentItem.itemId;
                }
                break;
            case Inventory.CustomizationType.Students:
                currentStudent.ApplyProp(_currentItem);
                for (var i = 0; i < _items.Count; i++)
                {
                    var btn = _itemButtons[i].GetComponent<Button>();
                    btn.transform.Find(EquippedTextString).gameObject.SetActive(false);
                    if (IsItemLocked(_items[i]))
                    {
                        btn.transform.Find(SelectedTextString).gameObject.SetActive(false);
                        btn.transform.Find(SelectTextString).gameObject.SetActive(false);
                        if (calledByUnlockItem)
                        {
                            btn.onClick.RemoveAllListeners();
                            if (_items[i].canUnlockByAd)
                            {
                                btn.GetComponent<Image>().sprite = inventory.GetAdBtnSprite();
                                btn.transform.Find(PriceTextString).gameObject.SetActive(false);
                                var i1 = i;
                                btn.onClick.AddListener(() => UnlockingConfirmation(_items[i1]));
                            }
                            else
                            {
                                btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                                btn.transform.Find(PriceTextString).gameObject.SetActive(true);
                                var i1 = i;
                                btn.onClick.AddListener(() => BuyingConfirmation(_items[i1]));
                            }
                        }
                    }
                    else
                    {
                        btn.transform.Find(SelectedTextString).gameObject.SetActive(false);
                        btn.transform.Find(SelectTextString).gameObject.SetActive(true);
                        if (calledByUnlockItem)
                        {
                            btn.transform.Find(PriceTextString).gameObject.SetActive(false);
                            btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                            btn.onClick.RemoveAllListeners();
                            var i1 = i;
                            btn.onClick.AddListener(() => SelectItem(_items[i1]));
                        }
                    }
                }

                if (_currentItem != null)
                {
                    if (!IsItemLocked(_currentItem))
                    {
                        var index = _items.IndexOf(_currentItem);
                        //Debug.Log("index: " + index);
                        if (index == -1)
                        {
                            return;
                        }

                        var btn = _itemButtons[_items.IndexOf(_currentItem)].GetComponent<Button>();
                        btn.transform.Find(SelectedTextString).gameObject.SetActive(true);
                        btn.transform.Find(SelectTextString).gameObject.SetActive(false);
                        btn.transform.Find(PriceTextString).gameObject.SetActive(false);
                        btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                        _lastItem = _currentItem;
                        PlayerPrefsHandler.SetStudentCurrentProp(_characterIndex, _currentItem.itemId);
                    }
                }
                else
                    Debug.Log("Current Item is Null");
                break;
            
        }
    }
    private bool IsItemLocked(Inventory.Item item)
    {
        switch (_currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                return PlayerPrefsHandler.IsTeacherLocked(item.itemId);
            case Inventory.CustomizationType.Students:
                return PlayerPrefsHandler.IsStudentPropLocked(_characterIndex, item.itemId);
            default:
                return false;
        }
    }
    private void UnlockingConfirmation(Inventory.Item newItem)
    {
        //Debug.Log(newItem.itemId + " :UnlockingConfirmation: ");
        SoundController.Instance.PlayBtnClickSound();
        _currentItem = newItem;
        confirmationPopup.transform.Find(CashIconString).gameObject.SetActive(false);
        confirmationPopup.transform.Find(AdIconString).gameObject.SetActive(true);
        var btn = confirmationPopup.transform.Find(BuyString).GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(WatchRewardedAdToUnlockItem);
        confirmationPopup.SetActive(true);
        if(_currentCustomizationType == Inventory.CustomizationType.Students)
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
        //Debug.Log(newItem.itemId + " :BuyingConfirmation : ");
        SoundController.Instance.PlayBtnClickSound();
        _currentItem = newItem;
        confirmationPopup.transform.Find(CashIconString).gameObject.SetActive(true);
        confirmationPopup.transform.Find(AdIconString).gameObject.SetActive(false);
        confirmationPopup.transform.Find(PriceTextPath).GetComponent<Text>().text = _currentItem.itemPrice.ToString();
        var btn = confirmationPopup.transform.Find(BuyString).GetComponent<Button>();
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(BuyItem);
        confirmationPopup.SetActive(true);
        if(_currentCustomizationType == Inventory.CustomizationType.Students)
            nameBar.SetActive(false);
        scrollView.gameObject.SetActive(false);
        SelectItem(newItem);
    }
    private void BuyItem()
    {
        //Debug.Log("BuyItem");
        if (_currentItem.itemPrice <= PlayerPrefsHandler.currency)
        {
            CurrencyCounter.Instance.DeductCurrency(_currentItem.itemPrice, _itemButtons[buttonIndex].transform);
            UnlockItem();
        }
    }
    private void UnlockItem()
    {
        //Debug.Log(itemIndexToUnlock + " : " + _currentCustomizationType + " : " + _currentItemType);
        _lastItem = _currentItem;
        switch (_currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                PlayerPrefsHandler.UnlockTeacher(_currentItem.itemId);
                SelectItem(_currentItem, true);
                break;
            case Inventory.CustomizationType.Students:
                PlayerPrefsHandler.UnlockStudentProp(_characterIndex, _currentItem.itemId);
                PlayerPrefsHandler.SetStudentCurrentProp(_characterIndex, _currentItem.itemId);
                SelectItem(_currentItem, true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        confirmationPopup.SetActive(false);
        if(_currentCustomizationType == Inventory.CustomizationType.Students)
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
            if (_currentItemType != Inventory.ItemType.Common)
            {
                _items.Clear();
                _items = inventory.GetItems(_currentCustomizationType, Inventory.ItemType.Common).ToList();
                scrollView.GetComponent<Image>().color = subCategoriesColors[0];
            }
            _currentItemType = Inventory.ItemType.Common;
        }
        else if (subCategory == Inventory.ItemType.Rare.ToString())
        {
            if (_currentItemType != Inventory.ItemType.Rare)
            {
                _items.Clear();
                _items = inventory.GetItems(_currentCustomizationType, Inventory.ItemType.Rare).ToList();
                scrollView.GetComponent<Image>().color = subCategoriesColors[1];
            }
            _currentItemType = Inventory.ItemType.Rare;
        }
        else if (subCategory == Inventory.ItemType.Epic.ToString())
        {
            if (_currentItemType != Inventory.ItemType.Epic)
            {
                _items.Clear();
                _items = inventory.GetItems(_currentCustomizationType, Inventory.ItemType.Epic).ToList();
                scrollView.GetComponent<Image>().color = subCategoriesColors[2];
            }
            _currentItemType = Inventory.ItemType.Epic;
        }
        else if (subCategory == Inventory.ItemType.Decorate.ToString())
        {
            if (_currentItemType != Inventory.ItemType.Decorate)
            {
                _items.Clear();
                _items = inventory.GetItems(_currentCustomizationType, Inventory.ItemType.Decorate).ToList();
                scrollView.GetComponent<Image>().color = subCategoriesColors[0];
            }
            _currentItemType = Inventory.ItemType.Decorate;
        }
        else
        {
            if (_currentItemType != Inventory.ItemType.Upgrade)
            {
                _items.Clear();
                _items = inventory.GetItems(_currentCustomizationType, Inventory.ItemType.Upgrade).ToList();
                scrollView.GetComponent<Image>().color = subCategoriesColors[1];
            }
            _currentItemType = Inventory.ItemType.Upgrade;
        }
        SetUI();
        SelectItem(_lastItem);
    }
    private void EnableATeacher()
    {
        for (var i = 0; i < teachers.childCount; i++)
        {
            teachers.GetChild(i).gameObject.SetActive(false);
        }
        teachers.GetChild(_characterIndex).gameObject.SetActive(true);
    }
    private void InvokeSetupCharacter()
    {
        SetupCharacter();   
    }
    private void SetupCharacter(bool newCharacter = false)
    {
        var currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(_characterIndex);
        switch (_currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                break;
            case Inventory.CustomizationType.Students:
                currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(_characterIndex);
                if (currentPropNo != -1)
                {
                    _lastItem = inventory.GetLastStudentProp(currentPropNo);
                    //Debug.Log("LastItem: " + lastItem.itemName);
                    SelectItem(_lastItem);
                }
                else
                {
                    _lastItem = null;
                }
                if (newCharacter)
                {
                    //Debug.Log( characterIndex + " :currentPropNo: " + currentPropNo);
                    for (var i = 0; i < _items.Count; i++)
                    {
                        var btn = _itemButtons[i].GetComponent<Button>();
                        if (IsItemLocked(_items[i]))
                        {
                            btn.transform.Find(SelectedTextString).gameObject.SetActive(false);
                            btn.transform.Find(SelectTextString).gameObject.SetActive(false);
                            btn.onClick.RemoveAllListeners();
                            if (_items[i].canUnlockByAd)
                            {
                                btn.GetComponent<Image>().sprite = inventory.GetAdBtnSprite();
                                btn.transform.Find(PriceTextString).gameObject.SetActive(false);
                                var i1 = i;
                                btn.onClick.AddListener(() => UnlockingConfirmation(_items[i1]));
                            }
                            else
                            {
                                btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                                btn.transform.Find(PriceTextString).gameObject.SetActive(true);
                                var i1 = i;
                                btn.onClick.AddListener(() => BuyingConfirmation(_items[i1]));
                            }
                            
                        }
                        else
                        {
                            btn.transform.Find(SelectedTextString).gameObject.SetActive(false);
                            btn.transform.Find(SelectTextString).gameObject.SetActive(true);
                            btn.transform.Find(PriceTextString).gameObject.SetActive(false);
                            btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                            btn.onClick.RemoveAllListeners();
                            var i1 = i;
                            btn.onClick.AddListener(() => SelectItem(_items[i1]));
                        }
                    }

                    if (_currentItem != null)
                    {
                        if (!IsItemLocked(_currentItem))
                        {
                            var index = _items.IndexOf(_currentItem);
                            //Debug.Log("index: " + index);
                            if (index == -1)
                            {
                                return;
                            }

                            var btn = _itemButtons[_items.IndexOf(_currentItem)].GetComponent<Button>();
                            btn.transform.Find(SelectedTextString).gameObject.SetActive(true);
                            btn.transform.Find(SelectTextString).gameObject.SetActive(false);
                            btn.transform.Find(PriceTextString).gameObject.SetActive(false);
                            btn.GetComponent<Image>().sprite = inventory.GetSimpleBtnSprite();
                        }
                    }
                    else
                        Debug.Log("Current Item is null");
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    public void NextCharacter()
    {
        SoundController.Instance.PlayBtnClickSound();
        if (_characterIndex < studentsCameraPositions.childCount - 1)
        {
            _characterIndex++;
            nextCharacterBtn.interactable = false;
            previousCharacterBtn.interactable = false;
            currentStudent = students.GetChild(_characterIndex).GetComponent<StudentCustomization>();
        }
        else
            return;
        studentsCamera.DOMove(studentsCameraPositions.GetChild(_characterIndex).position, 0.5f).OnComplete(() =>
        {
            _nameText.text = StudentsNames[_characterIndex];
            nextCharacterBtn.interactable = true;
            previousCharacterBtn.interactable = true;
        });
        SetupCharacter(true);
    }
    public void PreviousCharacter()
    {
        SoundController.Instance.PlayBtnClickSound();
        if (_characterIndex > 0)
        {
            _characterIndex--;
            nextCharacterBtn.interactable = false;
            previousCharacterBtn.interactable = false;
            currentStudent = students.GetChild(_characterIndex).GetComponent<StudentCustomization>();
        }
        else
            return;
        studentsCamera.DOMove(studentsCameraPositions.GetChild(_characterIndex).position, 0.5f).OnComplete(() =>
        {
            _nameText.text = StudentsNames[_characterIndex];
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
        if(_currentCustomizationType == Inventory.CustomizationType.Students)
            nameBar.SetActive(true);
        var currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(_characterIndex);
        switch (_currentCustomizationType)
        {
            case Inventory.CustomizationType.Teachers:
                SetTeachersUI();
                break;
            case Inventory.CustomizationType.Students:
                currentPropNo = PlayerPrefsHandler.GetStudentCurrentProp(_characterIndex);
                if (currentPropNo != -1)
                {
                    if (_lastItem == null)
                    {
                        _lastItem = inventory.GetLastStudentProp(currentPropNo);
                    }
                    SelectItem(_lastItem);
                }
                else
                {
                    _currentItem = null;
                    currentStudent.ApplyProp(null);
                }
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
    }
}
