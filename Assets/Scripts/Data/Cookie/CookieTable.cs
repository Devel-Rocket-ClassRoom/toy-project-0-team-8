using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CookieData
{
    public string Id {  get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public int Hp { get; set; }
    public string Icon { get; set; }

    public Grade Grade { get; set; }

    public override string ToString()
    {
        return $"{Id} / {Name} / {Icon} / {Desc} / {Grade} / {Hp}";
    }
    public string StringName => DataTableManager.StringTable.Get(Name);
    public string StringDesc => DataTableManager.StringTable.Get(Desc);
    public Sprite SpriteIcon => Resources.Load<Sprite>($"Sprite/Ui/Lobby/{Icon}");
}


public class CookieTable : DataTable
{
    private readonly Dictionary<string, CookieData> table = new Dictionary<string, CookieData>();



    private List<CookieData> gradeList = new List<CookieData>();

    public override void Load(string filename)
    {
        table.Clear();

        string path = string.Format(FormatPath, filename);
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        List<CookieData> list = LoadCSV<CookieData>(textAsset.text);

        foreach (var character in list)
        {
            if (!table.ContainsKey(character.Id))
            {
                table.Add(character.Id, character);
                gradeList.Add(character);
            }
            else
            {
                Debug.LogError("캐릭터 아이디 중복");
            }
        }


    }

    public CookieData Get(string id)
    {
        if (!table.ContainsKey(id))
        {
            Debug.LogError("캐릭터 아이디 없음");
            return null;
        }

        return table[id];
    }

}
