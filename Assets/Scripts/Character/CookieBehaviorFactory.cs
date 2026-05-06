using UnityEngine;

public static class CookieBehaviorFactory {
	public static CookieBehavior AddBehavior(GameObject target, CookieData data) {
		return data.Type switch {
			CookieType.Pirate => target.AddComponent<PirateCookieBehavior>(),
			CookieType.Hero => target.AddComponent<HeroCookie>(),
		};
	}	
}