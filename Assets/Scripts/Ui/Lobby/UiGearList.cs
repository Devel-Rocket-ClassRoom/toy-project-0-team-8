using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiGearList : MonoBehaviour
{
    private List<UiGearSlot> uiSlotList = new List<UiGearSlot>();

    // 정렬을 위한 리스트
    private List<SaveGear> saveGearDataList = new List<SaveGear>();

    public UiGearSlot prefab;
    public ScrollRect scrollRect;

    private int selectedSlotIndex = -1;

    // 데이터 갱신을 위한 이벤트
    public UnityEvent onUpdateSlot;
    public UnityEvent<SaveGear> onSelectSlot;

    private void OnSelectSlot(SaveGear saveGear)
    {
        Debug.Log(saveGear);
        //HeroInfo.SetCharacterInfo(saveCharacter);
    }
    private void Start()
    {
        onSelectSlot.AddListener(OnSelectSlot);

    }

    private void OnDisable()
    {
        saveGearDataList = null;
    }

    public void SetSaveGearDataList(List<SaveGear> source)
    {

        saveGearDataList = source.ToList();
        UpdateSlots();
    }

    public List<SaveGear> GetSaveGearDataList()
    {
        return saveGearDataList;
    }

    public void ClearList()
    {
        for (int i = 0; i < uiSlotList.Count; i++)
        {
            uiSlotList[i].gameObject.SetActive(false);
        }
    }
    private void UpdateSlots()
    {
        // 필터링 할 때 사용
        //var list = saveCookieDataList.Where(filterings[(int)filter]).ToList();
        //list.Sort(comparison[(int)sorting]);


        var list = saveGearDataList;


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
                    onSelectSlot.Invoke(newSlot.SaveGearData);
                });

                uiSlotList.Add(newSlot);
            }
        }

        for (int i = 0; i < uiSlotList.Count; i++)
        {
            if (i < list.Count)
            {
                uiSlotList[i].gameObject.SetActive(true);
                uiSlotList[i].SetGear(list[i]);
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

}
