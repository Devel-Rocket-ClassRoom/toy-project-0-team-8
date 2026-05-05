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
}