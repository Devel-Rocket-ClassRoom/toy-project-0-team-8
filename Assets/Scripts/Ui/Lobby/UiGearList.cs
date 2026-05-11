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

    public List<UiGearSlot> uiSlotList = new List<UiGearSlot>();

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

        if (equip.gameObject.activeSelf)
        {
            Debug.Log(saveGear.GearData.itemName);
            equip.EquipGear(saveGear);
        }
    }

    private void Awake()
    {
        foreach (var slot in scrollRect.content.GetComponentsInChildren<UiGearSlot>(true))
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

            for (int i = 0; i < equip.changeScene.equipGear.Length; i++)
            {
                int index = uiSlotList.IndexOf(equip.changeScene.equipGear[i]);

                if (index > -1)
                {
                    equip.EquipCheck(uiSlotList[index]);
                }
            }
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
        if (equip.SaveCookie != null)
        {
            equip.SaveCookie = null;
        }
    }
}
