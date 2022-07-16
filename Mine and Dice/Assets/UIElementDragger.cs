using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIElementDragger : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	private bool dragging;
	public float curZOffset = 0;
	const float carryZOffset = 9;
	const float zLerpSpeed = 20;
	public GameObject shadow;
	private float baseZ;

	private void Start() {
		baseZ = transform.position.z;
		shadow.transform.SetParent(ShadowFieldReference.s.transform);
	}

	public void Update() {
		var targetPos = transform.position;
		print(targetPos);
		if (dragging) {
			targetPos = GetMouseWorldPoint() + mouseOffset;
			curZOffset = Mathf.Lerp(curZOffset, carryZOffset, zLerpSpeed * Time.deltaTime);
		} else {
			curZOffset = Mathf.Lerp(curZOffset, baseZ, zLerpSpeed * Time.deltaTime);
		}

		targetPos.z = curZOffset;
		transform.position = targetPos;
		targetPos.z = baseZ;
		shadow.transform.position = targetPos;
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