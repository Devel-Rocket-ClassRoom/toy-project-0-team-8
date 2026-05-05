using System.Collections;
using UnityEngine;

// 모든 쿠키가 베이스로 사용할 클래스입니다.
// 쿠키에는 필수적으로 Rigidbody2D, Animator, BoxCollider2D가 붙어있어야 합니다.
[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(BoxCollider2D))]
public class CookieController : MonoBehaviour {
	private int MaxHealth { get; set; }
	private int CurrentHp { get; set; }

	[SerializeField] private KeyCode _jumpKey = KeyCode.Space;
	[SerializeField] private KeyCode _slideKey = KeyCode.LeftControl;
	
	private Rigidbody2D _rigidBody;
	private BoxCollider2D _collider;
	private Animator _animator;
	private CookieBehavior _cookieBehavior;
	
	private bool _isJumping;
	private bool _isDoubleJumping;
	private bool _isGodMode;
	
	private readonly float startX = -6f;
	private readonly float startY = -2.6f;
	
	public void Init(CookieData data) {
		MaxHealth = data.Hp;
		CurrentHp = MaxHealth;
	}
	
	// 체력 감소
	protected virtual void TakeDamage(int amount) {
		CurrentHp -= amount;
		CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHealth);
	}
	
	protected void EnableGodMode(float time) {
		
	}
	
	// 체력 증가
	protected virtual void RecoverHp(int amount) {
		CurrentHp += amount;
		CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHealth);
	}
	
	public CookieController(CookieData cookieData) {

	}

	private void OnTriggerEnter(Collider other) {
		// 땅에 닿으면 점프 초기화
		if (other.CompareTag(Tags.Ground)) {
			_isJumping = false;
			_isDoubleJumping = false;
			_cookieBehavior.StartRun();
		}
		
		if (other.CompareTag(Tags.Obstacle)) {
			TakeDamage(20);
		}
	}

	protected virtual void Update() {
		// 점프 키 누르면 점프
		if (Input.GetKeyDown(_jumpKey)) {
			if (!_isJumping && !_isDoubleJumping) {
				_isJumping = true;
				_cookieBehavior.StartJumpAnimation();
				RigidbodyJump();
			}
		}
		
		else if (Input.GetKeyDown(_slideKey)) {
			
		}
	}
	
	private void RigidbodyJump() {
		
	}
}