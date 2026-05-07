using System;
using UnityEngine;

public class PlayDataManager : MonoBehaviour {
	public static PlayDataManager Instance;
	
	public CookieData CookieData;
	public int EarnedCoin;
	public int Score;
	public int StageIdx;
	public bool IsMaxScoreRenewed;

	private void Awake() {
		if (Instance != null) {
			Destroy(gameObject);
			return;
		}
		
		Instance = this;
		DontDestroyOnLoad(gameObject);
	}
}