using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;

public class GameReadyManager : MonoBehaviour {
	[SerializeField] private Sprite[] _backgroundSprites;
	[SerializeField] private Image _backgroundImage;
	[SerializeField] private TextMeshProUGUI _maxScoreText;

	public Image cookie;
	public Image[] gear;

	public GachaManager data;

	private int stageNum;
	
	// 씬 시작 시에 값 넘겨서 몇 번 사용할지 결정
	public void Init() {
		stageNum = 0;
		
		_maxScoreText.text = SaveLoadManager.Data.score.ToString("N0");
		_backgroundImage.sprite = _backgroundSprites[stageNum];
	}
	
	private void Start() {
		// 임시로 start에서 Init 호출하도록 설정
		Init();
	}

    private void OnEnable()
    {
		string cookieId = SaveLoadManager.Data.currentCookie;
		string[] gearId = new string[gear.Length];

		gearId = SaveLoadManager.Data.currentGear;

		foreach(var id in data.cookieList)
		{
			if(id.cookieId == cookieId)
			{
				cookie.sprite = id.Icon;
            }
		}

        for (int i = 0; i < gearId.Length; i++)
        {
            string targetId = gearId[i];

            // ItemList에서 ID가 일치하는 스크립터블 오브젝트 찾기
            foreach (var item in data.itemList)
            {
                if (item.itemId == targetId)
                {
                    if (i < gear.Length)
                    {
                        gear[i].sprite = item.Icon;
                    }
                    break; 
                }
            }
        }

    }
}
