using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using SaveDataVC = SaveDataV1;
public class SaveLoadTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            List<SaveCookie> list = new List<SaveCookie>();

            for(int i = 0 ; i < 20; i++)
            {
                SaveCookie newCookie = new SaveCookie();
                newCookie.CookieData = DataTableManager.CookieTable.Get("Cookie1");

                list.Add(newCookie);

            }

            SaveLoadManager.Data = new SaveDataVC();
            SaveLoadManager.Data.CookieList = list;


            SaveLoadManager.Save(0);
            Debug.Log("Saved Cookie");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            List<SaveGear> list = new List<SaveGear>();

            for (int i = 0; i < 10; i++)
            {
                SaveGear newGear = new SaveGear();
                newGear.GearData = DataTableManager.GearTable.Get("Item1");

                list.Add(newGear);

            }

            SaveLoadManager.Data = new SaveDataVC();
            SaveLoadManager.Data.GearList = list;

            SaveLoadManager.Save(1);
            Debug.Log("Saved Gear");

        }
    }
}