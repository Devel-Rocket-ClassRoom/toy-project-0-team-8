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

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null) continue;

            if (i == index)
            {
                buttons[i].Select();
            }
        }
    }

    private void OnEnable()
    {

        OnCategoryTab(0);
    }

    public void OnLoadCookie()
    {
        DataTableManager.ChangeDataType(DataType.Cookie);

        uiGearList.gameObject.SetActive(false);
        uiCookieList.gameObject.SetActive(true);

        uiCookieList.SetSaveCookieDataList(SaveLoadManager.Data.CookieList);

    }
    public void OnLoadGear()
    {
        DataTableManager.ChangeDataType(DataType.Gear);

        uiCookieList.gameObject.SetActive(false);
        uiGearList.gameObject.SetActive(true);

        uiGearList.SetSaveGearDataList(SaveLoadManager.Data.GearList);

    }
}
