using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieRayCaster : MonoBehaviour {

    public LayerMask diceLayer;
    void Update() {
        var camera = SceneLoader.s.ScreenSpaceCanvasCamera;
        
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.GetPoint(0), ray.GetPoint(20));

            if (Physics.Raycast(ray, out RaycastHit hit, 10000, diceLayer)) {
                var die = hit.collider.gameObject.GetComponent<DieScript>();
                if (die != null) {
                    die.GetComponent<ItemCarrier>().OnMouseDown();
                }
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.GetPoint(0), ray.GetPoint(20));

            if (Physics.Raycast(ray, out RaycastHit hit, 10000, diceLayer)) {
                var die = hit.collider.gameObject.GetComponent<DieScript>();
                if (die != null) {
                    die.GetComponent<ItemCarrier>().OnMouseUp();
                }
            }
        }
    }
}
