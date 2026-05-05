using UnityEngine;
using UnityEngine.UI;

// 모든 쿠키가 베이스로 사용할 클래스입니다.
// 쿠키에는 필수적으로 Rigidbody2D, Animator, BoxCollider2D가 붙어있어야 합니다.
[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(BoxCollider2D))]
public class CookieController : MonoBehaviour {
	// UI가 표현 가능한 최대체력을 정의
	private readonly float UiMaxHp = 100f;
	private float MaxHp { get; set; }
	private readonly float MaxAdditionalHp = 100f;
	private float _currentHp;
	private float _additionalHp;
	
	[Header("=== 체력바 관련 Image ===")]
	[SerializeField] private Image _hpBar;
	[SerializeField] private Image _additionalHpBar;
	
	[SerializeField] private KeyCode _jumpKey = KeyCode.Space;
	[SerializeField] private KeyCode _slideKey = KeyCode.LeftControl;

	[Header("=== 디버그용 변수 ===")]
	[SerializeField] private float _jumpForce = 17f;
	[SerializeField] private float _gravityScale = 5f;
	[SerializeField] private float _healthReduceSpeed = 2f;
	
	private Rigidbody2D _rigidBody;
	private BoxCollider2D _collider;
	private CookieBehavior _cookieBehavior;
	private Animator _animator;
	private GameManager _gameManager;
	
	private bool _isJumping;
	private bool _isDoubleJumping;
	private bool _isSliding;
	private bool _isGodMode;
	private bool _jumpRequested;
	private bool _isDead;
	
	private readonly float _standingYPos = -2.735f;
	private readonly float _slidingYPos = -3.16f;
	
	private readonly float _healthBarHeight = 50f;
	
	private readonly float _standingColliderYSize = 2f;
	private readonly float _slidingColliderYSize = 1.22f;
	
	// 체력 관련 값 세팅시에는, Clamping해서 범위 넘어가지 않도록 함
	public float CurrentHp {
		get => _currentHp;
		private set => _currentHp = Mathf.Clamp(value, 0, MaxHp);
	}
	public float AdditionalHp {
		get => _additionalHp;
		private set => _additionalHp = Mathf.Clamp(value, 0, MaxAdditionalHp);
	}
	
	private float UiHpPercent => CurrentHp / UiMaxHp;
	private float AdditionalHpPercent => AdditionalHp / MaxAdditionalHp; 
	
	
	public void Init(CookieData data) {
		_gameManager = GameObject.FindWithTag(Tags.GameManager).GetComponent<GameManager>();
		
		MaxHp = data.Hp;
		CurrentHp = MaxHp;
		
		// Factory 이용해서 data에 맞는 Behavior 붙이기
		CookieBehaviorFactory.AddBehavior(gameObject, data);
		
		_rigidBody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<BoxCollider2D>();
		_cookieBehavior = GetComponent<CookieBehavior>();
		_animator = GetComponent<Animator>();
		
		_rigidBody.gravityScale = _gravityScale;
		
		_cookieBehavior.Init(this);
		_animator.runtimeAnimatorController = data.AnimatorController;
	}
	
	// 체력 감소
	public void TakeDamage(float amount) {
		// 추가체력이 있다면, 그것부터 감소
		if (_additionalHp > 0) {
			float reducedAmount = Mathf.Clamp(amount, 0, _additionalHp);
			AdditionalHp -= reducedAmount;
			amount -= reducedAmount;
		}
		
		CurrentHp -= amount;
	}
	
	// 체력 증가
	public void RecoverHp(float amount) {
		CurrentHp += amount;
	}
	
	public void GetAdditionalHealth(float amount) {
		AdditionalHp += amount;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// 땅에 닿으면 점프 초기화
		if (other.CompareTag(Tags.Ground) && _isJumping) {
			Debug.Log($"땅에 닿음");
			_isJumping = false;
			_isDoubleJumping = false;
			
			_cookieBehavior.StartRunAnimation();
		}
	}

	private void Update() {
		// 체력 조금씩 줄이기. 추가체력 있으면 추가체력부터
		if (AdditionalHp > 0) {
			AdditionalHp -= _healthReduceSpeed * Time.deltaTime;
		} else {
			CurrentHp -= _healthReduceSpeed * Time.deltaTime;	
		}
		
		// 죽었는지 체크해서 게임 종료 알림
		if (_cookieBehavior.DeathCheck() && !_isDead) {
			_isDead = true;
			_gameManager.GameEndFlag = true;
			
			// 죽었을 때도 충돌 판정 줄여줘야함
			SetSlidingPosition();
			_cookieBehavior.StartDeathAnimation();
			return;
		}
		
		// UI도 줄이고, 위치 설정
		_hpBar.fillAmount = UiHpPercent;
		// 추가체력 위치 설정시에는 체력바 줄어든 값에 맞춰서 배치
		_additionalHpBar.rectTransform.anchoredPosition = new Vector2(_hpBar.rectTransform.anchoredPosition.x + _hpBar.rectTransform.sizeDelta.x * UiHpPercent, _additionalHpBar.rectTransform.anchoredPosition.y); 
		_additionalHpBar.fillAmount = AdditionalHpPercent;
		
		// Update에서는 점프 요청했는지 확인만, 물리 처리는 FixedUpdate에서
		if (Input.GetKeyDown(_jumpKey)) { RequestJump(); }
		// 슬라이드 키 누르면 슬라이드 시작		
		if (Input.GetKeyDown(_slideKey) && !_isJumping) { RequestSlidingStart(); }
		// 슬라이드 키 떼면 슬라이드 종료
		if (Input.GetKeyUp(_slideKey) && _isSliding) { RequestSlidingEnd(); }
		
		if (Input.GetKeyDown(KeyCode.A)) { GetAdditionalHealth(10); }
	}

	private void FixedUpdate() {
		// 점프 요청되었다면 점프
		if (_jumpRequested) {
			if (!_isJumping && !_isDoubleJumping) {
				_isSliding = false;
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
		_jumpRequested = false;
	}
	
	public void RequestJump() {
		_jumpRequested = true;
	}
	
	public void RequestSlidingStart() {
		_isSliding = true;
		
		SetSlidingPosition();
		_cookieBehavior.StartSlidingAnimation();
	}
	
	public void RequestSlidingEnd() {
		_isSliding = false;
			
		SetStandingPosition();
		_cookieBehavior.StartRunAnimation();
	}
	
	// 슬라이딩 시에 위치와 충돌 박스 크기 바꿔서 충돌 자연스럽게 하기
	public void SetSlidingPosition() {
		transform.position = new Vector3(transform.position.x, _slidingYPos, transform.position.z);
		_collider.size = new Vector2(_collider.size.x, _slidingColliderYSize);
	}
	
	// 위치, 충돌 박스 크기 원래대로
	public void SetStandingPosition() {
		transform.position = new Vector3(transform.position.x, _standingYPos, transform.position.z);
		_collider.size = new Vector2(_collider.size.x, _standingColliderYSize);
	}
}