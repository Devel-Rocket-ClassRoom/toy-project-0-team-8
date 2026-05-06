public class GiantItem : ItemBase {
	protected override float ItemDuration => 5f;

	protected override void ApplyItemEffect(CookieController other) {
		_gameManager.ActivateGiant(ItemDuration);
	}

	protected override void RemoveItemEffect(CookieController other) {
		
	}
}