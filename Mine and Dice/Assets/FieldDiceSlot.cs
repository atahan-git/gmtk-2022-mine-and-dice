using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldDiceSlot : MonoBehaviour {
	public LayerMask diceLayer;
	
	public bool CheckIfDiceInside() {
		var collider = GetComponent<BoxCollider>();
		Vector3 worldCenter = collider.transform.TransformPoint(collider.center);
		Vector3 worldHalfExtents = collider.transform.TransformVector(collider.size * 0.5f); // only necessary when collider is scaled by non-uniform transform
		var overlapBox = Physics.OverlapBox(worldCenter, worldHalfExtents, collider.transform.rotation, diceLayer);

		return overlapBox.Length > 0;
	}
}
