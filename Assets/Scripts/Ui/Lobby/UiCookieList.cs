using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiCookieList : MonoBehaviour
{
    private List<UiCookieSlot> uiSlotList = new List<UiCookieSlot>();

    public UiCookieSlot prefab;
    public ScrollRect scrollRect;
    public Image LobbyCookie;

    // 데이터 갱신을 위한 이벤트
    public UnityEvent onUpdateSlot;
    public UnityEvent<UiCookieSlot> onSelectSlot;

    //데이터 전송
    public EquipObject equip;

    private void OnSelectSlot(UiCookieSlot saveCookie)
    {
        if (equip.gameObject.activeSelf)
        {
            Debug.Log(saveCookie.CookieData.cookieName);

            foreach (var slot in uiSlotList)
            {
                slot.selectMark.enabled = false;
            }
            equip.EquipCookie(saveCookie);
        }
        else
        {
            Debug.Log(saveCookie.CookieData.Icon.name);
            SaveLoadManager.Data.lobbyCookieId = saveCookie.CookieData.cookieId;
            SaveLoadManager.Save();
            LobbyCookie.sprite = saveCookie.CookieData.Icon;
        }
    }

    private void Awake()
    {
        foreach (var slot in scrollRect.content.GetComponentsInChildren<UiCookieSlot>(true))
        {
            uiSlotList.Add(slot);
            slot.button.onClick.AddListener(() => onSelectSlot.Invoke(slot));
        }
    }

    private void Start()
    {
        onSelectSlot.AddListener(OnSelectSlot);
    }

    private void OnEnable()
    {
        // 준비 씬일때만 호출
        if (SceneManager.GetActiveScene().name == "ReadyScene")
        {
            equip.gameObject.SetActive(true);

            int index = uiSlotList.IndexOf(equip.SaveCookie);

            if (index > -1)
            {
                equip.EquipCheckCookie(uiSlotList[index]);
            }
        }
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
        equip.saveGearIcon = null;
    }
}
