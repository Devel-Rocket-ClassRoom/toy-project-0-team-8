using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	[Header("=== 사용될 CookieController ===")]
	[SerializeField] private CookieController _cookieController;

	[Header("=== 게임에서 사용될 자석 영역 ===")]
	[SerializeField] public MagnetArea MagnetArea;

	[Header("=== 생성된 Prefab들이 배치될 Root ===")]
	[SerializeField] private Transform _stageRoot;

	[Header("=== 백그라운드 스크롤링 할 오브젝트 둘 ===")]
	[SerializeField] private Background _backgroundRendererA;
	[SerializeField] private Background _backgroundRendererB;

	[Header("=== UI에 표시될 코인, 점수 텍스트 ===")]
	[SerializeField] private TextMeshProUGUI _scoreText;
	[SerializeField] private TextMeshProUGUI _earnedCoinText;

	[Header("=== StageData 목록 ===")] 
	[SerializeField] private StageData[] _stageDatas;
	[SerializeField] public GameObject InvisibleGround;
	
	[Header("=== 착용한 유물을 각각 표현할 1, 2, 3번 슬롯 ===")]
	[SerializeField] private InGameGearSlot _firstGearSlot;
	[SerializeField] private InGameGearSlot _secondGearSlot;
	[SerializeField] private InGameGearSlot _thirdGearSlot;
	[Header("=== 착용한 유물을 각각 표현할 1, 2, 3번 이미지 ===")]
	[SerializeField] private Image _firstGearImage;
	[SerializeField] private Image _secondGearImage;
	[SerializeField] private Image _thirdGearImage;

	[SerializeField] private ChangeScene _changeScene;
	
	private float _initialScrollSpeed;
	private Vector3 _initialCookieScale;
	private readonly float _giantScaleMultiplier = 3f;
	private readonly float _dashSpeedMultiplier = 1.5f;
	private readonly float _godmodeDurationAfterItem = 1.0f;
	
	private MapPrefab _currentStage;
	private float _scrollSpeed;
	
	private Coroutine _coMagnet;
	private Coroutine _coGiant;
	private Coroutine _coDash;
	
	private float _giantGrowDuration = 0.5f;
	
	private int _score = 0;
	private int _earnedCoin = 0;
	
	public bool ScrollObjectsFlag { get; set; } = true;
	public bool GameEndFlag { get; set; } = false;
	
	private KeyCode _pauseKey = KeyCode.Escape;
	public KeyCode PauseKey => _pauseKey;

	private void Start() {
		Init();
	}
	
	private int _stageIdx = 0;
	
	public void Init() {
		// 파일 로딩 후 추가
		SaveLoadManager.Load();
		string cookie = SaveLoadManager.Data.currentCookie;
		CookieData data = DataTableManager.CookieTable.Get(cookie);
		
		PlayDataManager.Instance.CookieData = data;
		
		// 스테이지 및 캐릭터 로딩
		LoadCharacter(data);
		LoadNextStage();

        // 맵 가리는 Panel을 잠시동안 치워두기 위한 Coroutine
        StartCoroutine(CoRemoveBlindPanel());
		
		MagnetArea.gameObject.SetActive(false);
		InvisibleGround.SetActive(false);
		
		_initialCookieScale = _cookieController.transform.localScale;
		_initialScrollSpeed = _scrollSpeed;
	}
	
	// 유물 설정하기, 없다면 입력 안해도 됨
	public void SetGears(List<GearBase> gears) {
		_firstGearSlot.Init(gears.Count >= 1 ? gears[0] : null);
		_secondGearSlot.Init(gears.Count >= 2 ? gears[1] : null);
		_thirdGearSlot.Init(gears.Count >= 3 ? gears[2] : null);
	}
	
	// 유물 이미지 설정하기, 역시 없다면 입력 안 해도 됨
	public void SetGearImages(List<GearData> gears) {
		_firstGearImage.sprite = gears.Count >= 1 ? gears[0].SpriteIcon : null;
		_secondGearImage.sprite = gears.Count >= 2 ? gears[1].SpriteIcon : null;
		_thirdGearImage.sprite = gears.Count >= 3 ? gears[2].SpriteIcon : null;
	}
	
	// 첫 스테이지는 가리는 패널이 없어야 해서 사용
	// 패널을 가리고, 10초 후에 등장시킨다.
	private IEnumerator CoRemoveBlindPanel() {
		_currentStage.DisappearBlindPanels();
		yield return new WaitForSeconds(10f);
		_currentStage.AppearBlindPanels();
	}

	public void LoadNextStage() {
		// 스테이지 모두 깼다면, 클리어!
		if (_stageIdx >= _stageDatas.Length) {
			_stageIdx++;
			EndGame();
			return;
		}
		
		// 스테이지 데이터 세팅
		StageData stageData = _stageDatas[_stageIdx++];
		
		// stageRoot내의 모든 오브젝트 지우기
		foreach (Transform child in _stageRoot) {
			Destroy(child.gameObject);
		}
		
		// 새 맵 생성
		_currentStage = Instantiate(stageData.StagePrefab, _stageRoot);
		_scrollSpeed = stageData.ScrollSpeed;
		// 백그라운드 이미지 교체
		_backgroundRendererA.Init(stageData.Background, true);
		_backgroundRendererB.Init(stageData.Background, false);
		// StageRoot를 다시 0으로 땡겨오기
		_stageRoot.transform.position = new Vector3(0, _stageRoot.position.y, _stageRoot.position.z);
	}
	
	public void LoadCharacter(CookieData cookieData) {
		_cookieController.Init(cookieData, this);
	}
	
	private void Update() {
		if (_currentStage ==null) return;
		
		// 스크롤을 진행해야 한다면, 스크롤 진행
		if (ScrollObjectsFlag && !GameEndFlag) {
			_backgroundRendererA.transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;
			_backgroundRendererB.transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;
			// StageRoot위에 Prefab을 생성하고, StageRoot를 밀면서 맵 진행 처리
			_stageRoot.position += Vector3.left * _scrollSpeed * Time.deltaTime;
		}
		
		// 무적이거나, 대쉬중이면 투명 바닥 활성화
		if (_cookieController.IsDashing || _cookieController.IsGodMode) {
			InvisibleGround.SetActive(true);
		} else {
			InvisibleGround.SetActive(false);
		}
		
		// 매 프레임 UI 갱신
		_earnedCoinText.text = _earnedCoin.ToString("N0");
		_scoreText.text = _score.ToString("N0");
	}
	
	public void ActivateMagnet(float duration) {
		if (_coMagnet != null) StopCoroutine(_coMagnet);
		_coMagnet = StartCoroutine(CoMagnet(duration));
	}
	
	private IEnumerator CoMagnet(float duration) {
		MagnetArea.gameObject.SetActive(true);
		yield return new WaitForSeconds(duration);
		MagnetArea.gameObject.SetActive(false);
		_coMagnet = null;
	}
	
	public void ActivateGiant(float duration) {
		if (_coGiant != null) StopCoroutine(_coGiant);
		_coGiant = StartCoroutine(CoGiant(duration));
	}
	
	private IEnumerator CoGiant(float duration) {
		// 거인모드 활성화
		_cookieController.IsGiantMode = true;
		// 0.5초동안 커지기
		yield return StartCoroutine(CoGrowToGiant(true));
		// 기다리기
		yield return new WaitForSeconds(duration);
		// 0.5초동안 작아지기
		yield return StartCoroutine(CoGrowToGiant(false));
		// 거인모드 비활성화
		_cookieController.IsGiantMode = false;
		// 종료 시 무적모드 1초간 활성화
		_cookieController.EnableGodMode(_godmodeDurationAfterItem);
		
		_coGiant = null;
	}
	
	// 0.5초동안 줄어들기, 커지기 애니메이션 등장
	private IEnumerator CoGrowToGiant(bool beBigger) {
		float growTimer = 0f;
		Vector3 startScale = _cookieController.transform.localScale;
		Vector3 endScale = beBigger ? _initialCookieScale * _giantScaleMultiplier : _initialCookieScale; 
		
		while (growTimer <= _giantGrowDuration) {
			growTimer += Time.deltaTime;
			_cookieController.transform.localScale = Vector3.Lerp(startScale, endScale, growTimer / _giantGrowDuration);
			yield return null;
		}
		
		_cookieController.transform.localScale = endScale;
	}
	
	public void ActivateDash(float duration) {
		if (_coDash != null) StopCoroutine(_coDash);
		_coDash = StartCoroutine(CoDash(duration));
	}
	
	private IEnumerator CoDash(float duration) {
		// IsDashing 활성화
		_cookieController.IsDashing = true;
		// 스크롤 속도 높이기
		_scrollSpeed = _initialScrollSpeed *_dashSpeedMultiplier;
		
		yield return new WaitForSeconds(duration);
		
		// IsDashing 비활성화
		_cookieController.IsDashing = false;
		// 스크롤 속도 원상복귀
		_scrollSpeed = _initialScrollSpeed;
		// 종료 시 무적모드 1초간 활성화
		_cookieController.EnableGodMode(_godmodeDurationAfterItem);
		
		_coDash = null;
	}
	
	public void AddScore(int amount) => _score += amount;
	public void AddCoin(int amount) => _earnedCoin += amount;
	
	public void EndGame() {
		// PlayDataManager에 값 갱신
		PlayDataManager.Instance.EarnedCoin = _earnedCoin;
		PlayDataManager.Instance.Score = _score;
		PlayDataManager.Instance.IsMaxScoreRenewed = SaveLoadManager.Data.score < _score;
		PlayDataManager.Instance.StageIdx = _stageIdx;
		
		// 이번 게임에 얻은 코인과 점수 SaveLoadManager에 저장
		SaveLoadManager.Data.Coin += _earnedCoin;
		SaveLoadManager.Data.score = Math.Max(SaveLoadManager.Data.score, _score);
		SaveLoadManager.Save();
		
		// 다음 씬으로 넘기기
		_changeScene.OnResultScene();
		
		Debug.Log($"게임 종료");
	}

}
