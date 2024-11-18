using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
[System.Serializable]
public class AllMenus
{
    [Required]
    public string name;
    [Space]
    [SceneObjectsOnly, Required]
    public GameObject menu;
}
public class SharedUI : MonoBehaviour
{
    #region Properties
    [ReadOnly] public GamePlayUIManager gamePlayUIManager;
    [ReadOnly] public MetaUIManager metaUIManager;
    [Space]
    [SerializeField] private AllMenus[] allMenus;
    [SerializeField] private AllMenus[] subMenus;
    [SerializeField] private AllMenus[] specialMenus;
    private int _sceneIndexToOpen = 1;
    [Header("Links")]
    public string privacyPolicyLink = "https://worldofanimal1234.blogspot.com/2022/02/world-of-animals.html";
    public string moreGamesLink = "https://play.google.com/store/apps/dev?id=6407456443209899378";
    #endregion
    public static SharedUI Instance;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        HideAllSubMenus();
    }
    public void GameplayUIActivated(GamePlayUIManager m)
    {
        gamePlayUIManager = m;
    }
    public void MetaUIActivated(MetaUIManager m)
    {
        metaUIManager = m;
    }
    private void GetReference()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            var g = FindObjectOfType<GamePlayUIManager>();
            GameplayUIActivated(g);
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            var g = FindObjectOfType<GamePlayUIManager>();
            GameplayUIActivated(g);
        }
    }
    public void SwitchMenu(string menuToShow)
    {
        if(!gamePlayUIManager)
            GetReference();
        if (GetActiveMenu(menuToShow))
        {
            //Debug.Log(menuToShow);
            return;
        }
        HideAll();
        GameObject menu = null;
        if (gamePlayUIManager)
        {
            menu = gamePlayUIManager.GetMenu(menuToShow);
            if (menu)
            {

                menu.SetActive(true);
                return;
            }
        }
        if (metaUIManager)
        {
            menu = metaUIManager.GetMenu(menuToShow);
            if (menu)
            {
                menu.SetActive(true);
                return;
            }
        }
        menu = GetMenu(menuToShow);
        if (menu)
        {
            menu.SetActive(true);
            return;
        }
        else
        {
            Debug.Log("No Menu Found! " + menuToShow);
        }
    }
    private GameObject GetMenu(string menuName)
    {
        foreach (var t in allMenus)
        {
            if (t.name == menuName)
                return t.menu;
        }
        return null;
    }
    public void HideAll()
    {
        if (gamePlayUIManager)
        {
            gamePlayUIManager.HideAll();
        }
        if (metaUIManager)
        {
            metaUIManager.HideAll();
        }
        foreach (var t in allMenus)
        {
            t.menu.SetActive(false);
        }
    }
    public bool GetActiveMenu(string menuName)
    {
        if (gamePlayUIManager)
        {
            if (gamePlayUIManager.GetActiveMenu(menuName))
            {
                return true;
            }
        }
        if (metaUIManager)
        {
            if (metaUIManager.GetActiveMenu(menuName))
            {
                return true;
            }
        }
        return allMenus.Where(t => t.menu.gameObject.activeSelf).Any(t => menuName == t.name);
    }
    public void SubMenu(string menuToShow)
    {
        HideAllSubMenus();
        GameObject menu = null;
        if (gamePlayUIManager)
        {
            menu = gamePlayUIManager.GetSubMenu(menuToShow);
            if (menu)
            {
                menu.SetActive(true);
                return;
            }
        }
        if (metaUIManager)
        {
            menu = metaUIManager.GetSubMenu(menuToShow);
            if (menu)
            {
                menu.SetActive(true);
                return;
            }
        }
        foreach (var t in subMenus)
        {
            if (!t.name.Equals(menuToShow)) continue;
            t.menu.SetActive(true);
            break;
        }
    }
    private void HideAllSubMenus()
    {
        if (gamePlayUIManager)
        {
            gamePlayUIManager.HideAllSubMenus();
        }
        if (metaUIManager)
        {
            metaUIManager.HideAllSubMenus();
        }
        foreach (var t in subMenus)
        {
            t.menu.SetActive(false);
        }
    }
    public void CloseSubMenu()
    {
        HideAllSubMenus();
    }
    private bool GetActiveSubMenu(string subMenuName)
    {
        return subMenus.Where(tempSubMenu => tempSubMenu.name == subMenuName).Any(tempSubMenu => tempSubMenu.menu.gameObject.activeSelf);
    }
    public GameObject GetSubMenu(string menuName)
    {
        return (from t in subMenus where t.name.Equals(menuName) select t.menu).FirstOrDefault();
    }
    public void OpenSpecialMenu(string menuToShow)
    {
        GameObject menu = null;
        menu = GetSpecialMenu(menuToShow);
        if (menu)
        {
            menu.SetActive(true);
            return;
        }
        else
        {
            Debug.Log("No Special Menu Found! " + menuToShow);
        }
    }
    private GameObject GetSpecialMenu(string menuName)
    {
        return (from t in specialMenus where t.name.Equals(menuName) select t.menu).FirstOrDefault();
    }
    public void CloseSpecialMenu(string menuToClose)
    {
        foreach (var t in specialMenus)
        {
            if (!t.name.Equals(menuToClose)) continue;
            t.menu.SetActive(false);
        }
    }
    public void SetNextSceneIndex(int sceneIndex)
    {
        _sceneIndexToOpen = sceneIndex;
    }
    public void SetNextSceneIndex(string sceneName)
    {
        switch (sceneName)
        {
            case PlayerPrefsHandler.Splash:
                _sceneIndexToOpen = 0;
                break;
            case PlayerPrefsHandler.GamePlay:
                _sceneIndexToOpen = 1;
                break;
            case PlayerPrefsHandler.Meta:
                _sceneIndexToOpen = 2;
                break;
        }
    }
    private int GetNextSceneIndex()
    {
        return _sceneIndexToOpen;
    }
    public void SwitchScene()
    {
        //StartCoroutine(DelayToSwitchScene());
        SceneManager.LoadScene(GetNextSceneIndex());
    }
    private IEnumerator DelayToSwitchScene()
    {
        //SwitchMenu(PlayerPrefsHandler.Loading);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(GetNextSceneIndex());
    }
}