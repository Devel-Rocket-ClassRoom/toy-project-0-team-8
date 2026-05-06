using System;
using UnityEngine;

public class MagnetItem : ItemBase {
    private GameManager _gameManager;

    protected override void Awake() {
        base.Awake();
        _gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    protected override float ItemDuration => 5f;
    
    protected override void ApplyItemEffect(CookieController other) {
        _gameManager.ActivateMagnet(ItemDuration);
    }
}
