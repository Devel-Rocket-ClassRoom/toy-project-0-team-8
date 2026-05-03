using Newtonsoft.Json;
using System;
using UnityEngine;

public class SaveCookie
{
    public Guid InstanceId { get; set; }

    [JsonConverter(typeof(CharacterDataConverter))]
    public CookieData CookieData;
    public DateTime creationTime { get; set; }


    public SaveCookie()
    {
        // json에서 알아서 직렬화, 역직렬화 처리해줌
        InstanceId = Guid.NewGuid();
        creationTime = DateTime.Now;
    }


    public override string ToString()
    {
        return $"{InstanceId}\n{creationTime}\n{CookieData.Id}";
    }
}
