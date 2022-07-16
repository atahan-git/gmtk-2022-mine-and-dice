using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFieldReference : MonoBehaviour {
    public static ShadowFieldReference s;

    private void Awake() {
	    s = this;
    }
}
