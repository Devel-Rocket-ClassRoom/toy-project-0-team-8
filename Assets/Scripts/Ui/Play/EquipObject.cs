using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipObject : MonoBehaviour
{
    public UiCookieSlot SaveCookie;
    public ChangeScene changeScene;
    public List<Image> saveGearIcon = new List<Image>();

    public Image CookieIcon;
    public Image[] Gear;
    public Sprite Blank;
    
    public GameObject CookieList;
    public GameObject GearList;

    public GameObject Inventory;

    private int maxIndex = 3;
    private UiGearSlot[] currentSlots = new UiGearSlot[3];
    public void EquipCheck(UiGearSlot uiGearSlot)
    {
        int emptyIndex = Array.FindIndex(currentSlots, slot => slot == null);

        currentSlots[emptyIndex] = uiGearSlot;
        uiGearSlot.selectMark.enabled = true;

    }
    public void EquipCheckCookie(UiCookieSlot saveCookie)
    {
        if(SaveCookie == saveCookie)
        {
            saveCookie.selectMark.enabled = true;
        }

    }

    public void EquipCookie(UiCookieSlot saveCookie)
    {
        SaveCookie = saveCookie;

        saveCookie.selectMark.enabled = !saveCookie.selectMark.enabled;


    }
    public void EquipGear(UiGearSlot saveGear)
    {
        // 취소 로직: 이미 선택된 칸을 다시 누른 경우
        if (saveGear.selectMark.enabled)
        {
            saveGear.selectMark.enabled = false;

            int foundIndex = Array.IndexOf(currentSlots, saveGear);
            if (foundIndex > -1)
            {
                currentSlots[foundIndex] = null;
            }
            return;
        }

        // 장착 로직: 새로 선택하는 경우
        // 중복 체크
        if (currentSlots.Contains(saveGear)) return;

        // 빈 칸 찾기
        int emptyIndex = Array.FindIndex(currentSlots, slot => slot == null);

        if (emptyIndex > -1)
        {
            saveGear.selectMark.enabled = true;
            currentSlots[emptyIndex] = saveGear;
            Debug.Log($"{emptyIndex}번 칸에 장착 완료");
        }
        else
        {
            // 모든 칸이 꽉 찼을 때의 처리를 추가하면 좋습니다.
            Debug.Log("더 이상 장착할 수 없습니다. 빈 칸이 없어요!");
        }
    }
    public void SetData()
    {
        // 클릭 시 실행할 함수.
        // null이 아닌 데이터를 찾아서 장착함

        // 
        changeScene.equipGear = currentSlots;

        currentSlots = new UiGearSlot[maxIndex];
        if (GearList.activeInHierarchy) 
        {
            string[] gear = new string[maxIndex];
            for (int i = 0; i < maxIndex; i++)
            {
                var data = changeScene.equipGear[i];


                if (data != null)
                {
                    Gear[i].sprite = data.GearData.Icon;
                    gear[i] = data.GearData.itemId;
                }
                else
                {
                    Gear[i].sprite = Blank;
                    gear[i] = null;
                }
            }

            SaveLoadManager.Data.currentGear = gear;
            SaveLoadManager.Save();
        }
        

        if (CookieList.activeInHierarchy)
        {
            CookieIcon.sprite = SaveCookie.CookieData.Icon;
            SaveCookie.selectMark.enabled = false;

            SaveLoadManager.Data.currentCookie = SaveCookie.CookieData.cookieId;
            SaveLoadManager.Save();
        }

        var list = Inventory.GetComponent<UiCategorySelect>().uiGearList.uiSlotList;

        for (int i = 0; i < list.Count; i++ ) 
        {
            list[i].selectMark.enabled = false;
        }

        Inventory.SetActive(false);
    }
}
