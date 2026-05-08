using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultManager : MonoBehaviour {
	private enum Grade {
		Silver,
		Gold,
		Emerald,
		Diamond,
		Rainbow,
		Crystal,
	}
	
	private Sprite GetGradeSpriteByStageNum(int stage) => stage switch {
		1 => Resources.Load<Sprite>($"Sprite/ResultScene/{Grade.Silver}"),
		2 => Resources.Load<Sprite>($"Sprite/ResultScene/{Grade.Gold}"),
		3 => Resources.Load<Sprite>($"Sprite/ResultScene/{Grade.Emerald}"),
		>= 4 => Resources.Load<Sprite>($"Sprite/ResultScene/{Grade.Rainbow}"),
		_ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null)
	};
	
	private string GetStageResultStringByStageNum(int stage) => stage switch {
		1 => $"스테이지 1 : 마녀의 집",
		2 => $"스테이지 2 : 젤리벌레의 숲",
		3 => $"스테이지 3 : 멀리멀리 바다",
		>= 4 => "클리어!",
		_ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null)
	};
	
	private int _score;
	private int _earnedCoin;
	private int _stageIdx;
	private bool _isMaxScoreRenewed;
	private CookieData _cookieData;
	private readonly float _appearPeriod = 0.5f;
	private readonly float _lerpDuration = 0.5f;

	[Header("=== 캐릭터가 표시될 이미지 ===")]
	[SerializeField] private Image _characterImage;
	
	[Header("=== 스테이지 클리어 결과 표시할 텍스트와 아이콘 ===")]
	[SerializeField] private TextMeshProUGUI _stageText;
	[SerializeField] private Image _gradeImage;
	
	[Header("=== 스테이지 중 얻은 코인 양 표시할 텍스트와 아이콘 ===")] 
	[SerializeField] private Image _coinIcon;
	[SerializeField] private TextMeshProUGUI _coinText;
	
	[Header("=== 점수 표시할 텍스트와 아이콘 ===")] 
	[SerializeField] private Image _scoreIcon;
	[SerializeField] private TextMeshProUGUI _scoreText;

	[Header("=== Best Score 아이콘 ===")] 
	[SerializeField] private Image _bestScoreImage;

	[Header("=== 확인 버튼 ===")]
	[SerializeField] private Button _confirmButton;

	[Header("=== 씬 체인저 등록 ===")]
	[SerializeField] ChangeScene _changeScene;
	
	private Coroutine _resultCoroutine;
	
	private void Awake() {
		// 필요한 데이터 먼저 로딩
		_cookieData = PlayDataManager.Instance.CookieData;
		_score = PlayDataManager.Instance.Score;
		_stageIdx = PlayDataManager.Instance.StageIdx;
		_earnedCoin = PlayDataManager.Instance.EarnedCoin;
		_isMaxScoreRenewed = PlayDataManager.Instance.IsMaxScoreRenewed;
		
		// 표시할 아이콘 전부 비활성화
		_characterImage.gameObject.SetActive(false);
		_stageText.gameObject.SetActive(false);
		_coinIcon.gameObject.SetActive(false);
		_coinText.gameObject.SetActive(false);
		_scoreIcon.gameObject.SetActive(false);
		_scoreText.gameObject.SetActive(false);
		
		// 결과 아이콘과 최대점수 갱신 아이콘은 비활성화하면 레이아웃이 이상해져서, 투명도 0으로
		_gradeImage.color = new Color(1, 1, 1, 0);
		_bestScoreImage.color = new Color(1, 1, 1, 0);
		
		// 미리 이미지를 정해둬야 Text위치가 바뀌지 않음
		_gradeImage.sprite = GetGradeSpriteByStageNum(_stageIdx);
		
		// 확인 버튼 누르면 메인으로 돌아가게 구성
		_confirmButton.onClick.AddListener(_changeScene.OnLobbyScene);
	}
	
	// Coroutine을 이용해 값 갱신
	private void Start() {
		_resultCoroutine = StartCoroutine(CoUpdateResult());
	}

	private void Update() {
		// ESC, Space, 혹은 마우스 버튼 클릭하면 결과 스킵
		if (Input.GetKeyDown(KeyCode.Escape) || 
		    Input.GetKeyDown(KeyCode.Space) || 
		    Input.GetMouseButtonDown(0)) 
		{
			if (_resultCoroutine != null) { StopCoroutine(_resultCoroutine); }
			UpdateAllResults();
		}
	}

	private IEnumerator CoUpdateResult() {
		// 캐릭터 표시
		_characterImage.gameObject.SetActive(true);
		_characterImage.sprite = _cookieData.SpriteIcon;
		yield return new WaitForSeconds(_appearPeriod);
		
		// 스테이지 텍스트 표시
		_stageText.gameObject.SetActive(true);
		_stageText.text = GetStageResultStringByStageNum(_stageIdx);
		yield return new WaitForSeconds(_appearPeriod);
		
		// 스테이지 Grade Sprite 표시
		_gradeImage.color = new Color(1, 1, 1, 1);
		_gradeImage.sprite = GetGradeSpriteByStageNum(_stageIdx);
		_gradeImage.SetNativeSize();
		yield return new WaitForSeconds(_appearPeriod);
		
		// 코인 아이콘 표시
		_coinIcon.gameObject.SetActive(true);
		yield return new WaitForSeconds(_appearPeriod);
		
		// 코인 값 표시
		float timer = 0f;
		_coinText.gameObject.SetActive(true);
		while (timer <= _lerpDuration) {
			timer += Time.deltaTime;
			_coinText.text = ((int)Mathf.Lerp(0, _earnedCoin, timer / _lerpDuration)).ToString("N0");
			yield return null;
		}
		_coinText.text = _earnedCoin.ToString("N0");
		yield return new WaitForSeconds(_appearPeriod);
		
		// 젤리 아이콘 표시
		_scoreIcon.gameObject.SetActive(true);
		yield return new WaitForSeconds(_appearPeriod);
		
		// 젤리 값 표시
		timer = 0f;
		_scoreText.gameObject.SetActive(true);
		while (timer <= _lerpDuration) {
			timer += Time.deltaTime;
			_scoreText.text = ((int)Mathf.Lerp(0, _score, timer / _lerpDuration)).ToString("N0");
			yield return null;
		}
		_scoreText.text = _score.ToString("N0");
		yield return new WaitForSeconds(_appearPeriod);
		
		// Best Score 갱신시에만 표시
		if (_isMaxScoreRenewed) { _gradeImage.color = new Color(1, 1, 1, 1); }
		
		// 마지막에 결과값 한번 쭉 갱신
		UpdateAllResults();
		
		_resultCoroutine = null;
	}
	
	// 결과값 쭉 갱신
	private void UpdateAllResults() {
		_characterImage.gameObject.SetActive(true);
		_characterImage.sprite = _cookieData.SpriteIcon;
		
		// 스테이지 텍스트 표시
		_stageText.gameObject.SetActive(true);
		_stageText.text = GetStageResultStringByStageNum(_stageIdx);
		
		// 스테이지 Grade Sprite 표시
		_gradeImage.color = new Color(1, 1, 1, 1);
		_gradeImage.SetNativeSize();
		
		// 코인 아이콘 표시
		_coinIcon.gameObject.SetActive(true);
		
		// 코인 값 표시
		_coinText.gameObject.SetActive(true);
		_coinText.text = _earnedCoin.ToString("N0");
		
		// 젤리 아이콘 표시
		_scoreIcon.gameObject.SetActive(true);
		
		// 젤리 값 표시
		_scoreText.gameObject.SetActive(true);
		_scoreText.text = _score.ToString("N0");
		
		// Best Score 표시 여부 확인해서 표시
		if (_isMaxScoreRenewed) { _gradeImage.color = new Color(1, 1, 1, 1); }
	}
}