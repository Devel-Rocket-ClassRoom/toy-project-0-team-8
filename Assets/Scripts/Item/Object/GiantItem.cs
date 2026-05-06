using UnityEngine;

public class GiantItem : ItemBase {
	protected override float ItemDuration => 5f;
	
	private GameManager _gameManager;

	protected override void Awake() {
		base.Awake();
		_gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
	}

	protected override void ApplyItemEffect(CookieController other) {
		_gameManager.ActivateGiant(ItemDuration);
	}

	protected override void RemoveItemEffect(CookieController other) {
		
	}
}