using UnityEngine;

public class CookieCollisionChecker : MonoBehaviour {
	[SerializeField] private CookieController _cookieController;
	public CookieController CookieController => _cookieController;
	private CapsuleCollider2D _collider2D;
	
	private readonly float _standingColliderXOffset = -0.08f;
	private readonly float _standingColliderXSize = 1.08f;
	private readonly float _standingColliderYOffset = -0.82f;
	private readonly float _standingColliderYSize = 2.36f;
	
	private readonly float _slidingColliderXOffset = 0f;
	private readonly float _slidingColliderXSize = 1.39f;
	private readonly float _slidingColliderYOffset = -1.15f;
	private readonly float _slidingColliderYSize = 0.73f;

	private void Awake() {
		_collider2D = GetComponent<CapsuleCollider2D>();
	}
	
	public void SetSlidingPos() {
		SetSlidingOffset();
		SetSlidingSize();
	}
	
	public void SetStandingPos() {
		SetStandingOffset();
		SetStandingSize();
	}
	
	private void SetSlidingOffset() {
		_collider2D.offset = new Vector2(_slidingColliderXOffset, _slidingColliderYOffset);
	}
	
	private void SetStandingOffset() {
		_collider2D.offset = new Vector2(_standingColliderXOffset, _slidingColliderYOffset);
	}
	
	public void SetSlidingSize() {
		_collider2D.size = new Vector2(_slidingColliderXSize, _slidingColliderYSize);
	}
	
	public void SetStandingSize() {
		_collider2D.size = new Vector2(_standingColliderXSize, _slidingColliderYSize);
	}
	
	private void OnTriggerEnter2D(Collider2D other) {
		// 장애물과 충돌하면 무적 판정 실행하고, 체력 깎기
		if (other.CompareTag(Tags.Obstacle)) {
			// 대쉬 혹은 거인화 상태라면, 부수고 지나감
			if (_cookieController.IsDashing || _cookieController.IsGiantMode) {
				Destroy(other.gameObject);
				return;
			}
			// 무적 상태라면, 데미지 받지 않음
			if (_cookieController.IsGodMode) { return; }
			
			// 다 아니라면 데미지 받기
			_cookieController.TakeDamage(20);
		}
	}
}