using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(BoxCollider2D))]
public class CookieController : MonoBehaviour
{
    // 능력 사용 중일 때 등 점프 불가능하게 하고 싶을 때 사용
    public bool JumpEnabled { get; set; } = true;

    // 같은 이유로 슬라이딩 불가능하게 하고 싶을 때 사용
    public bool SlideEnabled { get; set; } = true;

    // 체력 줄지 않게 하고 싶을 때 사용
    public bool CollisionEnabled { get; set; } = true;

    // 체력 회복되지 않게 하고 싶을 때 사용
    public bool HealEnabled { get; set; } = true;

    public bool _isSkill = false; // 스킬 : 무적, 충돌이 같이 들어가야하는 쿠키용

    [Header("=== 비행 쿠키 전용 천장 ===")]
    public GameObject roof;

    [Header("=== 능력치 표시 바 ===")]
    [SerializeField]
    private Image _abilityProgressBar;

    [Header("=== 떨어졌을때 메세지 출력할 Panel ===")]
    [SerializeField]
    private GameObject _fallAlertPanel;

    [Header("=== 쿠키 애니메이션을 재생할 GameObject ===")]
    [SerializeField]
    private CookieRenderer _cookieRenderer;

    // 사망 후 몇 초 있다 엔딩화면으로 넘어갈지
    private readonly float _latencyAfterDeath = 1.5f;

    private bool _isDashing;
    private bool _isGiantMode;

    // 붙어있는 Component 목록
    private BoxCollider2D _collider;
    private CookieBehavior _cookieBehavior;
    private GameManager _gameManager;
    private Animator _animator;

    // 서브 컴포넌트
    private CookieHealthController _healthController;
    private CookieInputController _inputController;
    private CookieSizeController _sizeController;
    private CookieGearController _gearController;
    private CookieMovementController _movementController;

    public Animator Animator => _animator;
    public BoxCollider2D Collider => _collider;

    public bool IsGiantMode
    {
        get => _isGiantMode;
        set => _isGiantMode = value;
    }

    public bool IsDashing
    {
        get => _isDashing;
        set
        {
            // 대쉬 먹게 되었을 때, 뛰는 상태면 Dash로 애니메이션 바꿔주기
            if (value && _movementController.CurrentState == CookieState.Run)
            {
                _cookieBehavior.StartDashAnimation();
            }
            // 대쉬 끝날 때, 뛰는 상태면 Run으로 애니메이션 변경
            else if (!value && _movementController.CurrentState == CookieState.Run)
            {
                _cookieBehavior.StartRunAnimation();
            }
            _isDashing = value;
        }
    }

    // === Health forwarding ===
    public float CurrentHp => _healthController.CurrentHp;
    public float AdditionalHp => _healthController.AdditionalHp;
    public bool IsGodMode => _healthController.IsGodMode;
    public UnityEvent OnTakeDamage => _healthController.OnTakeDamage;

    public void TakeDamage(float amount) => _healthController.TakeDamage(amount);

    public void RecoverHp(float amount) => _healthController.RecoverHp(amount);

    public void GetAdditionalHealth(float amount) => _healthController.GetAdditionalHealth(amount);

    public void EnableGodMode(float duration) => _healthController.EnableGodMode(duration);

    // === Input forwarding ===
    public UnityEvent OnJumpKeyDown => _inputController.OnJumpKeyDown;
    public UnityEvent OnJumpKeyUp => _inputController.OnJumpKeyUp;
    public UnityEvent WhileJumpKeyPressed => _inputController.WhileJumpKeyPressed;
    public UnityEvent OnSlideKeyDown => _inputController.OnSlideKeyDown;
    public UnityEvent OnSlideKeyUp => _inputController.OnSlideKeyUp;
    public UnityEvent WhileSlideKeyPressed => _inputController.WhileSlideKeyPressed;

    // === Size forwarding ===
    public CookieCollisionChecker CollisionCollider => _sizeController.CollisionCollider;

    public void SetSlidingCollider() => _sizeController.SetSlidingCollider();

    public void SetStandingCollider() => _sizeController.SetStandingCollider();

    public void SetSlidingPosition() => _sizeController.SetSlidingPosition();

    public void SetStandingPosition() => _sizeController.SetStandingPosition();

    // === Movement forwarding ===
    public void RequestJump() => _movementController.RequestJump();

    public void RequestSlidingStart() => _movementController.RequestSlidingStart();

    public void RequestSlidingEnd() => _movementController.RequestSlidingEnd();

    public float GravityScale => _movementController.GravityScale;

    public void Init(CookieData data, GameManager gameManager)
    {
        _gameManager = gameManager;

        _collider = GetComponent<BoxCollider2D>();
        _animator = _cookieRenderer.Animator;

        _healthController = GetComponent<CookieHealthController>();
        _inputController = GetComponent<CookieInputController>();
        _sizeController = GetComponent<CookieSizeController>();
        _gearController = GetComponent<CookieGearController>();
        _movementController = GetComponent<CookieMovementController>();

        _healthController.Init(data.Hp, _cookieRenderer.SpriteRenderer);
        _inputController.Init(_gameManager);

        // Factory 이용해서 data에 맞는 Behavior 붙이기
        CookieBehaviorFactory.AddBehavior(gameObject, data);
        _cookieBehavior = GetComponent<CookieBehavior>();

        _movementController.Init(_cookieBehavior, _sizeController, this);

        _gearController.SetGear(gameManager);

        _cookieBehavior.Init(this);
        _animator.runtimeAnimatorController = data.AnimatorController;

        // 시작 시에 달리기 상태로 시작
        if (IsDashing)
        {
            _cookieBehavior.StartDashAnimation();
        }
        else
        {
            _cookieBehavior.StartRunAnimation();
        }

        // 점프 및 슬라이드 이벤트에 실제 동작 바인딩
        OnJumpKeyDown.AddListener(RequestJump);
        OnSlideKeyDown.AddListener(RequestSlidingStart);
        OnSlideKeyUp.AddListener(RequestSlidingEnd);
        // 슬라이드 누르고 있으면 땅에 내리자마자 슬라이딩
        WhileSlideKeyPressed.AddListener(() =>
        {
            if (_movementController.CurrentState == CookieState.Run)
            {
                RequestSlidingStart();
            }
        });

        // 능력 진행도 바를 사용한다면 활성화, 안한다면 비활성화
        _abilityProgressBar.gameObject.SetActive(_cookieBehavior.UseAbilityProgressBar);

        // 추락 알림 패널 비활성화
        _fallAlertPanel.SetActive(false);

        // 렌더러 위치 수정
        _cookieRenderer.SetSpriteLocation(data);
    }

    private void Update()
    {
        // 사망 체크 후 각 서브 컴포넌트 코디네이션
        if (_cookieBehavior.DeathCheck() && _movementController.CurrentState != CookieState.Death)
        {
            _movementController.MarkDead();
            _gameManager.GameEndFlag = true;
            _healthController.StopGodMode();
            _sizeController.SetSlidingPosition();
            _sizeController.SetSlidingCollider();
            _cookieBehavior.StartDeathAnimation();
            StartCoroutine(CoEndGame());
            return;
        }

        // ProgressBar 사용한다면, 현재 진행도 값으로 fillAmount 수정
        if (_cookieBehavior.UseAbilityProgressBar)
        {
            _abilityProgressBar.fillAmount = _cookieBehavior.GetProgressbarAmount();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 바닥으로 떨어지면 게임 종료 처리
        if (other.CompareTag(Tags.Drop))
        {
            _gameManager.GameEndFlag = true;
            _fallAlertPanel.SetActive(true);
            StartCoroutine(CoEndGame());
        }
    }

    private IEnumerator CoEndGame()
    {
        yield return new WaitForSeconds(_latencyAfterDeath);
        _gameManager.EndGame();
    }
}
