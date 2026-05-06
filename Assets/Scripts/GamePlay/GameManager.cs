using UnityEngine;

public class GameManager : MonoBehaviour {

	[Header("=== 생성된 Prefab들이 배치될 Root ===")]
	[SerializeField] private Transform _stageRoot;

	[Header("=== 백그라운드 스크롤링 할 오브젝트 둘 ===")]
	[SerializeField] private Background _backgroundRendererA;
	[SerializeField] private Background _backgroundRendererB;

	[Header("=== 사용될 CookieController ===")]
	[SerializeField] private CookieController _cookieController;

	[Header("=== StageData 목록 ===")] 
	[SerializeField] private StageData[] _stageDatas;
	[SerializeField] public GameObject InvisibleGround;
	
	private StageData _currentStage;
	private float scrollSpeed;
	
	public bool ScrollObjectsFlag { get; set; } = true;
	public bool GameEndFlag { get; set; } = false;

	private void Start() {
		CookieData data = DataTableManager.CookieTable.Get("Cookie_Hero");
		LoadCharacter(data);
		LoadStage(_stageDatas[0]);
		
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
}
