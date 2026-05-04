using System;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class UiCategorySelect : MonoBehaviour
{
    public UiCookieList uiCookieList;
    public UiGearList uiGearList;
    
    public Button[] buttons;
    public int index;


    public void OnCategoryTab(int index)
    {
        buttons[index].onClick.Invoke();
    }



    public void OnLoadCookie()
    {
        SaveLoadManager.Load(0);
        uiGearList.ClearList();
        uiCookieList.SetSaveCookieDataList(SaveLoadManager.Data.CookieList);

    }
    public void OnLoadGear()
    {
        SaveLoadManager.Load(1);
        uiCookieList.ClearList();
        uiGearList.SetSaveGearDataList(SaveLoadManager.Data.GearList);

    }
}
