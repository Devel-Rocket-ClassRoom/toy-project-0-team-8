using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour {
	[Header("=== 계속하기 버튼 ===")]
	[SerializeField] private Button _resumeButton;
	[Header("=== 종료 버튼 ===")]
	[SerializeField] private Button _quitButton;
	
	// 종료 버튼에 붙을 이벤트
	[HideInInspector] public UnityEvent OnQuitButtonPressed;
	[HideInInspector] public UnityEvent OnResumeButtonPressed;
	
	private void Awake() {
		// 이벤트 붙이기
		_quitButton.onClick.AddListener(() => OnQuitButtonPressed?.Invoke());
		_resumeButton.onClick.AddListener(() => OnResumeButtonPressed?.Invoke());
	}
}