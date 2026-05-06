using System;
using System.Collections.Generic;
using UnityEngine;

public class MagnetArea : MonoBehaviour {
	private readonly List<ItemBase> magnetItemList = new();

	// 활성화될 때 아이템 리스트 초기화
	private void OnEnable() {
		magnetItemList.Clear();
	}
	
	private void OnDisable() {
		foreach (var item in magnetItemList) {
			item.MagnetTarget = null;
		}
		magnetItemList.Clear();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// 충돌한 아이템들을 캐릭터 위치로 끌어당기기
		if (other.transform.CompareTag(Tags.Item)) {
			Debug.Log($"아이템 진입");
			ItemBase item = other.gameObject.GetComponent<ItemBase>();
			item.MagnetTarget = transform;
			magnetItemList.Add(item);
		}
	}
}