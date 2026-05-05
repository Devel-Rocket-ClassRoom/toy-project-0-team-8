using UnityEngine;

public abstract class CookieBehavior : MonoBehaviour {
	
	protected CookieController _controller;
	protected CookieData _data;
	
	public virtual void Init(CookieController controller, CookieData data) {
		_controller = controller;
		_data = data;
	}
	
	public abstract void StartJumpAnimation();
	
	public abstract void StartRunAnimation();
	
	public abstract void StartDoubleJumpAnimation();
	
	public abstract void StartSlidingAnimation();
	
	// 캐릭터가 사망했는지 아닌지 체크하기 위함. 특정 쿠키는 능력 사용중에 죽으면 안되고, 누구는 죽으면 살아나고 해야 해서 공통 로직으로 분리하였음
	public abstract bool DeathCheck();
}