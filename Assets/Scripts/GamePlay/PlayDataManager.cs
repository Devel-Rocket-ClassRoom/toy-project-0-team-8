using UnityEngine;

public class PlayDataManager : MonoBehaviour {
	public static PlayDataManager Instance;

	[HideInInspector] public CookieData CookieData;
	[HideInInspector] public int EarnedCoin;
	[HideInInspector] public int Score;
	[HideInInspector] public int StageIdx;
	[HideInInspector] public bool IsMaxScoreRenewed;

	private void Awake() {
		if (Instance != null) {
			Destroy(gameObject);
			return;
		}
		
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}
}