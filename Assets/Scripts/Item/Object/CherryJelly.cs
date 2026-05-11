public class CherryJelly : ItemBase
{
    public int scoreAmount = 200;
    protected override float ItemDuration => 0f;

    protected override void ApplyItemEffect(CookieController other)
    {
        _gameManager.AddScore(scoreAmount);
    }
}
