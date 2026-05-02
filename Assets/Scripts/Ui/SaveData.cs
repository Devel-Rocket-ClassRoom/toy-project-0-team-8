using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using static UnityEditor.Progress;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

[System.Serializable]
public abstract class SaveData
{
    public int Version { get; protected set; }
    public abstract SaveData VersionUp();
}


[System.Serializable]
public class SaveDataV1 : SaveData
{
    public string PlayerName {  get; set; } = string.Empty;
    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        // 다음 버젼 호출
        var saveData = new SaveDataV2();

        saveData.Name = PlayerName;
        return saveData;
    }
}

[System.Serializable]
public class SaveDataV2 : SaveData
{
    public string Name {  get; set; } = string.Empty;
    public int Gold = 0;

    public SaveDataV2()
    {
        Version = 2;
    }

    public override SaveData VersionUp()
    {
        var saveData = new SaveDataV3();

        // itemTable 파일 불러와서 리스트에 넣기
        string path = string.Format(DataTable.FormatPath, DataTableIds.Item);
        TextAsset textAsset = Resources.Load<TextAsset>(path);

       saveData.list = DataTable.LoadCSV<ItemData>(textAsset.text);
      

        // id 저장
        int index = Random.Range(0, saveData.list.Count);
        saveData.ItemId = saveData.list[index].Id;

        return saveData;
    }
}

[System.Serializable]
public class SaveDataV3 : SaveData
{
    public string Name {  get; set; } = string.Empty;
    public int Gold = 0;
    public string ItemId {  get; set; } = string.Empty;
    public List<ItemData> list = new List<ItemData>();
    public SaveDataV3()
    {
        Version = 3;

    }

    public override SaveData VersionUp()
    {
        SaveDataV4 data = new SaveDataV4();
        data.Name = Name;
        data.Gold = Gold;
        foreach (ItemData item in list)
        {
            SaveItemData itemData = new SaveItemData();
            itemData.ItemData = DataTableManager.ItemTable.Get(item.Id);
            data.ItemList.Add(itemData);


        }
        return data;
    }
}

[System.Serializable]
public class SaveDataV4 : SaveDataV3
{
    public List<SaveItemData> ItemList = new List<SaveItemData>();
    public UiInvenSlotList.SortingOptions sortingOptions;
    public UiInvenSlotList.FilteringOptions filteringOptions;

    public List<SaveCharacter> CharacterList = new List<SaveCharacter>();
    public UiHeroList.SortingOptions characterSortingOptions;
    public UiHeroList.FilteringOptions characterFilteringOptions;

    public SaveDataV4()
    {
        Version = 4;

    }

    public override SaveData VersionUp()
    {
        return new SaveDataV3();
    }
}





//수업내용
public class SaveDataV3_1 : SaveDataV2
{
    public List<ItemData> list = new List<ItemData>();
    public SaveDataV3_1()
    {
        Version = 3;

    }

    public override SaveData VersionUp()
    {
        return new SaveDataV3_1();
    }
}