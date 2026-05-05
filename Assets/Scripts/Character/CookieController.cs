using System;
using UnityEngine;

// 모든 쿠키가 베이스로 사용할 클래스입니다.
// 쿠키에는 필수적으로 Rigidbody2D, Animator, BoxCollider2D가 붙어있어야 합니다.
[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(BoxCollider2D))]
public class CookieController : MonoBehaviour {
	private int MaxHealth { get; set; }
	private int CurrentHp { get; set; }

	[SerializeField] private KeyCode _jumpKey = KeyCode.Space;
	[SerializeField] private KeyCode _slideKey = KeyCode.LeftControl;

	[Header("=== 디버그용 변수 ===")]
	[SerializeField] private float _jumpForce;
	[SerializeField] private float _gravityScale;
	
	private Rigidbody2D _rigidBody;
	private BoxCollider2D _collider;
	private CookieBehavior _cookieBehavior;
	private Animator _animator;
	
	private bool _isJumping;
	private bool _isDoubleJumping;
	private bool _isSliding;
	private bool _isGodMode;
	
	private readonly float _startXPos = -6f;
	private readonly float _startYPos = -2.5856f;
	private readonly float _slidingYPos = -3.16f;
	
	private readonly float _colliderYSize = 2f;
	private readonly float _slidingColliderYSize = 1.22f;
	
	
	public void Init(CookieData data) {
		MaxHealth = data.Hp;
		CurrentHp = MaxHealth;
		
		// Factory 이용해서 data에 맞는 Behavior 붙이기
		CookieBehaviorFactory.AddBehavior(gameObject, data);
		
		_rigidBody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<BoxCollider2D>();
		_cookieBehavior = GetComponent<CookieBehavior>();
		_animator = GetComponent<Animator>();
		
		_rigidBody.gravityScale = _gravityScale;
		
		_cookieBehavior.Init(this, data);
		_animator.runtimeAnimatorController = data.AnimatorController;
		
		
	}
	
	// 체력 감소
	public void TakeDamage(int amount) {
		CurrentHp -= amount;
		CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHealth);
	}
	
	protected void EnableGodMode(float time) {
		
	}
	
	// 체력 증가
	public void RecoverHp(int amount) {
		CurrentHp += amount;
		CurrentHp = Mathf.Clamp(CurrentHp, 0, MaxHealth);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// 땅에 닿으면 점프 초기화
		if (other.CompareTag(Tags.Ground) && _isJumping) {
			Debug.Log($"땅에 닿음");
			_isJumping = false;
			_isDoubleJumping = false;
			
			_cookieBehavior.StartRunAnimation();
		}
		
		if (other.CompareTag(Tags.Obstacle)) {
			TakeDamage(20);
		}
	}

	private void Update() {
		// 점프 키 누르면 점프, 단 슬라이딩 중엔 점프 불가능
		if (Input.GetKeyDown(_jumpKey) && !_isSliding) {
			if (!_isJumping && !_isDoubleJumping) {
				_isJumping = true;
				_cookieBehavior.StartJumpAnimation();
				
				_rigidBody.linearVelocity = new Vector2(0, _jumpForce);
			}
			
			else if (_isJumping && !_isDoubleJumping) {
				_isDoubleJumping = true;
				_cookieBehavior.StartDoubleJumpAnimation();
				
				_rigidBody.linearVelocity = new Vector2(0, _jumpForce);
			}
		}
		
		// 슬라이드 키 누르면 슬라이드 시작
		if (Input.GetKeyDown(_slideKey) && !_isJumping) {
			_isSliding = true;
			
			transform.position = new Vector3(transform.position.x, _slidingYPos, transform.position.z);
			_collider.size = new Vector2(_collider.size.x, _slidingColliderYSize);
			_cookieBehavior.StartSlidingAnimation();
		}
		
		if (Input.GetKeyUp(_slideKey)) {
			_isSliding = false;
			
			transform.position = new Vector3(transform.position.x, _startYPos, transform.position.z);
			_collider.size = new Vector2(_collider.size.x, _colliderYSize);
			_cookieBehavior.StartRunAnimation();
		}
	}

	private void FixedUpdate() {

	}

	private void RigidbodyJump() {
		
	}
	
	
}