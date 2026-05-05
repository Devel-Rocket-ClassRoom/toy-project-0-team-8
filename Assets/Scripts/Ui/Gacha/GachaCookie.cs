using UnityEngine;

[CreateAssetMenu(fileName = "GachaCookie", menuName = "Gacha/Cookie")]
public class GachaCookie : ScriptableObject
{
    public string cookieName;
    public string cookieId;
    public float weight;
    public Sprite Icon;

    // 스킬 스크립트도 여기 추가해서 써도 됌
}