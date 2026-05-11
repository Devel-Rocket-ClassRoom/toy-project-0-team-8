public class Jelly : ItemBase
{
    private readonly int _scoreAmount = 100;
    protected override float ItemDuration => 0f;

    protected override void ApplyItemEffect(CookieController other)
    {
        _gameManager.AddScore(_scoreAmount);
    }
}
