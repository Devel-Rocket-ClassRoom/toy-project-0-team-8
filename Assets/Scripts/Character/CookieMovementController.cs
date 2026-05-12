using UnityEngine;

[RequireComponent(typeof(CookieController))]
public class CookieMovementController : MonoBehaviour
{
    private readonly float _jumpForce = 25f;
    private readonly float _gravityScale = 8f;
    private readonly float _linearVelocityMax = 22f;

    private float _ignoreGroundTimer;
    private readonly float _ignoreGroundDuration = 0.1f;

    private bool _jumpRequested;
    private CookieState _state;

    private Rigidbody2D _rigidBody;
    private CookieBehavior _cookieBehavior;
    private CookieSizeController _sizeController;
    private CookieController _cookieController;

    public CookieState CurrentState => _state;
    public float GravityScale => _gravityScale;

    public void Init(
        CookieBehavior cookieBehavior,
        CookieSizeController sizeController,
        CookieController cookieController
    )
    {
        _cookieBehavior = cookieBehavior;
        _sizeController = sizeController;
        _cookieController = cookieController;

        _rigidBody = GetComponent<Rigidbody2D>();
        _rigidBody.gravityScale = _gravityScale;
        _state = CookieState.Run;
    }

    public void MarkDead()
    {
        _state = CookieState.Death;
        _rigidBody.simulated = false;
    }

    public void RequestJump()
    {
        _jumpRequested = true;
    }

    public void RequestSlidingStart()
    {
        if (
            _state == CookieState.Jump
            || _state == CookieState.DoubleJump
            || _state == CookieState.Death
        )
            return;
        if (!_cookieController.SlideEnabled)
            return;

        _state = CookieState.Slide;
        _sizeController.SetSlidingPosition();
        _sizeController.SetSlidingCollider();
        _cookieBehavior.StartSlidingAnimation();
    }

    public void RequestSlidingEnd()
    {
        if (_state != CookieState.Slide)
            return;

        _state = CookieState.Run;
        _sizeController.SetStandingPosition();
        _sizeController.SetStandingCollider();

        if (_cookieController.IsDashing)
            _cookieBehavior.StartDashAnimation();
        else
            _cookieBehavior.StartRunAnimation();
    }

    private void FixedUpdate()
    {
        _ignoreGroundTimer += Time.deltaTime;

        if (_jumpRequested && _cookieController.JumpEnabled && _state != CookieState.Death)
        {
            if (_state == CookieState.Run || _state == CookieState.Slide)
            {
                _ignoreGroundTimer = 0f;
                _state = CookieState.Jump;
                _cookieBehavior.StartJumpAnimation();
                _sizeController.SetStandingPosition();
                _sizeController.SetStandingCollider();
                _rigidBody.linearVelocity = new Vector2(0, _jumpForce);
            }
            else if (_state == CookieState.Jump)
            {
                _state = CookieState.DoubleJump;
                _cookieBehavior.StartDoubleJumpAnimation();
                _rigidBody.linearVelocity = new Vector2(0, _jumpForce);
            }
        }
        _jumpRequested = false;

        var temp = _rigidBody.linearVelocity;
        temp.y = Mathf.Clamp(temp.y, -_linearVelocityMax, _linearVelocityMax);
        _rigidBody.linearVelocity = temp;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (
            other.CompareTag(Tags.Ground)
            && (_state == CookieState.Jump || _state == CookieState.DoubleJump)
            && _ignoreGroundTimer >= _ignoreGroundDuration
        )
        {
            _state = CookieState.Run;

            if (_cookieController.IsDashing)
                _cookieBehavior.StartDashAnimation();
            else
                _cookieBehavior.StartRunAnimation();
        }
    }
}
