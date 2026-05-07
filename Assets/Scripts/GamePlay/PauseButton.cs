using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour {
	[Header("=== GameManager ===")]
	[SerializeField] private GameManager _gameManager;

	[Header("=== Pause 상태일 때 등장할 패널 ===")]
	[SerializeField] private GameObject _pausePanel;

	[Header("=== 게임 계속하기, 나가기 버튼(패널 내에 존재) ===")]
	[SerializeField] private Button _resumeButton;
	[SerializeField] private Button _quitButton;
	
	private Button _button;
	private bool _isPaused = false;
	
	private void Awake() {
		_button = GetComponent<Button>();
		_button.onClick.AddListener(PauseToggle);
		_resumeButton.onClick.AddListener(PauseToggle);
		_quitButton.onClick.AddListener(Quit);
		
		// 패널 처음엔 숨기기
		_pausePanel.SetActive(false);
	}
	
	private void PauseToggle() {
		if (!_isPaused) {
			Time.timeScale = 0f;
		
			// 일시정지 패널 보이기
			_pausePanel.SetActive(true);	
		} else {
			Time.timeScale = 1f;
			// 일시정지 패널 숨기기
			_pausePanel.SetActive(false);	
		}
		
		_isPaused = !_isPaused;
	}
	
	private void Quit() {
		_gameManager.EndGame();
	}

	private void Update() {
		if (Input.GetKeyDown(_gameManager.PauseKey)) {
			PauseToggle();
		}
	}
}