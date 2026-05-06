using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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
	
	private readonly float _godModeDuration = 2f;
	
	private bool _isGodMode = false;
	
	[HideInInspector] public UnityEvent OnTakeDamage;
	
	// 이벤트 종류별로 모두 생성 (점프키 누르기, 떼기, 누르고있기, 슬라이드 누르기, 떼기, 누르고있기)
	[HideInInspector] public UnityEvent OnJumpKeyDown;
	[HideInInspector] public UnityEvent OnJumpKeyUp;
	[HideInInspector] public UnityEvent WhileJumpKeyPressed;
	[HideInInspector] public UnityEvent OnSlideKeyDown;
	[HideInInspector] public UnityEvent OnSlideKeyUp;
	[HideInInspector] public UnityEvent WhileSlideKeyPressed;
	
	[Header("=== 체력바 관련 Image ===")]
	[SerializeField] private Image _hpBar;
	[SerializeField] private Image _additionalHpBar;
	
	[Header("=== 점프, 슬라이드 버튼 ===")]
	[SerializeField] public JumpButton JumpButton;
	[SerializeField] public SlideButton SlideButton;

	[Header("=== 단축키 ===")]
	[SerializeField] private KeyCode _jumpKey = KeyCode.Space;
	[SerializeField] private KeyCode _slideKey = KeyCode.LeftControl;

	[Header("=== 디버그용 변수 ===")]
	[SerializeField] private float _jumpForce = 10f;
	[SerializeField] private float _gravityScale = 3f;
	[SerializeField] private float _healthReduceSpeed = 2f;
	
	// 점프하자마자 Ground와 착지 판정 생겨서 3단 점프 되는 문제 해결을 위함
	private float _ignoreGroundTimer;
	private readonly float _ignoreGroundDuration = 0.1f;
	
	private Rigidbody2D _rigidBody;
	private BoxCollider2D _collider;
	public BoxCollider2D Collider => _collider;
	private CookieBehavior _cookieBehavior;
	private Animator _animator;
	private GameManager _gameManager;
	private CookieState _state;
	private SpriteRenderer _spriteRenderer;
	
	private bool _jumpRequested;
	// 능력 사용 중일 때 등 점프 불가능하게 하고 싶을 때 사용
	public bool JumpEnabled { get; set; } = true;
	// 같은 이유로 슬라이딩 불가능하게 하고 싶을 때 사용
	public bool SlideEnabled { get; set; } = true;
	
	private readonly float _standingYPos = -2.735f;
	private readonly float _slidingYDiff = -0.425f;
	
	private readonly float _standingColliderYSize = 1.69f;
	private readonly float _slidingColliderYSize = 1.1f;
	
	private Coroutine _coGodMode;
	
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
		_spriteRenderer = GetComponent<SpriteRenderer>();
		
		_rigidBody.gravityScale = _gravityScale;
		
		_cookieBehavior.Init(this);
		_animator.runtimeAnimatorController = data.AnimatorController;
		_cookieBehavior.StartRunAnimation();
		
		// 점프 및 슬라이드 버튼의 동작을 키와 동기화
		JumpButton.OnButtonDown.AddListener(() => OnJumpKeyDown?.Invoke());
		JumpButton.OnButtonUp.AddListener(() => OnJumpKeyUp?.Invoke());
		JumpButton.WhileButtonPressed.AddListener(() => WhileJumpKeyPressed?.Invoke());
		SlideButton.OnButtonDown.AddListener(() => OnSlideKeyDown?.Invoke());
		SlideButton.OnButtonUp.AddListener(() => OnSlideKeyUp?.Invoke());
		SlideButton.WhileButtonPressed.AddListener(() => WhileSlideKeyPressed?.Invoke());
		
		// 처음엔 점프 눌렀을 때 RequestJump, 슬라이드 누르고 뗄 때 Start, End만 넣음
		OnJumpKeyDown.AddListener(RequestJump);
		OnSlideKeyDown.AddListener(RequestSlidingStart);
		OnSlideKeyUp.AddListener(RequestSlidingEnd);
	}
	
	// 체력 감소
	public void TakeDamage(float amount) {
		// 무적 상태일 땐 데미지 없음
		if (_isGodMode) { return; }
		// 추가체력이 있다면, 그것부터 감소
		if (_additionalHp > 0) {
			float reducedAmount = Mathf.Clamp(amount, 0, _additionalHp);
			AdditionalHp -= reducedAmount;
			amount -= reducedAmount;
		}
		
		CurrentHp -= amount;
		
		// 부딫혔을 때 이벤트 활성화
		OnTakeDamage?.Invoke();
		// 무적 상태 활성화
		_coGodMode = StartCoroutine(CoGodMode());
	}
	
	public IEnumerator CoGodMode() {
		_gameManager.InvisibleGround.SetActive(true);
		_isGodMode = true;
		float godModeTimer = 0f;
		
		while (godModeTimer <= _godModeDuration) {
			godModeTimer += Time.deltaTime;
			if ((int)(godModeTimer / 0.1) % 2 == 0) {
				_spriteRenderer.enabled = false;
			} else {
				_spriteRenderer.enabled = true;
			}
			
			yield return null;
		}
		
		_spriteRenderer.enabled = true;
		_gameManager.InvisibleGround.SetActive(false);
		_isGodMode = false;
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
		if (other.CompareTag(Tags.Ground) &&
		    (_state == CookieState.Jump || _state == CookieState.DoubleJump) &&
		    _ignoreGroundTimer >= _ignoreGroundDuration) {
			Debug.Log($"땅에 닿음");
			_state = CookieState.Run;
			_cookieBehavior.StartRunAnimation();
		}
		
		// 바닥으로 떨어지면 다시 위로 올려주기
		if (other.CompareTag(Tags.Drop)) {
			Debug.Log($"바닥에 떨어짐");
			TakeDamage(20f);
			transform.position = new Vector3(transform.position.x, _standingYPos, transform.position.z);
		}
	}

	private void Update() {
		// 체력 조금씩 줄이기. 추가체력 있으면 추가체력부터
		if (AdditionalHp > 0) {
			AdditionalHp -= _healthReduceSpeed * Time.deltaTime;
		} else {
			CurrentHp -= _healthReduceSpeed * Time.deltaTime;	
		}
		
		// 죽었는지 체크해서 게임 종료 알림 및 RigidBody 해제
		if (_cookieBehavior.DeathCheck() && _state != CookieState.Death) {
			_state = CookieState.Death;
			_gameManager.GameEndFlag = true;
			_rigidBody.simulated = false;
			
			// 사망 시에 무적상태였을 수 있으니, 무적상태 연출 해제
			StopCoroutine(_coGodMode);
			_spriteRenderer.enabled = true;
			
			// 죽었을 때 충돌 판정 조정해서 죽는 모션 자연스럽게
			SetSlidingPosition();
			SetSlidingCollider();
			_cookieBehavior.StartDeathAnimation();
			
			return;
		}
		
		// UI도 줄이고, 위치 설정
		_hpBar.fillAmount = UiHpPercent;
		// 추가체력 위치 설정시에는 체력바 줄어든 값에 맞춰서 배치
		_additionalHpBar.rectTransform.anchoredPosition = new Vector2(_hpBar.rectTransform.anchoredPosition.x + _hpBar.rectTransform.sizeDelta.x * UiHpPercent, _additionalHpBar.rectTransform.anchoredPosition.y); 
		_additionalHpBar.fillAmount = AdditionalHpPercent;
		
		// Update에서는 점프애 관련하여 요청했는지 확인만, 물리 처리는 FixedUpdate에서 수행
		if (Input.GetKeyDown(_jumpKey)) { OnJumpKeyDown?.Invoke(); }
		if (Input.GetKeyUp(_jumpKey)) { OnJumpKeyUp?.Invoke(); }
		if (Input.GetKey(_jumpKey)) { WhileJumpKeyPressed?.Invoke(); }
		// 슬라이드 키 누르면 슬라이드 시작		
		if (Input.GetKeyDown(_slideKey)) { OnSlideKeyDown?.Invoke(); }
		// 슬라이드 키 떼면 슬라이드 종료
		if (Input.GetKeyUp(_slideKey)) { OnSlideKeyUp?.Invoke(); }
		if (Input.GetKey(_slideKey)) { WhileSlideKeyPressed?.Invoke(); }
		
		if (Input.GetKeyDown(KeyCode.A)) { GetAdditionalHealth(10); }
	}

	private void FixedUpdate() {
		_ignoreGroundTimer += Time.deltaTime;
		
		// 점프 요청되었다면 점프
		if (_jumpRequested && JumpEnabled) {
			if (_state == CookieState.Run || _state == CookieState.Slide || _state == CookieState.Death) {
				_ignoreGroundTimer = 0f;
				_state = CookieState.Jump;
				_cookieBehavior.StartJumpAnimation();
				
				_rigidBody.linearVelocity = new Vector2(0, _jumpForce);
			}
			
			else if (_state == CookieState.Jump) {
				_state = CookieState.DoubleJump;
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
		// 점프나 더블점프중엔 슬라이드 불가능
		if (_state == CookieState.Jump || _state == CookieState.DoubleJump || _state == CookieState.Death) { return; }
		if (!SlideEnabled) { return; }
		_state = CookieState.Slide;
		
		SetSlidingPosition();
		SetSlidingCollider();
		_cookieBehavior.StartSlidingAnimation();
	}
	
	public void RequestSlidingEnd() {
		// 슬라이딩 중일 때만 슬라이딩 종료 가능
		if (_state != CookieState.Slide) { return; }
		
		_state = CookieState.Run;
			
		SetStandingPosition();
		SetStandingCollider();
		_cookieBehavior.StartRunAnimation();
	}
	
	// 슬라이딩 시에 위치 자연스럽게 변경
	public void SetSlidingPosition() {
		transform.position = new Vector3(transform.position.x, transform.position.y + _slidingYDiff, transform.position.z);
	}
	// 위치 원래대로
	public void SetStandingPosition() {
		transform.position = new Vector3(transform.position.x, transform.position.y - _slidingYDiff, transform.position.z);
	}
	// 슬라이딩 시에 충돌 박스 크기 자연스럽게 변경
	public void SetSlidingCollider() {
		_collider.size = new Vector2(_collider.size.x, _slidingColliderYSize);
	}
	// 충돌 박스 크기 원래대로
	public void SetStandingCollider() {
		_collider.size = new Vector2(_collider.size.x, _standingColliderYSize);
	}
}