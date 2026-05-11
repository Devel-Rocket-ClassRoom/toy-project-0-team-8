public class Coin : ItemBase
{
    private readonly int _coinAmount = 1;

    protected override float ItemDuration => 0f;

    protected override void ApplyItemEffect(CookieController other)
    {
        _gameManager.AddCoin(_coinAmount);
    }
}
