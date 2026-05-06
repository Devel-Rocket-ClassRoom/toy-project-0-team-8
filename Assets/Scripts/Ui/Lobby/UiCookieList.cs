using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiCookieList : MonoBehaviour
{
    private List<UiCookieSlot> uiSlotList = new List<UiCookieSlot>();

    public UiCookieSlot prefab;
    public ScrollRect scrollRect;

    // 데이터 갱신을 위한 이벤트
    public UnityEvent onUpdateSlot;
    public UnityEvent<SaveCookie> onSelectSlot;

    //데이터 전송
    public EquipObject equip;

    private void OnSelectSlot(SaveCookie saveCookie)
    {
        Debug.Log(saveCookie.CookieData.Name);
        equip.saveCookie = saveCookie;

    }
    private void Start()
    {
        onSelectSlot.AddListener(OnSelectSlot);

    }

    public void SetSaveCookieDataList(Dictionary<string, int> source)
    {
        foreach (var slot in uiSlotList)
        {
            slot.level = source[slot.CookieData.cookieId];
            slot.SetCookie();
        }
    }



    public void ClearList()
    {
        for (int i = 0; i < uiSlotList.Count; i++)
        {
            uiSlotList[i].gameObject.SetActive(false);
        }

        // 선택된 보물 할당 제거
        equip.saveGear = null;
    }
    
}
