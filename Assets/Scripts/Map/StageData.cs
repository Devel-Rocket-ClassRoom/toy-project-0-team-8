using UnityEngine;

[CreateAssetMenu(menuName = "CookieRun/Stage Data")]
public class StageData : ScriptableObject
{
    public Sprite Background;
    public float ScrollSpeed;
    public MapPrefab StagePrefab;
}
