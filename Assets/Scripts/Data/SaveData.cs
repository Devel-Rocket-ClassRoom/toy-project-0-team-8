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
    // 저장할 때 필요한 데이터
    public List<SaveCookie> CookieList = new List<SaveCookie>();
    public List<SaveGear> GearList = new List<SaveGear>();
    public int Cristal = 0;

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        return null;
    }
}

