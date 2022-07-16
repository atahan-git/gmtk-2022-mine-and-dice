using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCarrier : MonoBehaviour {

    private bool dragging;
    public float curZOffset = 0;
    const float carryZOffset = 9;
    const float zLerpSpeed = 20;
    public GameObject shadow;

     Rigidbody rg;
     Collider cl;
     private DieScript die;

     public static ItemCarrier currentlyBeingCarried;

    private void Start() {
        rg = GetComponent<Rigidbody>();
        cl = GetComponent<Collider>();
        die = GetComponent<DieScript>();
    }

    public bool isSnapping = false;
    public DieBoxScript currentSnap;

    public void SnapToTarget(DieBoxScript target) {
        ClearSnapping(null);
        isSnapping = true;
        currentSnap = target;
        shadow.SetActive(false);
    }

    public void ClearSnapping(DieBoxScript source) {
        if (currentSnap == source) {
            isSnapping = false;
            shadow.SetActive(true);
        } else if(currentSnap == null) {
            currentSnap.OnPointerExit(null);
        } else {
            currentSnap.OnPointerExit(null);
        }
        currentSnap = null;
    }
    
    

    public void Update() {
        if (!isSnapping) {
            var targetPos = transform.position;
            if (dragging) {
                targetPos = GetMouseWorldPoint() + mouseOffset;
                curZOffset = Mathf.Lerp(curZOffset, carryZOffset, zLerpSpeed * Time.deltaTime);
                targetPos.z = curZOffset;
                transform.position = targetPos;
            }


            targetPos.z = 10;
            shadow.transform.position = targetPos;
            shadow.transform.rotation = Quaternion.identity;
        } else {
            transform.position = currentSnap.GetSnapPos();
        }
    }

    private Vector3 mouseOffset;
    public void OnMouseDown() {
        mouseOffset = transform.position - GetMouseWorldPoint();
        dragging = true;
        curZOffset = transform.position.z;
        rg.isKinematic = true;
        if(currentlyBeingCarried != null)
            currentlyBeingCarried.OnMouseUp();
        
        die.AlignRotation();

        currentlyBeingCarried = this;
    }

    public void OnMouseUp() {
        dragging = false;
        rg.isKinematic = false;

        if (currentlyBeingCarried == this)
            currentlyBeingCarried = null;
    }
	
    private static Vector3 GetMouseWorldPoint() {
        Vector3 screenPosDepth = Input.mousePosition;
        screenPosDepth.z = 10f; 
        return SceneLoader.s.ScreenSpaceCanvasCamera.ScreenToWorldPoint(screenPosDepth);
    }
}
