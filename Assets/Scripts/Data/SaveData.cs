using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;
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

    public Dictionary<string, int> CookieList = new Dictionary<string, int>()
    {
        // 쿠키 레벨
        {"Cookie_Pirate", 0},
        {"Cookie_Hero", 0},
        {"Cookie_Cherry", 0},
    };
    public Dictionary<string, int> GearList = new Dictionary<string, int>()
    {
        // 보물 레벨
        {"Gear_JellyPot", 0},
        {"Gear_MagnetMachine", 0},
        {"Gear_EnergyBooster", 0},
    };
    public int Cristal = 0;
    public int Coin = 0;
    public int score = 0;

    public string currentCookie = "Cookie_Pirate";
    public string[] currentGear = new string[]
    {
        "Blank",
        "Blank",
        "Blank"
    };
    public int currentExp = 0;

    public string lobbyCookieId = "Cookie_Pirate";
    public int playerLevel = 1;

    public SaveDataV1()
    {
        Version = 1;
    }

    public override SaveData VersionUp()
    {
        return null;
    }
}

