using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Gacha
{
    public string itemName;
    public float weight;
    public Image Icon;
}


public class GachaManager : MonoBehaviour
{
    private float totalWeight;
    private int lastIndex = -1;
    public List<Gacha> itemList = new List<Gacha>();
    private List<Gacha> dropHistory = new List<Gacha>();
    public Gacha GachaItem()
    {
        totalWeight = 0f;
        foreach (var item in itemList)
        {
            totalWeight += item.weight;
        }
        float pivot = Random.Range(0, totalWeight);
        for (int i = 0; i < itemList.Count; i++)
        {
            if (pivot <= itemList[i].weight)
            {
                lastIndex = i;
                break;
            }
            pivot -= itemList[i].weight;
        }
        if (lastIndex != -1)
        {
            Gacha result = itemList[lastIndex];
            dropHistory.Add(result);
            return result;
        }
        return null;
    }
}