using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CookieController))]
public class CookieGearController : MonoBehaviour
{
    [Header("=== 보물 프리팹 생성 ===")]
    public Transform gearPrefabParent;

    public void SetGear(GameManager gameManager)
    {
        string[] gearIds = SaveLoadManager.Data.currentGear;
        List<GearBase> gears = new List<GearBase>();
        List<GearData> gearDatas = new List<GearData>();

        foreach (string id in gearIds)
        {
            if (string.IsNullOrEmpty(id))
                continue;

            // 추하게 들고오긴 했음.. (알려진 이슈)
            var gearData = DataTableManager.GearTable.Get(id);
            var gearPrefab = GameObject
                .FindGameObjectWithTag("SceneManager")
                .GetComponent<GachaManager>()
                .itemList.Find(g => g.itemId == id)
                .GearPrefab;
            gearDatas.Add(gearData);

            if (gearData != null && gearPrefab != null)
            {
                GameObject gearObj = Instantiate(gearPrefab, gearPrefabParent);
                gears.Add(gearObj.GetComponent<GearBase>());
                Debug.Log($"{id} 보물이 {gearPrefabParent.name} 아래에 생성되었습니다.");
            }
        }

        gameManager.SetGears(gears);
        gameManager.SetGearImages(gearDatas);
    }
}
