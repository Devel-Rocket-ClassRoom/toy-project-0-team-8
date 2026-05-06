public class DashItem : ItemBase {
	
	protected override float ItemDuration => 5f;
	protected override void ApplyItemEffect(CookieController other) {
			_gameManager.ActivateDash(ItemDuration);
	}
}