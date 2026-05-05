using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "GachaGear", menuName = "Gacha/Gear")]
public class GachaGear : ScriptableObject
{
    public string itemName;
    public string itemId;
    public float weight;
    public Sprite Icon;
}

[CreateAssetMenu(fileName = "GachaCookie", menuName = "Gacha/Cookie")]
public class GachaCookie : ScriptableObject
{
    public string cookieName;
    public string cookieId;
    public float weight;
    public Sprite Icon;

    // 스킬 스크립트도 여기 추가해서 써도 됌
}


public class GachaManager : MonoBehaviour
{
    private float totalWeight;
    private int lastIndex = -1;
    public List<GachaGear> itemList;
    private List<GachaGear> dropHistoryGear = new List<GachaGear>();

    public List<GachaCookie> cookieList;
    private List<GachaCookie> dropHistoryCookie = new List<GachaCookie>();

    // 나중에 쿠키 뽑는 함수도 만들기
    public GachaGear GachaItem()
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
            GachaGear result = itemList[lastIndex];
            dropHistoryGear.Add(result);
            return result;
        }
        return null;
    }


    // 유료에셋이 있어서 기존에 쓰던 Resources.Load로 불러오는 방식은 못 씀..
    // 그래서 스크립터블 오브젝트에 등록된 이미지를 불러오는 함수 등록
    public Sprite LoadGearSprite(string soName)
    {
        return itemList.Find(item => item.name == soName).Icon;
    }
    public Sprite LoadCookieSprite(string soName)
    {
        return cookieList.Find(item => item.name == soName).Icon;
    }
}