using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GearData
{
    public string Id {  get; set; }
    public string Name { get; set; }    
    public string Desc {  get; set; }
    public Grade Grade { get; set; }
    public int CoolTime { get; set; }
    public string Icon {  get; set; }

    public override string ToString()
    {
        return $"{Id} / {Name} / {Desc} / {Grade} / {CoolTime} / {Icon}";
    }

    public string StringName => DataTableManager.GearStringTable.Get(Name);
    public string StringDesc => DataTableManager.GearStringTable.Get(Desc);

    // 경로 수정 필요함
    public Sprite SpriteIcon => GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GachaManager>().LoadGearSprite(Icon);
}

public class GearTable : DataTable
{
    public readonly Dictionary<string, GearData> table = new Dictionary<string, GearData>();


    private List<string> keyList;

    public override void Load(string filename)
    {
        table.Clear();

        string path = string.Format(FormatPath, filename);
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        List<GearData> list = LoadCSV<GearData>(textAsset.text);

        foreach(var item in list)
        {
            if(!table.ContainsKey(item.Id))
            {
                table.Add(item.Id, item);
            }
            else
            {
                Debug.LogError("아이템 아이디 중복");
            }
        }

        keyList = table.Keys.ToList();
    }

    public GearData Get(string id)
    {
        if(!table.ContainsKey(id))
        {
            Debug.LogError("아이템 아이디 없음");
            return null;
        }

        return table[id];
    }

    public GearData GetRandom()
    {
        return Get(keyList[Random.Range(0,keyList.Count)]);
    }
}