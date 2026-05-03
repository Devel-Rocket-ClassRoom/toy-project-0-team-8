using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiCookieList : MonoBehaviour
{
    private List<UiCookieSlot> uiSlotList = new List<UiCookieSlot>();

    // 정렬을 위한 리스트
    private List<SaveCookie> saveCookieDataList = new List<SaveCookie>();

    public UiCookieSlot prefab;
    public ScrollRect scrollRect;

    private int selectedSlotIndex = -1;

    // 데이터 갱신을 위한 이벤트
    public UnityEvent onUpdateSlot;
    public UnityEvent<SaveCookie> onSelectSlot;

    private void OnSelectSlot(SaveCookie saveCookie)
    {
        Debug.Log(saveCookie);
        //HeroInfo.SetCharacterInfo(saveCharacter);
    }
    private void Start()
    {
        onSelectSlot.AddListener(OnSelectSlot);

    }

    private void OnDisable()
    {
        saveCookieDataList = null;
    }

    public void SetSaveCookieDataList(List<SaveCookie> source)
    {
        saveCookieDataList = source.ToList();
        UpdateSlots();
    }

    public List<SaveCookie> GetSaveCookieDataList()
    {
        return saveCookieDataList;
    }

    private void UpdateSlots()
    {
        // 필터링 할 때 사용
        //var list = saveCookieDataList.Where(filterings[(int)filter]).ToList();
        //list.Sort(comparison[(int)sorting]);

        var list = saveCookieDataList;
        // 아이템 리스트를 받아서 슬롯 리스트 생성
        if (uiSlotList.Count < list.Count)
        {
            for (int i = uiSlotList.Count; i < list.Count; i++)
            {
                var newSlot = Instantiate(prefab, scrollRect.content);

                newSlot.slotIndex = i;
                newSlot.SetEmpty();
                newSlot.gameObject.SetActive(false);

                newSlot.button.onClick.AddListener(() =>
                {
                    selectedSlotIndex = newSlot.slotIndex;
                    onSelectSlot.Invoke(newSlot.SaveCookieData);
                });

                uiSlotList.Add(newSlot);
            }
        }

        for (int i = 0; i < uiSlotList.Count; i++)
        {
            if (i < list.Count)
            {
                uiSlotList[i].gameObject.SetActive(true);
                uiSlotList[i].SetCookie(list[i]);
            }
            else
            {
                uiSlotList[i].gameObject.SetActive(false);
                uiSlotList[i].SetEmpty();
            }

        }

        selectedSlotIndex = -1;
        onUpdateSlot.Invoke();
    }


    public void AddRandomItem()
    {
        //saveCookieDataList.Add(SaveCookie.GetRandomItem());
        UpdateSlots();
    }

}
