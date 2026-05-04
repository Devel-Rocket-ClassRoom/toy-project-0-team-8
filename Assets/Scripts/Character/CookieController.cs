using System.Diagnostics.Tracing;
using UnityEngine;

public class CookieController : MonoBehaviour {
	private InGameCookieData _cookieData;
	private ICookieAbility _ability;
	private GameObject _cookie;
	
	private readonly float startX = -6f;
	private readonly float startY = -2.6f;
	
	public void Init(CookieData data) {
		_cookieData = new InGameCookieData(data);
		_ability = CookieAbilityFactory.Create(data.Id);
		
		// 캐릭터 모델 생성 후 플레이어 태그 달기
		_cookie = Instantiate(_cookieData.Prefab, new Vector3(startX, startY, 0f), Quaternion.identity);
		_cookie.tag = Tags.Player;
		
		// GameManager받아와서 Event 넣기
		_ability.Init();
	}
	
	
}