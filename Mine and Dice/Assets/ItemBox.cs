using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public ItemCarrier myItem;

    public ItemType myAcceptedType;

    public float backOffset = 0.5f;
    
    public void OnPointerEnter(PointerEventData eventData) {
        if (ItemCarrier.currentlyBeingCarried != null) {
            if (ItemCarrier.currentlyBeingCarried.myType == myAcceptedType) {
                myItem = ItemCarrier.currentlyBeingCarried.SnapToTarget(this);
               var die = ItemCarrier.currentlyBeingCarried.GetComponent<DieScript>();
               if (die != null) {
                   die.OnlyShowActiveFace(true);
               }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (ItemCarrier.currentlyBeingCarried != null) {
            if (ItemCarrier.currentlyBeingCarried == myItem) {
                ItemCarrier.currentlyBeingCarried.ClearSnapping(this);
                var die = ItemCarrier.currentlyBeingCarried.GetComponent<DieScript>();
                if (die != null) {
                    die.OnlyShowActiveFace(false);
                }
                myItem = null;
            }
        }
    }

    public Vector3 GetSnapPos() {
        return transform.position + Vector3.forward * backOffset;
    }
}
