using UnityEngine;

public abstract class CookieBehavior : MonoBehaviour {
	
	protected CookieController _controller;
	protected CookieData _data;
	
	public void Init(CookieController controller, CookieData data) {
		_controller = controller;
		_data = data;
	}
	
	public abstract void StartJumpAnimation();
	
	public abstract void StartRun();
	
	public abstract void StartDoubleJump();
}