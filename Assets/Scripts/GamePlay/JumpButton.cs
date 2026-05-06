using UnityEngine;
using UnityEngine.UI;

public class JumpButton : MonoBehaviour {
	[SerializeField] private CookieController _cookieController;
	private Button _button;

	private void Awake() {
		_button = gameObject.GetComponent<Button>();
		_button.onClick.AddListener(_cookieController.RequestJump);
	}
}