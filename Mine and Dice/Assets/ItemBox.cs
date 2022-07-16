using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ItemBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public ItemCarrier myItem;

    public ItemType myAcceptedType;

    public float backOffset = 0.5f;
    
    public UnityEvent itemAdded = new UnityEvent();

    public void OnPointerEnter(PointerEventData eventData) {
        if (ItemCarrier.currentlyBeingCarried != null) {
            if (ItemCarrier.currentlyBeingCarried.myType == myAcceptedType) {
                var die = ItemCarrier.currentlyBeingCarried.GetComponent<DieScript>();
                if (die != null) {
                    die.OnlyShowActiveFace(true);
                    if (!die.canDieStillBeActivated) {
                        return;

                    }
                }

                itemAddedCalled = false;
                myItem = ItemCarrier.currentlyBeingCarried.SnapToTarget(this);
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

    private bool itemAddedCalled = false;
    private void Update() {
        if (!itemAddedCalled) {
            if (myItem != null) {
                if (ItemCarrier.currentlyBeingCarried == null) {
                    itemAdded?.Invoke();
                    itemAddedCalled = true;
                }
            }
        }
    }

    public Vector3 GetSnapPos() {
        return transform.position + Vector3.forward * backOffset;
    }
}
