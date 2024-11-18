using UnityEngine;
public class ClassRoomCustomization : MonoBehaviour
{
    [SerializeField] private Transform decorations;
    private Inventory.Item _lastItem;
    private GameObject _currentProp;
    [SerializeField] private GameObject classRoomMesh;
    [SerializeField] private Material floorDirty, floorClean;
    [SerializeField] private Texture dirtyTexture;
    [SerializeField] private Color wallDirtyColor, wallCleanColor, roofDirtyColor, roofCleanColor;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    private Inventory _inventory;

    private enum PropNames
    {
        BookShelf,
        ChemistryTable,
        Skeleton,
        ComputerTable,
        Pot,
        Counting,
        Ceiling,
        Windows,
        Walls,
        Floor
    }
    /*private void Start()
    {
        _inventory = GamePlayManager.Instance ? GamePlayManager.Instance.environmentManager.GetInventory() : SharedUI.Instance.metaUIManager.
            GetMenu(PlayerPrefsHandler.CharactersCustomization).GetComponent<Customization>().GetInventory();
    }*/
    public void ApplyProp(Inventory.Item prop)
    {
        if(prop == null) return;
        UndoPreviousWorking(prop);
    }
    public void HideProp(Inventory.Item prop)
    {
        if (_currentProp)
        {
            if (IsNameExists(_currentProp.name))
            {
                _currentProp.SetActive(false);
            }
            else if (_currentProp.name.Contains(PropNames.Windows.ToString()))
            {
                for (var i = 0; i < _currentProp.transform.childCount; i++)
                {
                    _currentProp.transform.GetChild(i).Find("Crack").gameObject.SetActive(true);
                }
            }
            else if (_currentProp.name.Contains("ClassRoom"))
            {
                _currentProp.GetComponent<MeshRenderer>().material = floorDirty;
                var materials = _currentProp.GetComponent<MeshRenderer>().materials;
                materials[1].SetTexture(MainTex, dirtyTexture);
                materials[2].SetTexture(MainTex, dirtyTexture);
                materials[1].color = wallDirtyColor;
                materials[2].color = roofDirtyColor;
            }
        }
    }
    private void PropWorking(Inventory.Item prop)
    {
        _lastItem = prop;
        if (IsNameExists(prop.itemName))
        {
            _currentProp = decorations.Find(prop.itemName).gameObject;
            _currentProp.SetActive(true);
        }
        else if (prop.itemName == PropNames.Windows.ToString())
        {
            //Debug.Log("itemName: " + prop.itemName);
            _currentProp = decorations.Find(prop.itemName).gameObject;
            for (var i = 0; i < _currentProp.transform.childCount; i++)
            {
                _currentProp = decorations.Find(prop.itemName).gameObject;
                _currentProp.transform.GetChild(i).Find("Crack").gameObject.SetActive(false);
            }
        }
        else if (prop.itemName == PropNames.Walls.ToString())
        {
            Debug.Log("itemName: " + prop.itemName);
            _currentProp = classRoomMesh;
            var materials = _currentProp.GetComponent<MeshRenderer>().materials;
            materials[1].SetTexture(MainTex, null);
            materials[2].SetTexture(MainTex, null);
            materials[1].color = wallCleanColor;
            materials[2].color = roofCleanColor;

        }
        else if (prop.itemName == PropNames.Floor.ToString())
        {
            _currentProp = classRoomMesh;
            _currentProp.GetComponent<MeshRenderer>().material = floorClean;
        }
    }
    private void UndoPreviousWorking(Inventory.Item prop)
    {
        if (_lastItem != null)
        {
            if (PlayerPrefsHandler.IsClassPropLocked(_lastItem.itemId))
            {
                if (_currentProp)
                {
                    if (IsNameExists(_currentProp.name))
                    {
                        _currentProp.SetActive(false);
                    }
                    else if (_currentProp.name.Contains(PropNames.Windows.ToString()))
                    {
                        for (var i = 0; i < _currentProp.transform.childCount; i++)
                        {
                            _currentProp.transform.GetChild(i).Find("Crack").gameObject.SetActive(true);
                        }
                    }
                    else if (_currentProp.name.Contains("ClassRoom"))
                    {
                        _currentProp.GetComponent<MeshRenderer>().material = floorDirty;
                        var materials = _currentProp.GetComponent<MeshRenderer>().materials;
                        materials[1].SetTexture(MainTex, dirtyTexture);
                        materials[2].SetTexture(MainTex, dirtyTexture);
                        materials[1].color = wallDirtyColor;
                        materials[2].color = roofDirtyColor;
                    }
                }
            }
        }
        PropWorking(prop);
    }
    private bool IsNameExists(string nameToCheck)
    {
        return nameToCheck == PropNames.BookShelf.ToString() || nameToCheck == PropNames.ChemistryTable.ToString() ||
               nameToCheck == PropNames.Skeleton.ToString()
               || nameToCheck == PropNames.ComputerTable.ToString() || nameToCheck == PropNames.Pot.ToString() ||
               nameToCheck == PropNames.Counting.ToString()
               || nameToCheck == PropNames.Ceiling.ToString();
    }
}