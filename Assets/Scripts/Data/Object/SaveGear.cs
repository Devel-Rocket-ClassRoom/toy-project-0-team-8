using Newtonsoft.Json;
using System;
using UnityEngine;

public class SaveGear
{
    public Guid InstanceId { get; set; }

    [JsonConverter(typeof(GearDataConverter))]
    public GearData GearData;
    public DateTime creationTime { get; set; }


    public SaveGear()
    {
        // json에서 알아서 직렬화, 역직렬화 처리해줌
        InstanceId = Guid.NewGuid();
        creationTime = DateTime.Now;
    }


    public override string ToString()
    {
        return $"{InstanceId}\n{creationTime}\n{GearData.Id}";
    }
}
