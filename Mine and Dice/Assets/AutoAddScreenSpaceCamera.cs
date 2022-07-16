using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAddScreenSpaceCamera : MonoBehaviour
{
    void Start() {
        GetComponent<Canvas>().worldCamera = SceneLoader.s.ScreenSpaceCanvasCamera;
    }
}
