using Newtonsoft.Json;
using System;
using UnityEngine;

public class SaveCharacter
{
    public Guid InstanceId { get; set; }

    [JsonConverter(typeof(CharacterDataConverter))]
    public CharacterData CharacterData;
    public DateTime creationTime { get; set; }


    public SaveCharacter()
    {
        // json에서 알아서 직렬화, 역직렬화 처리해줌
        InstanceId = Guid.NewGuid();
        creationTime = DateTime.Now;
    }

    public static SaveCharacter GetRandomItem()
    {
        SaveCharacter newCharacter = new SaveCharacter();
        newCharacter.CharacterData = DataTableManager.CharacterTable.GetRandom();

        Debug.Log("New Character Created: " + newCharacter.CharacterData.Id);
        return newCharacter;
    }

    public override string ToString()
    {
        return $"{InstanceId}\n{creationTime}\n{CharacterData.Id}";
    }
}
