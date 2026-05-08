using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// 모든 쿠키가 베이스로 사용할 클래스입니다.
// 쿠키에는 필수적으로 Rigidbody2D, Animator, BoxCollider2D가 붙어있어야 합니다.
[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(BoxCollider2D))]
public class CookieController : MonoBehaviour {
	// UI가 표현 가능한 최대체력을 정의
	private readonly float UiMaxHp = 100f;
	
	// 체력 세팅 관련 변수
	private float MaxHp { get; set; }
	private readonly float MaxAdditionalHp = 100f;
	private float _currentHp;
	private float _additionalHp;
	
	// 무적, 능력 관련 변수
	private readonly float _godModeDurationAfterHit = 2f;
	private bool _isGodMode = false; 
	private bool _isDashing = false;
	private bool _isGiantMode = false;
	public bool _isSkill = false; // 스킬 : 무적, 충돌이 같이 들어가야하는 쿠키용

    // 데미지 입을 시 특정 행동을 하는 캐릭터들이 사용하기 위한 이벤트
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
	[SerializeField] private Image _healthBarShine;

	[Header("=== 능력치 표시 바 ===")]
	[SerializeField] private Image _abilityProgressBar;
	
	[Header("=== 점프, 슬라이드 버튼 ===")]
	[SerializeField] public JumpButton JumpButton;
	[SerializeField] public SlideButton SlideButton;

	[Header("=== 단축키 ===")]
	[SerializeField] private KeyCode _jumpKey = KeyCode.Space;
	[SerializeField] private KeyCode _slideKey = KeyCode.LeftControl;
	
	[Header("=== 실제 충돌 처리할 캡슐 ===")]
	[SerializeField] private CookieCollisionChecker _collisionCollider;

    [Header("=== 떨어졌을때 메세지 출력할 Panel ===")] 
	[SerializeField] private GameObject _fallAlertPanel;

	[Header("=== 비행 쿠키 전용 천장 ===")]
	public GameObject roof;

	[Header("=== 보물 프리팹 생성 ===")]
	public Transform gearPrefabParent;

	[Header("=== 쿠키 애니메이션을 재생할 GameObject ===")] 
	[SerializeField] private CookieRenderer _cookieRenderer;
	
	// 점프 관련 변수
	private readonly float _jumpForce = 25f;
	private readonly float _gravityScale = 8f;
	private readonly float _healthReduceSpeed = 2f;
	private readonly float _linearVelocityMax = 22f;
	public float GravityScale => _gravityScale;
	
	// 사망 후 몇 초 있다 엔딩화면으로 넘어갈지
	private readonly float _latencyAfterDeath = 1.5f;
	
	// 점프하자마자 Ground와 착지 판정 생겨서 3단 점프 되는 문제 해결을 위함
	private float _ignoreGroundTimer;
	private readonly float _ignoreGroundDuration = 0.1f;
	
	// 붙어있는 Component 목록
	private Rigidbody2D _rigidBody;
	private BoxCollider2D _collider;
	private CookieBehavior _cookieBehavior;
	private GameManager _gameManager;
	private CookieState _state;
	private Animator _animator;
	private SpriteRenderer _spriteRenderer;
	public Animator Animator => _animator;
	public BoxCollider2D Collider => _collider;
    public CookieCollisionChecker CollisionCollider => _collisionCollider;
    public bool IsGodMode => _isGodMode;

	public bool IsGiantMode {
		get => _isGiantMode;
		set => _isGiantMode = value;
	}

	public bool IsDashing {
		get => _isDashing;
		set {
			// 대쉬 먹게 되었을 때, 뛰는 상태면 Dash로 애니메이션 바꿔주기
			if (value && _state == CookieState.Run) { _cookieBehavior.StartDashAnimation(); }
			// 대쉬 끝날 때, 뛰는 상태면 Run으로 애니메이션 변경
			else if (!value && _state == CookieState.Run) { _cookieBehavior.StartRunAnimation(); }
			_isDashing = value;
		}
	}
	
	private bool _jumpRequested;
	// 능력 사용 중일 때 등 점프 불가능하게 하고 싶을 때 사용
	public bool JumpEnabled { get; set; } = true;
	// 같은 이유로 슬라이딩 불가능하게 하고 싶을 때 사용
	public bool SlideEnabled { get; set; } = true;
	// 체력 줄지 않게 하고 싶을 때 사용
	public bool CollisionEnabled { get; set; } = true;
	// 체력 회복되지 않게 하고 싶을 때 사용
	public bool HealEnabled { get; set; } = true;
	
	//private readonly float _standingYPos = -2.735f;
	private readonly float _slidingYDiff = -0.425f;
	
	private readonly float _standingColliderYOffset = -0.21f;
	private readonly float _standingColliderYSize = 1.32f;
	private readonly float _slidingColliderYOffset = -0.21f;
	private readonly float _slidingColliderYSize = 0.7f;
	
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
	
	public void Init(CookieData data, GameManager gameManager) {
		_gameManager = gameManager;
		
		MaxHp = data.Hp;
		CurrentHp = MaxHp;
		
		// Factory 이용해서 data에 맞는 Behavior 붙이기
		CookieBehaviorFactory.AddBehavior(gameObject, data);

		// 보물 장착
		SetGear();

        _rigidBody = GetComponent<Rigidbody2D>();
		_cookieBehavior = GetComponent<CookieBehavior>();
		_collider = GetComponent<BoxCollider2D>();
		_animator = _cookieRenderer.Animator;
		_spriteRenderer = _cookieRenderer.SpriteRenderer;
		
		_rigidBody.gravityScale = _gravityScale;
		
		_cookieBehavior.Init(this);
		_animator.runtimeAnimatorController = data.AnimatorController;
		
		// 시작 시에 달리기 상태로 시작
		if (IsDashing) { _cookieBehavior.StartDashAnimation(); }
		else { _cookieBehavior.StartRunAnimation(); }
		
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
		// 슬라이드 누르고 있으면 땅에 내리자마자 슬라이딩
		WhileSlideKeyPressed.AddListener(() => {
			if (_state == CookieState.Run) { RequestSlidingStart(); }
		});
		
		// 능력 진행도 바를 사용한다면 활성화, 안한다면 비활성화
		_abilityProgressBar.gameObject.SetActive(_cookieBehavior.UseAbilityProgressBar);
		
		// 추락 알림 패널 비활성화
		_fallAlertPanel.SetActive(false);
		
		// 렌더러 위치 수정
		_cookieRenderer.SetSpriteLocation(data);
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
		EnableGodMode(_godModeDurationAfterHit);
	}
	
	public void EnableGodMode(float duration) {
		if (_coGodMode != null) StopCoroutine(_coGodMode);
		_coGodMode = StartCoroutine(CoGodMode(duration));
	} 
	
	// 실제 무적모드 효과 연출 및 무적 적용
	private IEnumerator CoGodMode(float duration) {
		_isGodMode = true;
		float godModeTimer = 0f;
		
		while (godModeTimer <= duration) {
			godModeTimer += Time.deltaTime;
			if ((int)(godModeTimer / 0.1) % 2 == 0) {
				_spriteRenderer.enabled = false;
			} else {
				_spriteRenderer.enabled = true;
			}
			
			yield return null;
		}
		
		_spriteRenderer.enabled = true;
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
			_state = CookieState.Run;
			
			// 대쉬중이라면 대쉬모션, 아니라면 일반모션
			if (IsDashing) { _cookieBehavior.StartDashAnimation(); }
			else { _cookieBehavior.StartRunAnimation(); }
		}
		
		// 바닥으로 떨어지면 게임 종료 처리
		if (other.CompareTag(Tags.Drop)) {
			_gameManager.GameEndFlag = true;
			_fallAlertPanel.SetActive(true);
			StartCoroutine(CoEndGame());
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
			
			StartCoroutine(CoEndGame());
			
			return;
		}
		
		// UI도 줄이고, 위치 설정
		_hpBar.fillAmount = UiHpPercent;
		// 추가체력 위치 설정시에는 체력바 줄어든 값에 맞춰서 배치
		_additionalHpBar.rectTransform.anchoredPosition = new Vector2(
																	_hpBar.rectTransform.anchoredPosition.x +
																	_hpBar.rectTransform.sizeDelta.x * UiHpPercent,
																	_additionalHpBar.rectTransform.anchoredPosition.y);
		_additionalHpBar.fillAmount = AdditionalHpPercent;
		// 추가체력 위치까지 고려해서 Shine 배치
		_healthBarShine.rectTransform.anchoredPosition = new Vector2(_additionalHpBar.rectTransform.anchoredPosition.x +
		                                                             _additionalHpBar.rectTransform.sizeDelta.x * AdditionalHpPercent - 20,
																	 _healthBarShine.rectTransform.anchoredPosition.y);
		// ProgressBar 사용한다면, 현재 진행도 값으로 ProgressbarAmount 수정
		if (_cookieBehavior.UseAbilityProgressBar) {
			_abilityProgressBar.fillAmount = _cookieBehavior.GetProgressbarAmount();
		}
		
		
		// Update에서는 점프애 관련하여 요청했는지 확인만, 물리 처리는 FixedUpdate에서 수행
		if (Input.GetKeyDown(_jumpKey)) { OnJumpKeyDown?.Invoke(); }
		if (Input.GetKeyUp(_jumpKey)) { OnJumpKeyUp?.Invoke(); }
		if (Input.GetKey(_jumpKey)) { WhileJumpKeyPressed?.Invoke(); }
		// 슬라이드 키 누르면 슬라이드 시작		
		if (Input.GetKeyDown(_slideKey)) { OnSlideKeyDown?.Invoke(); }
		// 슬라이드 키 떼면 슬라이드 종료
		if (Input.GetKeyUp(_slideKey)) { OnSlideKeyUp?.Invoke(); }
		if (Input.GetKey(_slideKey)) { WhileSlideKeyPressed?.Invoke(); }
		
		// 임시. A키 누르면 임시 체력 생김
		if (Input.GetKeyDown(KeyCode.A)) {
			GetAdditionalHealth(10);
			_gameManager.ActivateDash(5f);
		}
	}

	private void FixedUpdate() {
		_ignoreGroundTimer += Time.deltaTime;
		
		// 점프 요청되었다면 점프
		if (_jumpRequested && JumpEnabled && _state != CookieState.Death) {
			if (_state == CookieState.Run || _state == CookieState.Slide) {
				_ignoreGroundTimer = 0f;
				_state = CookieState.Jump;
				_cookieBehavior.StartJumpAnimation();
				
				// 점프하면 포지션 값 초기화
				SetStandingPosition();
				SetStandingCollider();
				
				_rigidBody.linearVelocity = new Vector2(0, _jumpForce);
			}
			
			else if (_state == CookieState.Jump) {
				_state = CookieState.DoubleJump;
				_cookieBehavior.StartDoubleJumpAnimation();	
				
				_rigidBody.linearVelocity = new Vector2(0, _jumpForce);
			}
		}
		_jumpRequested = false;
		
		// 만약 스피드가 제한 속도 이상이면, 그 아래로 잘라줘야 함(중력 영향 줄이기)
		var temp = _rigidBody.linearVelocity;
		temp.y = Mathf.Clamp(temp.y, -_linearVelocityMax, _linearVelocityMax);
		_rigidBody.linearVelocity = temp;
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
		_collisionCollider.SetSlidingPos();
		transform.position = new Vector3(transform.position.x, transform.position.y + _slidingYDiff, transform.position.z);
	}
	
	// 위치 원래대로
	public void SetStandingPosition() {
		_collisionCollider.SetStandingPos();
		transform.position = new Vector3(transform.position.x, transform.position.y - _slidingYDiff, transform.position.z);
	}
	// 슬라이딩 시에 충돌 박스 크기 자연스럽게 변경
	public void SetSlidingCollider() {
		_collider.offset = new Vector2(_collider.offset.x, _slidingColliderYOffset);
		_collider.size = new Vector2(_collider.size.x, _slidingColliderYSize);
	}
	// 충돌 박스 크기 원래대로
	public void SetStandingCollider() {
		_collider.offset = new Vector2(_collider.offset.x, _standingColliderYOffset);
		_collider.size = new Vector2(_collider.size.x, _standingColliderYSize);
	}
	
	private IEnumerator CoEndGame() {
		yield return new WaitForSeconds(_latencyAfterDeath);
		_gameManager.EndGame();
	}



	private void SetGear()
	{
        string[] gearIds = SaveLoadManager.Data.currentGear;
        List<GearBase> gears = new List<GearBase>();
        List<GearData> gearDatas = new List<GearData>();

        foreach (string id in gearIds)
        {
            if (string.IsNullOrEmpty(id)) continue;

            // 테이블에서 ID에 해당하는 데이터(SO) 가져오기
			// 추하게 들고오긴 했음..
            var gearData = DataTableManager.GearTable.Get(id);
			var gearPrefab = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<GachaManager>().itemList.Find(g => g.itemId == id).GearPrefab;
			gearDatas.Add(gearData);
            
			if (gearData != null && gearPrefab != null)
            {
                GameObject gearObj = Instantiate(gearPrefab, gearPrefabParent);
                gears.Add(gearObj.GetComponent<GearBase>());
                Debug.Log($"{id} 보물이 {gearPrefabParent.name} 아래에 생성되었습니다.");
            }
        }
        
        // UI에도 유물 세팅하기
        _gameManager.SetGears(gears);
        _gameManager.SetGearImages(gearDatas);
    }
}