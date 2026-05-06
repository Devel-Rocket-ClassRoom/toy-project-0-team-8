using UnityEngine;
using UnityEngine.EventSystems;

public class SlideButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	[SerializeField] private CookieController _cookieController;
	
	public void OnPointerDown(PointerEventData eventData) {
		_cookieController.RequestSlidingStart();
	}
	
	public void OnPointerUp(PointerEventData eventData) {
		_cookieController.RequestSlidingEnd();
	}
}