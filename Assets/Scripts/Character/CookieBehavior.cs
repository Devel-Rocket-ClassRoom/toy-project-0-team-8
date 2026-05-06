using UnityEngine;
using UnityEngine.UI;

public abstract class CookieBehavior : MonoBehaviour {
	
	protected CookieController _controller;
	protected GameManager _gameManager;
	
	public virtual void Init(CookieController controller) {
		_controller = controller;
		_gameManager = GameObject.FindWithTag(Tags.GameManager).GetComponent<GameManager>();
	}
	
	/// <summary>
	/// 능력이 얼마나 준비되었는지 보여주는 ProgressBar를 사용하는가?
	/// </summary>
	public abstract bool UseAbilityProgressBar { get; }
	/// <summary>
	/// 사용하는 경우에는, 0 ~ 1 사이 값으로 현재 진행도 바는 얼마의 값인가?
	/// </summary>
	public abstract float GetProgressbarAmount();
	public abstract void StartJumpAnimation();
	public abstract void StartRunAnimation();
	public abstract void StartDoubleJumpAnimation();
	public abstract void StartSlidingAnimation();
	public abstract void StartDeathAnimation();
	public abstract void StartDashAnimation();
	
	// 캐릭터가 사망했는지 아닌지 체크하기 위함. 특정 쿠키는 능력 사용중에 죽으면 안되고, 누구는 죽으면 살아나고 해야 해서 공통 로직으로 분리하였음
	// 별도 사망 미루기 로직이 없다면, 재구현 안해도 됨
	public virtual bool DeathCheck() {
		return _controller.CurrentHp <= 0 && _controller.AdditionalHp <= 0;
	}
}