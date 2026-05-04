public interface ICookieAbility {
	void OnInit(CookieController owner, InGameCookieData data) { }
	void OnUpdate(float deltaTime, CookieController owner, InGameCookieData data) { }
	void OnTakeDamage(int damage, CookieController owner, InGameCookieData data) { }
	void OnDead(CookieController owner, InGameCookieData data) { }
}