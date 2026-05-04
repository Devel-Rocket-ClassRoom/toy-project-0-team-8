using System;
using UnityEngine;

public class UiCategorySelect : MonoBehaviour
{
    public UiCookieList uiCookieList;
    public UiGearList uiGearList;

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
