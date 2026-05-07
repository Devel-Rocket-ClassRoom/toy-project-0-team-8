public class SmallHealthPotion : ItemBase {
	private readonly float _healAmount = 30f;

	protected override float ItemDuration => 0f;

	protected override void ApplyItemEffect(CookieController other) {
		// 상대가 회복 가능할때만 회복
		if (other.HealEnabled) { other.RecoverHp(_healAmount); }
	}
}