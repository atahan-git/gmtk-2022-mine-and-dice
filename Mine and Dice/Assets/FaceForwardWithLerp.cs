using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceForwardWithLerp : MonoBehaviour {
    public float lerpSpeed = 20f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity,lerpSpeed*Time.deltaTime);
    }
}
