using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
// 1. CSV 파일 (ID / 이름 / 설명 / 공격력.. / 아이콘 기타 등등
// 2. DataTable 상속
// 3. DataTableManager 등록
// 4. 테스트 패널


public class CharacterData
{
    public string Id {  get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public int Attack { get; set; }
    public string Icon { get; set; }

    public Grade Grade { get; set; }

    public override string ToString()
    {
        return $"{Id} / {Name} / {Desc} / {Attack} / {Icon} / {Grade}";
    }
    public string StringName => DataTableManager.StringTable.Get(Name);
    public string StringDesc => DataTableManager.StringTable.Get(Desc);
    public Sprite SpriteIcon => Resources.Load<Sprite>($"Icon/{Icon}");
}


public class CharacterTable : DataTable
{
    private readonly Dictionary<string, CharacterData> table = new Dictionary<string, CharacterData>();



    private List<CharacterData> gradeList = new List<CharacterData>();

    public override void Load(string filename)
    {
        table.Clear();

        string path = string.Format(FormatPath, filename);
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        List<CharacterData> list = LoadCSV<CharacterData>(textAsset.text);

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

    public CharacterData Get(string id)
    {
        if (!table.ContainsKey(id))
        {
            Debug.LogError("캐릭터 아이디 없음");
            return null;
        }

        return table[id];
    }

    public CharacterData GetRandom()
    {
        // 등급별로 랜덤 뽑기
        CharacterData character = new CharacterData();
        Grade targetGrade;

        string id = string.Empty;
        float randomValue = Random.value;

        if(randomValue < 0.7f)
        {
            targetGrade = Grade.Common;
        }
        else if(randomValue < 0.9f)
        {
            targetGrade = Grade.Rare;
        }
        else
        {
            targetGrade = Grade.Epic;
        }

        var filteredList = gradeList.Where(x => x.Grade == targetGrade).ToList();



        if (filteredList.Count == 0)
        {
            Debug.LogWarning($"{targetGrade} 등급의 캐릭터 데이터가 없습니다");
            return null;
        }

        // 4. 랜덤 반환
        return filteredList[Random.Range(0, filteredList.Count)];
    }
}
