using UnityEngine;

public class CookieCollisionChecker : MonoBehaviour {
	// 장애물 파괴 시에 사용할 hash값 등록
	private static readonly int _break = Animator.StringToHash("Break");
	[SerializeField] private CookieController _cookieController;
	[SerializeField] private GameManager _gameManager;
	public CookieController CookieController => _cookieController;
	public bool _changeStage = false; // 스테이지 전환마다 실행할 거 필요한 쿠키들이 사용
	
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
			// 충돌 비활성화 상태라면, 일체의 상호작용 없음
			if (!_cookieController.CollisionEnabled) { return; }
			// 대쉬 혹은 거인화 상태라면, 부수고 지나감
			if (_cookieController.IsDashing || _cookieController.IsGiantMode || _cookieController._isSkill) {
				other.GetComponent<Animator>().SetTrigger(_break);
				return;
			}
			// 무적 상태라면 데미지 받지 않음
			if (_cookieController.IsGodMode) { return; }
			
			// 다 아니라면 데미지 받기
			_cookieController.TakeDamage(20);
		}

		// 맵 끝과 충돌하면, 다음 스테이지 로딩하게
		if (other.CompareTag(Tags.StageEnd)) {
			_gameManager.LoadNextStage();
			_changeStage = true;

        }
	}
}