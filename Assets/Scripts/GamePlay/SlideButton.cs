using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SlideButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	[SerializeField] private CookieController _cookieController;
	private bool _isPressed;

	[HideInInspector] public UnityEvent OnButtonDown;
	[HideInInspector] public UnityEvent OnButtonUp;
	[HideInInspector] public UnityEvent WhileButtonPressed;
	
	public void OnPointerDown(PointerEventData eventData) {
		OnButtonDown?.Invoke();
		_cookieController.RequestSlidingStart();
		_isPressed = true;
	}
	
	public void OnPointerUp(PointerEventData eventData) {
		OnButtonUp?.Invoke();
		_cookieController.RequestSlidingEnd();
		_isPressed = false;
	}

	private void Update() {
		if (_isPressed) {
			WhileButtonPressed.Invoke();
		}
	}
}