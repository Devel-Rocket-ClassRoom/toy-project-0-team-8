using UnityEngine;

public class CherryJelly : ItemBase
{
    public int scoreAmount = 200;
    protected override float ItemDuration => 0f;
    protected override void ApplyItemEffect(CookieController other)
    {
        Debug.Log($"젤리 먹음");
        _gameManager.AddScore(scoreAmount);
    }
        
}
