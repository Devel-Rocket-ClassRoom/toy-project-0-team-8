using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CookieController))]
public class CookieInputController : MonoBehaviour
{
    [Header("=== 점프, 슬라이드 버튼 ===")]
    [SerializeField]
    public JumpButton JumpButton;

    [SerializeField]
    public SlideButton SlideButton;

    [Header("=== 단축키 ===")]
    [SerializeField]
    private KeyCode _jumpKey = KeyCode.Space;

    [SerializeField]
    private KeyCode _slideKey = KeyCode.LeftControl;

    [HideInInspector]
    public UnityEvent OnJumpKeyDown;

    [HideInInspector]
    public UnityEvent OnJumpKeyUp;

    [HideInInspector]
    public UnityEvent WhileJumpKeyPressed;

    [HideInInspector]
    public UnityEvent OnSlideKeyDown;

    [HideInInspector]
    public UnityEvent OnSlideKeyUp;

    [HideInInspector]
    public UnityEvent WhileSlideKeyPressed;

    private GameManager _gameManager;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;

        JumpButton.OnButtonDown.AddListener(() => OnJumpKeyDown?.Invoke());
        JumpButton.OnButtonUp.AddListener(() => OnJumpKeyUp?.Invoke());
        JumpButton.WhileButtonPressed.AddListener(() => WhileJumpKeyPressed?.Invoke());
        SlideButton.OnButtonDown.AddListener(() => OnSlideKeyDown?.Invoke());
        SlideButton.OnButtonUp.AddListener(() => OnSlideKeyUp?.Invoke());
        SlideButton.WhileButtonPressed.AddListener(() => WhileSlideKeyPressed?.Invoke());
    }

    private void Update()
    {
        if (Input.GetKeyDown(_jumpKey))
            OnJumpKeyDown?.Invoke();
        if (Input.GetKeyUp(_jumpKey))
            OnJumpKeyUp?.Invoke();
        if (Input.GetKey(_jumpKey))
            WhileJumpKeyPressed?.Invoke();

        if (Input.GetKeyDown(_slideKey))
            OnSlideKeyDown?.Invoke();
        if (Input.GetKeyUp(_slideKey))
            OnSlideKeyUp?.Invoke();
        if (Input.GetKey(_slideKey))
            WhileSlideKeyPressed?.Invoke();

        // 임시. A키 누르면 임시 체력 생김
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetComponent<CookieController>().GetAdditionalHealth(10);
            _gameManager.ActivateDash(5f);
        }
    }
}
