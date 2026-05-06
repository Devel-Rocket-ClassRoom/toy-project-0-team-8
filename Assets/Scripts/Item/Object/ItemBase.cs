using System.Collections;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour {
	[HideInInspector] public Transform MagnetTarget = null;
	[HideInInspector] public float _magnetSpeed = 5f;
	protected abstract float ItemDuration { get; }
	
	protected virtual void Update() {
		if (MagnetTarget != null) {
			
			// transform을 MagnetTarget의 위치로 점점 끌어당기기
			transform.position = 
				Vector3.MoveTowards(
					transform.position,
					MagnetTarget.transform.position,
					_magnetSpeed * Time.deltaTime);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// Player와 닿으면 아이템 먹어지고, 효과 발동
		if (other.transform.CompareTag(Tags.Player)) {
			// 아이템 먹으면 바로 사라지는 처리 후, 실제 효과 종료되면 아예 Destroy
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<BoxCollider2D>().enabled = false;
			StartCoroutine(CoApplyItemEffect(other.gameObject.GetComponent<CookieController>()));
		}
	}
	
	protected abstract void ApplyItemEffect(CookieController other);
	// 아이템 remove될 때 필요한 아이템만 재정의
	protected virtual void RemoveItemEffect(CookieController other) { }
	
	// 아이템 효과 적용 후, 지속시간만큼 기다린 후 효과 해제. 그리고 오브젝트 삭제
	private IEnumerator CoApplyItemEffect(CookieController other) {
		ApplyItemEffect(other);
		
		yield return new WaitForSeconds(ItemDuration);
		
		RemoveItemEffect(other);
		Destroy(this);
	}

	protected virtual void Awake() {
		transform.tag = Tags.Item;
	}
}