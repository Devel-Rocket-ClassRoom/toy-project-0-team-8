using UnityEngine;

public class Background : MonoBehaviour {
	private float Width { get; set; }
	public float ScrollSpeed { get; set; }
	
	private SpriteRenderer _backgroundImage;
	
	public void Init(Sprite sprite, bool isFirst) {
		_backgroundImage = GetComponent<SpriteRenderer>();
		_backgroundImage.sprite = sprite;
		
		Width = _backgroundImage.bounds.size.x;
		
		// isFirst가 true면 화면 가운데, false면 그 뒤에 이어서
		transform.position = isFirst ? 
			new Vector3(0, 0, 0) :
			new Vector3(Width, 0, 0);
	}
	
	private void Update() {
		// 백그라운드는, 내가 화면 밖으로 완전히 나가면 다시 뒤로 돌아와야 함
		if (transform.position.x <= -Width) {
			transform.position += Vector3.right * Width * 2f;
		}
	}
}