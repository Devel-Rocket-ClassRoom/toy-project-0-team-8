using UnityEngine;

[CreateAssetMenu(menuName = "CookieRun/Stage Data")]
public class StageData : ScriptableObject {
	public int stageId;
	public float stageLength;
	public float scrollSpeed;
	public Sprite background;
	
	public GameObject stagePrefab;
}