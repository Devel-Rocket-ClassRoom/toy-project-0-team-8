using UnityEngine;

public class InGameCookieData {
	public CookieData baseData { get; }
	
	public float MaxHp { get; private set; }
	public float CurrentHp { get; private set; }
	
	public string Id => baseData.Id;
	public string Name => baseData.Name;
	public Grade Grade => baseData.Grade;
	public Sprite Icon => baseData.SpriteIcon;
	public GameObject Prefab => baseData.CookiePrefab;
	
	public bool IsDeadDelayed { get; private set; }
	public bool IsDead => CurrentHp <= 0;
	public bool IsGodMode { get; private set; }
	
	public InGameCookieData(CookieData data) {
		baseData = data;
		
		MaxHp = baseData.Hp;
		CurrentHp = MaxHp;
	}
	
	public void TakeDamage(float damage) {
		if (IsGodMode) return;
		CurrentHp -= damage;
	}
	
	public void RecoverHp(float amount) {
		CurrentHp += amount;
	}
}