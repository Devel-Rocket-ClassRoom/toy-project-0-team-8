using UnityEngine;

public class CherryJelly : ItemBase
{

    protected override float ItemDuration => 0f;
    protected override void ApplyItemEffect(CookieController other)
    {
        Debug.Log($"젤리 먹음");
    }
        
}
