using System.Collections;
using UnityEngine;

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

	[Header("=== StageData 목록 ===")] 
	[SerializeField] private StageData[] _stageDatas;
	[SerializeField] public GameObject InvisibleGround;
	
	private readonly float _giantScaleMultiplier = 3f;
	private readonly float _dashSpeedMultiplier = 1.5f;
	
	private StageData _currentStage;
	private float _scrollSpeed;
	
	private Coroutine _coMagnet;
	private Coroutine _coGiant;
	private Coroutine _coDash;
	
	private float _giantGrowDuration = 0.5f;
	
	public bool ScrollObjectsFlag { get; set; } = true;
	public bool GameEndFlag { get; set; } = false;

	private void Start() {
		CookieData data = DataTableManager.CookieTable.Get("Cookie_Hero");
		LoadCharacter(data);
		LoadStage(_stageDatas[0]);
		
		MagnetArea.gameObject.SetActive(false);
		InvisibleGround.SetActive(false);
	}

	public void LoadStage(StageData stageData) {
		_currentStage = stageData;
		_scrollSpeed = stageData.scrollSpeed;
		
		_backgroundRendererA.Init(stageData.background, true);
		_backgroundRendererB.Init(stageData.background, false);
		
		Instantiate(stageData.stagePrefab, _stageRoot);
	}
	
	public void LoadCharacter(CookieData cookieData) {
		_cookieController.Init(cookieData, this);
	}
	
	private void Update() {
		if (_currentStage ==null) return;
		if (ScrollObjectsFlag && !GameEndFlag) {
			_backgroundRendererA.transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;
			_backgroundRendererB.transform.position += Vector3.left * _scrollSpeed * Time.deltaTime;
			// StageRoot위에 Prefab을 생성하고, StageRoot를 밀면서 맵 진행 처리
			_stageRoot.position += Vector3.left * _scrollSpeed * Time.deltaTime;	
		}
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
		yield return StartCoroutine(CoGrowToGiant(true));
		yield return new WaitForSeconds(duration);
		yield return StartCoroutine(CoGrowToGiant(false));
		
		_coGiant = null;
	}
	
	private IEnumerator CoGrowToGiant(bool beBigger) {
		float growTimer = 0f;
		Vector3 startScale = _cookieController.transform.localScale;
		Vector3 endScale = beBigger ? startScale * _giantScaleMultiplier : startScale / _giantScaleMultiplier; 
		
		while (growTimer <= _giantGrowDuration) {
			growTimer += Time.deltaTime;
			_cookieController.transform.localScale = Vector3.Lerp(startScale, endScale, growTimer / _giantGrowDuration);
			yield return null;
		}
	}
	
	public void ActivateDash(float duration) {
		_coDash = StartCoroutine(CoDash(duration));
	}
	
	private IEnumerator CoDash(float duration) {
		// IsDashing 활성화
		_cookieController.IsDashing = true;
		// 스크롤 속도 높이기
		_scrollSpeed *= _dashSpeedMultiplier;
		// 투명 바닥 활성화
		InvisibleGround.SetActive(true);
		
		yield return new WaitForSeconds(duration);
		
		// IsDashing 비활성화
		_cookieController.IsDashing = false;
		// 스크롤 속도 원상복귀
		_scrollSpeed /= _dashSpeedMultiplier;
		// 투명 바닥 비활성화
		InvisibleGround.SetActive(false);
	}
}
