using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiGearList : MonoBehaviour
{
    // 끼고 있는 아이템에는 장착 중 표시를 해야함. 


    private List<UiGearSlot> uiSlotList = new List<UiGearSlot>();

    // 정렬을 위한 리스트
    private List<SaveGear> saveGearDataList = new List<SaveGear>();

    public UiGearSlot prefab;
    public ScrollRect scrollRect;

    private int selectedSlotIndex = -1;

    // 데이터 갱신을 위한 이벤트
    public UnityEvent onUpdateSlot;
    public UnityEvent<UiGearSlot> onSelectSlot;

    public EquipObject equip;

    private void OnSelectSlot(UiGearSlot saveGear)
    {
        Debug.Log(saveGear);
        if (equip.saveGear == null)
        {
            Debug.LogWarning("Equip.saveGear가 null이라 런타임에서 생성합니다.");
            equip.saveGear = new List<UiGearSlot>();
        }
        saveGear.selectMark.enabled = true;

        // 아이템 슬롯 프리펩에는 순번 표기용 ui도 활성화 하기
        if (equip.saveGear.Count < 3)
        {
            equip.saveGear.Add(saveGear);
        }
        else
        {
            // 4개 넘었을 시
            equip.saveGear.Add(saveGear);

            equip.saveGear[0].selectMark.enabled = false;
            equip.saveGear.RemoveAt(0);


        }

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
        // 쿠키 데이터 비활성화
        for (int i = 0; i < uiSlotList.Count; i++)
        {
            uiSlotList[i].gameObject.SetActive(false);
        }

        // 선택된 쿠키 할당 제거
        if(equip.saveCookie != null)
        {
            equip.saveCookie = null;
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
                    onSelectSlot.Invoke(newSlot);
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
