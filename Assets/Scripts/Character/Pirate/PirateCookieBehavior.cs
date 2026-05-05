using UnityEngine;

public class PirateCookieBehavior : CookieBehavior {
	private Animator _animator;
	
	private readonly int _isJumping = Animator.StringToHash("isJumping");
	private readonly int _isSliding = Animator.StringToHash("isSliding");
	private readonly int _isDoubleJumping = Animator.StringToHash("isDoubleJumping");
	
	public override void Init(CookieController controller, CookieData data) {
		base.Init(controller, data);
		
		_animator = GetComponent<Animator>();
	}
	
	public override void StartJumpAnimation() {
		_animator.SetBool(_isJumping, true);
	}
	
	public override void StartRunAnimation() {
		_animator.SetBool(_isJumping, false);
		_animator.SetBool(_isDoubleJumping, false);
		_animator.SetBool(_isSliding, false);
	}
	
	public override void StartDoubleJumpAnimation() {
		_animator.SetBool(_isDoubleJumping, true);
	}

	public override void StartSlidingAnimation() {
		_animator.SetBool(_isSliding, true);
	}
}