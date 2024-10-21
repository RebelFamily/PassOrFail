using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GamePlayUIManager : MonoBehaviour
{
    #region Properties
    
    [SerializeField] private AllMenus[] allMenus;
    [SerializeField] private AllMenus[] subMenus;
    
    [HideInInspector] public Controls controls;
    
    #endregion
    private void Awake()
    {
        SharedUI.Instance.HideAll();
        SharedUI.Instance.GameplayUIActivated(this);
        controls = GetMenu(PlayerPrefsHandler.HUD).GetComponent<Controls>();
    }
    public bool GetActiveMenu(string menuName)
    {
        return allMenus.Where(t => t.menu.gameObject.activeSelf).Any(t => menuName == t.name);
    }
    public void HideAll()
    {
        foreach (var t in allMenus)
        {
            t.menu.SetActive(false);
        }
    }
    public GameObject GetMenu(string menuName)
    {
        return (from t in allMenus where t.name.Equals(menuName) select t.menu).FirstOrDefault();
    }
    public void CloseMenu(string menuToClose)
    {
        foreach (var t in allMenus)
        {
            if (!t.name.Equals(menuToClose)) continue;
            t.menu.SetActive(false);
        }
    }
    public GameObject GetSubMenu(string menuName)
    {
        return (from t in subMenus where t.name.Equals(menuName) select t.menu).FirstOrDefault();
    }
    public void HideAllSubMenus()
    {
        foreach (var t in subMenus)
        {
            t.menu.SetActive(false);
        }
    }
}