using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElementDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private bool dragging;

	public void Update() {
		if (dragging) {
			transform.position = GetMouseWorldPoint() + mouseOffset;
		}
	}

	private Vector3 mouseOffset;
	public void OnPointerDown(PointerEventData eventData) {
		mouseOffset = transform.position - GetMouseWorldPoint();
		dragging = true;
	}

	public void OnPointerUp(PointerEventData eventData) {
		dragging = false;
	}
	
	private static Vector3 GetMouseWorldPoint() {
		Vector3 screenPosDepth = Input.mousePosition;
		screenPosDepth.z = 10f; 
		return SceneLoader.s.ScreenSpaceCanvasCamera.ScreenToWorldPoint(screenPosDepth);
	}
}