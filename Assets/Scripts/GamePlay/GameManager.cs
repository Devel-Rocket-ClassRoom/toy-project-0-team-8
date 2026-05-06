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
	
	private readonly float _scaleMultiplier = 3f;
	
	private StageData _currentStage;
	private float scrollSpeed;
	
	private Coroutine _coMagnet;
	private Coroutine _coGiant;
	
	private float _giantGrowDuration = 0.5f;
	
	public bool ScrollObjectsFlag { get; set; } = true;
	public bool GameEndFlag { get; set; } = false;

	private void Start() {
		CookieData data = DataTableManager.CookieTable.Get("Cookie_Pirate");
		LoadCharacter(data);
		LoadStage(_stageDatas[0]);
		
		MagnetArea.gameObject.SetActive(false);
		InvisibleGround.SetActive(false);
	}

	public void LoadStage(StageData stageData) {
		_currentStage = stageData;
		scrollSpeed = stageData.scrollSpeed;
		
		_backgroundRendererA.Init(stageData.background, true);
		_backgroundRendererB.Init(stageData.background, false);
		
		Instantiate(stageData.stagePrefab, _stageRoot);
	}
	
	public void LoadCharacter(CookieData cookieData) {
		_cookieController.Init(cookieData);
	}
	
	private void Update() {
		if (_currentStage ==null) return;
		if (ScrollObjectsFlag && !GameEndFlag) {
			_backgroundRendererA.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
			_backgroundRendererB.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
			// StageRoot위에 Prefab을 생성하고, StageRoot를 밀면서 맵 진행 처리
			_stageRoot.position += Vector3.left * scrollSpeed * Time.deltaTime;	
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
		Vector3 endScale = beBigger ? startScale * _scaleMultiplier : startScale / _scaleMultiplier; 
		
		while (growTimer <= _giantGrowDuration) {
			growTimer += Time.deltaTime;
			_cookieController.transform.localScale = Vector3.Lerp(startScale, endScale, growTimer / _giantGrowDuration);
			yield return null;
		}
	}
}
