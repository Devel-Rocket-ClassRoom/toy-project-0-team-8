using System;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class test : MonoBehaviour
{
    private List<SaveCookie> list = new List<SaveCookie>();


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            CookieDummy("Cookie_Pirate");
            CookieDummy("Cookie_Hero");


            SaveLoadManager.Data = new SaveDataV1();
            SaveLoadManager.Data.CookieList = list;

            SaveLoadManager.Save(0);
            Debug.Log("Saved Cookie");
        }
    }

    private void CookieDummy(string cookieId)
    {
        SaveCookie newCookie = new SaveCookie();
        newCookie.CookieData = DataTableManager.CookieTable.Get(cookieId);

        list.Add(newCookie);
    }
}
