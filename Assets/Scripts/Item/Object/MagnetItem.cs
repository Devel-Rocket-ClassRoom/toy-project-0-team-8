using System;
using UnityEngine;

public class MagnetItem : ItemBase
{
    protected override float ItemDuration => 5f;

    protected override void ApplyItemEffect(CookieController other)
    {
        _gameManager.ActivateMagnet(ItemDuration);
    }
}
