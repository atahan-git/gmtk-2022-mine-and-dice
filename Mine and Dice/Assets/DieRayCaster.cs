using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRayCaster : MonoBehaviour {
    public static DieRayCaster s;

    public List<UIElementDragger> UIStuff = new List<UIElementDragger>();
    private void Awake() {
        s = this;
        UIStuff = new List<UIElementDragger>();
    }

    public LayerMask diceLayer;
    void Update() {
        if (DiceMaster.s.isRolling == false) {
            var camera = SceneLoader.s.ScreenSpaceCanvasCamera;

            if (Input.GetMouseButtonDown(0)) {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.GetPoint(0), ray.GetPoint(20));

                if (Physics.Raycast(ray, out RaycastHit hit, 10000, diceLayer)) {
                    var die = hit.collider.gameObject.GetComponent<ItemCarrier>();
                    if (die != null) {
                        die.GetComponent<ItemCarrier>().OnMouseDown();

                        for (int i = 0; i < UIStuff.Count; i++) {
                            UIStuff[i].skipHit = true;
                            UIStuff[i].OnPointerUp(null);
                        }
                    }
                }



            }

            if (Input.GetMouseButtonUp(0)) {
                /*Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.GetPoint(0), ray.GetPoint(20));
    
                if (Physics.Raycast(ray, out RaycastHit hit, 10000, diceLayer)) {
                    var die = hit.collider.gameObject.GetComponent<ItemCarrier>();
                    if (die != null) {
                        die.GetComponent<ItemCarrier>().OnMouseUp();
                    }
                }*/
                if (ItemCarrier.currentlyBeingCarried != null)
                    ItemCarrier.currentlyBeingCarried.OnMouseUp();

                for (int i = 0; i < UIStuff.Count; i++) {
                    UIStuff[i].skipHit = false;
                }
            }
        }
    }
}
