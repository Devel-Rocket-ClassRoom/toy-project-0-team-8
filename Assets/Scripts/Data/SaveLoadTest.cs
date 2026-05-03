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

            foreach (SaveCookie cookie in list)
            {
                cookie.CookieData = new CookieData();
            }

            SaveLoadManager.Data = new SaveDataVC();
            SaveLoadManager.Data.CookieList = list;


            SaveLoadManager.Save(0);
            Debug.Log("Saved Cookie");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SaveLoadManager.Data = new SaveDataVC();
            SaveLoadManager.Save(1);
            Debug.Log("Saved Gear");

        }
    }
}