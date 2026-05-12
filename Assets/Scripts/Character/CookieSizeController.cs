using UnityEngine;

[RequireComponent(typeof(CookieController), typeof(BoxCollider2D))]
public class CookieSizeController : MonoBehaviour
{
    [Header("=== 실제 충돌 처리할 캡슐 ===")]
    [SerializeField]
    private CookieCollisionChecker _collisionCollider;

    private readonly float _slidingYDiff = -0.425f;

    private readonly float _standingColliderYOffset = -0.21f;
    private readonly float _standingColliderYSize = 1.32f;
    private readonly float _slidingColliderYOffset = -0.21f;
    private readonly float _slidingColliderYSize = 0.7f;

    private BoxCollider2D _collider;

    public CookieCollisionChecker CollisionCollider => _collisionCollider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    public void SetSlidingPosition()
    {
        _collisionCollider.SetSlidingPos();
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y + _slidingYDiff,
            transform.position.z
        );
    }

    public void SetStandingPosition()
    {
        _collisionCollider.SetStandingPos();
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y - _slidingYDiff,
            transform.position.z
        );
    }

    public void SetSlidingCollider()
    {
        _collider.offset = new Vector2(_collider.offset.x, _slidingColliderYOffset);
        _collider.size = new Vector2(_collider.size.x, _slidingColliderYSize);
    }

    public void SetStandingCollider()
    {
        _collider.offset = new Vector2(_collider.offset.x, _standingColliderYOffset);
        _collider.size = new Vector2(_collider.size.x, _standingColliderYSize);
    }
}
