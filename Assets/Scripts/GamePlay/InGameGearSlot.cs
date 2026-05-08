using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InGameGearSlot : MonoBehaviour {
	// 진행도 바 이미지
	[SerializeField] private Image _progressBar;
	// 반짝할 때 사용할 이미지
	[SerializeField] Image _shineEffect;
	
	private GearBase _gear;
	private Coroutine _coShowShineEffect;
	
	private void SetShineEffectAlpha(float alpha) => _shineEffect.color = new Color(1, 1, 1, alpha);
	
	public void Init(GearBase gear) {
		_gear = gear;
		// gear가 활성화될 때 생기는 Event에 내 함수 등록해두기
		if (gear != null) _gear.OnGearActivated.AddListener(ShowShineEffect);
		// 시작 시에 깜빡임 이펙트는 꺼두기
		SetShineEffectAlpha(0);
	}

	private void Update() {
		if (_gear == null) { _progressBar.fillAmount = 1f;}
		else {
			_progressBar.fillAmount = 1f - _gear.GetProgressBarAmount();
		}
	}
	
	private void ShowShineEffect() {
		if (_coShowShineEffect != null) { StopCoroutine(_coShowShineEffect); }
		_coShowShineEffect = StartCoroutine(CoShowShineEffect());
	}
	
	// 잠깐 반짝하고 말도록
	private IEnumerator CoShowShineEffect() {
		yield return StartCoroutine(CoShineEffectOn());
		yield return StartCoroutine(CoShineEffectOff()); 
	}
	
	// 반짝거릴 때 사용
	private IEnumerator CoShineEffectOn() {
		float timer = 0f;
		while (timer < 0.2f) {
			timer += Time.deltaTime;
			SetShineEffectAlpha(Mathf.Lerp(0, 1,  timer / 0.1f));
			yield return null;
		}
	}
	
	private IEnumerator CoShineEffectOff() {
		float timer = 0f;
		while (timer < 0.2f) {
			timer += Time.deltaTime;
			SetShineEffectAlpha(Mathf.Lerp(1, 0,  timer / 0.1f));
			yield return null;
		}
	}
}