public class SmallHealthPotion : ItemBase {
	private readonly float _healAmount = 30f;

	protected override float ItemDuration => 0f;

	protected override void ApplyItemEffect(CookieController other) {
		other.RecoverHp(_healAmount);
	}
}