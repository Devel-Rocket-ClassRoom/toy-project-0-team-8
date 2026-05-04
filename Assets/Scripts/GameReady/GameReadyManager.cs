using System;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class GameReadyManager : MonoBehaviour {
	[SerializeField] private Sprite[] _backgroundSprites;
	[SerializeField] private Image _backgroundImage;
	
	private int stageNum;
	
	// 씬 시작 시에 값 넘겨서 몇 번 사용할지 결정
	public void Init() {
		stageNum = 0;
		
		_backgroundImage.sprite = _backgroundSprites[stageNum];
	}
	
	private void Start() {
		// 임시로 start에서 Init 호출하도록 설정
		Init();
	}
}
