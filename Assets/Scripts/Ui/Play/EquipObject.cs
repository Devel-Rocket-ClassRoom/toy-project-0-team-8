using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipObject : MonoBehaviour
{
    public SaveCookie saveCookie;
    public List<UiGearSlot> saveGear = new List<UiGearSlot>();

    public Image CookieIcon;
    public Image[] Gear;


    public GameObject Inventory;

    public void EquipCheck()
    {
        // saveGear 내부 데이터를 확인해서
        // 세이브 기어의 "장착중" 이미지 켜기
        
    }

    public void SetData()
    {
        // 클릭 시 실행할 함수.
        // null이 아닌 데이터를 찾아서 장착함
        if (saveGear != null)
        {
            for(int i = 0; i < saveGear.Count; i++)
            {
                if(Gear[i] != null)
                    Gear[i].sprite = saveGear[i].SaveGearData.GearData.SpriteIcon;
            }
        }

        if (saveCookie != null)
        {
            CookieIcon.sprite = saveCookie.CookieData.SpriteIcon;
        }

        Inventory.SetActive(false);
    }
}
