using UnityEngine;

public abstract class GearBase : MonoBehaviour {
	// 능력 게이지가 얼마나 찼는지 인게임에서 보여줄 때 사용할 함수
	public abstract float GetProgressBarAmount();
}