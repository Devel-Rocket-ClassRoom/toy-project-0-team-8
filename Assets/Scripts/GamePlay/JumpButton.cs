using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JumpButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler{
	[SerializeField] private CookieController _cookieController;
	private Button _button;
	private bool _isPressed;
	
	[HideInInspector] public UnityEvent OnButtonDown;
	[HideInInspector] public UnityEvent OnButtonUp;
	[HideInInspector] public UnityEvent WhileButtonPressed;

	public void OnPointerDown(PointerEventData eventData) {
		OnButtonDown?.Invoke();
		_cookieController.RequestJump();
		_isPressed = true;
	}
	
	public void OnPointerUp(PointerEventData eventData) {
		OnButtonUp?.Invoke();
		_isPressed = false;
	}

	private void Update() {
		if (_isPressed) {
			WhileButtonPressed?.Invoke();
		}
	}
}