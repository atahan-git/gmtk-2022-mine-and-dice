using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour {

    public GameObject hp;
    public GameObject hpButWillBeLost;
    public GameObject empty;

    public Transform hpTransform;

    public void UpdateHealthDisplay(int currentHealth, int maxHealth, int endOfTurnHealth) {
        hpTransform.ClearAllChildren();

        for (int i = 0; i < maxHealth; i++) {
            if (i < endOfTurnHealth) {
                Instantiate(hp, hpTransform);
            } else if (i < currentHealth) {
                Instantiate(hpButWillBeLost, hpTransform);
            }else {
                Instantiate(empty, hpTransform);
            }
        }
        
    }
}
