using System.Collections.Generic;
using System.Data;
using UnityEngine;
using static Unity.VisualScripting.Icons;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables =
        new Dictionary<string, DataTable>();

    public static StringTable StringTable => Get<StringTable>(DataTableIds.String);

    public static ItemTable ItemTable => Get<ItemTable>(DataTableIds.Item);
    public static CharacterTable CharacterTable => Get<CharacterTable>(DataTableIds.character);

#if UNITY_EDITOR
    public static StringTable GetStringTable(Languages lang)
    {
        return Get<StringTable>(DataTableIds.StringTableIds[(int)lang]);
    }
#endif

    static DataTableManager()
    {
        Init();
    }

    private static void Init()
    {
#if !UNITY_EDITOR
        var stringTable = new StringTable();
        stringTable.Load(DataTableIds.String);
        tables.Add(DataTableIds.String, stringTable);
#else
        foreach (var id in DataTableIds.StringTableIds)
        {
            var stringTable = new StringTable();
            stringTable.Load(id);
            tables.Add(id, stringTable);
        }
#endif

        var itemTable = new ItemTable();
        itemTable.Load(DataTableIds.Item);
        tables.Add(DataTableIds.Item, itemTable);

        var characterTable = new CharacterTable();
        characterTable.Load(DataTableIds.character);
        tables.Add(DataTableIds.character, characterTable);

    }

    public static void ChangeLanguage(Languages lang)
    {
        string newId = DataTableIds.StringTableIds[(int)lang];
        if (tables.ContainsKey(newId))
            return;

        string oldId = string.Empty;
        foreach (var id in DataTableIds.StringTableIds)
        {
            if (tables.ContainsKey(id))
            {
                oldId = id;
                break;
            }
        }
        var stringTable = tables[oldId];
        stringTable.Load(newId);
        tables.Remove(oldId);
        tables.Add(newId, stringTable);


    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError("테이블 없음");
            return null;
        }
        return tables[id] as T;
    }
}

