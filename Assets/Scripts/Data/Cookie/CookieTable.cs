using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CookieData
{
    public string Id {  get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public string Desc { get; set; }
    public Grade Grade { get; set; }
    public int Hp { get; set; }
    public CookieType Type { get; set; }

    public override string ToString()
    {
        return $"{Id} / {Name} / {Icon} / {Desc} / {Grade} / {Hp}";
    }
    public RuntimeAnimatorController AnimatorController => Resources.Load<RuntimeAnimatorController>($"Animations/Character/{Type.ToString()}/AnimationController");
    public string StringName => DataTableManager.CookieStringTable.Get(Name);
    public string StringDesc => DataTableManager.CookieStringTable.Get(Desc);
    public Sprite SpriteIcon => GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GachaManager>().LoadCookieSprite(Icon);
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
