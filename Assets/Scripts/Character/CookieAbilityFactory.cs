public static class CookieAbilityFactory {
	public static ICookieAbility Create(string cookieId) {
		return cookieId switch {
			"Cookie_Pirate" => new PirateCookieAbility(),
		};
	}	
}