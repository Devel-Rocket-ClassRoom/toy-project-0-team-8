using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "GachaItem", menuName = "Gacha/Item")]
public class Gacha : ScriptableObject
{
    public string itemName;
    public string itemId;
    public float weight;
    public Sprite Icon;
}


public class GachaManager : MonoBehaviour
{
    private float totalWeight;
    private int lastIndex = -1;
    public List<Gacha> itemList;
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