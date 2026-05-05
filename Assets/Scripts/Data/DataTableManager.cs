using System.Collections.Generic;
using System.Data;
using UnityEngine;

public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables =
        new Dictionary<string, DataTable>();

    public static StringTable CookieStringTable => Get<StringTable>("StringTableCookie");
    public static CookieTable CookieTable => Get<CookieTable>(DataTableIds.Cookie);

    public static StringTable GearStringTable => Get<StringTable>("StringTableGear");
    public static GearTable GearTable => Get<GearTable>(DataTableIds.Gear);
    

#if UNITY_EDITOR
    public static StringTable GetStringTable(DataType lang)
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
        foreach (var id in DataTableIds.StringTableIds)
        {
            var stringTable = new StringTable();
            stringTable.Load(id);
            tables.Add(id, stringTable);
        }

        var itemTable = new GearTable();
        itemTable.Load(DataTableIds.Gear);
        tables.Add(DataTableIds.Gear, itemTable);

        var characterTable = new CookieTable();
        characterTable.Load(DataTableIds.Cookie);
        tables.Add(DataTableIds.Cookie, characterTable);

    }

    public static void ChangeDataType(DataType dataType)
    {
        string newId = DataTableIds.StringTableIds[(int)dataType];
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

