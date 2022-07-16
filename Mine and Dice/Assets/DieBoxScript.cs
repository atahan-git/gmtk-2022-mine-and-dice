using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DieBoxScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private CardScript myCardScript;

    private void Start() {
        myCardScript = GetComponentInParent<CardScript>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (ItemCarrier.currentlyBeingCarried != null)
            ItemCarrier.currentlyBeingCarried.SnapToTarget(this);
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (ItemCarrier.currentlyBeingCarried != null)
            ItemCarrier.currentlyBeingCarried.ClearSnapping(this);
    }

    public Vector3 GetSnapPos() {
        return transform.position + Vector3.forward * 0.5f;
    }
}
