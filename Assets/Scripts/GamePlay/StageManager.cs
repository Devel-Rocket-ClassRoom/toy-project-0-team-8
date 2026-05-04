using UnityEngine;
using UnityEngine.TextCore.Text;

public class StageManager : MonoBehaviour {

	[Header("=== 생성된 Prefab들이 배치될 Root ===")]
	[SerializeField] private Transform _stageRoot;

	[Header("=== 백그라운드 스크롤링 할 오브젝트 둘 ===")]
	[SerializeField] private Background _backgroundRendererA;
	[SerializeField] private Background _backgroundRendererB;

	[Header("=== StageData 목록 ===")] 
	[SerializeField] private StageData[] _stageDatas;
	
	private StageData _currentStage;
	private float scrollSpeed;

	private void Start() {
		LoadStage(_stageDatas[0]);
	}

	public void LoadStage(StageData stageData) {
		_currentStage = stageData;
		scrollSpeed = stageData.scrollSpeed;
		
		_backgroundRendererA.Init(stageData.background, true);
		_backgroundRendererB.Init(stageData.background, false);
		_backgroundRendererA.ScrollSpeed = scrollSpeed;
		_backgroundRendererB.ScrollSpeed = scrollSpeed;
		
		Instantiate(stageData.stagePrefab, _stageRoot);
	}
	
	private void Update() {
		if (_currentStage == null) return;
		
		_stageRoot.position += Vector3.left * scrollSpeed * Time.deltaTime;
	}
}
