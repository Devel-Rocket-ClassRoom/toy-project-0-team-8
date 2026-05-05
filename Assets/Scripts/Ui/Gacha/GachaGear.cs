using UnityEngine;

[CreateAssetMenu(fileName = "GachaGear", menuName = "Gacha/Gear")]
public class GachaGear : ScriptableObject
{
    public string itemName;
    public string itemId;
    public float weight;
    public Sprite Icon;
}
