using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiGearList : MonoBehaviour
{
    // 끼고 있는 아이템에는 장착 중 표시를 해야함. 


    private List<UiGearSlot> uiSlotList = new List<UiGearSlot>();

    // 정렬을 위한 리스트

    public UiGearSlot prefab;
    public ScrollRect scrollRect;


    // 데이터 갱신을 위한 이벤트
    public UnityEvent onUpdateSlot;
    public UnityEvent<UiGearSlot> onSelectSlot;
    public UnityEvent<UiGearSlot> onEquipSlot;

    public EquipObject equip;

    private void OnSelectSlot(UiGearSlot saveGear)
    {
        Debug.Log(saveGear);

        // 준비 씬일때만 호출
        if (SceneManager.GetActiveScene().name == "ReadyScene")
        { 
            equip.gameObject.SetActive(true);
            EquipGear(saveGear); 
        }

    }

    public void EquipGear(UiGearSlot saveGear)
    {

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

        foreach (var slot in scrollRect.content.GetComponentsInChildren<UiGearSlot>(true))
        {
            uiSlotList.Add(slot);
        }
    }


    public void SetSaveGearDataList(Dictionary<string, int> source)
    {

        foreach (var slot in uiSlotList)
        {
            slot.level = source[slot.GearData.itemId];
            slot.SetGear();
        }

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


}
