using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DieCollisionEffectsScript : MonoBehaviour {

    public List<AudioSource> sources = new List<AudioSource>();

    public GameObject collisionSparks;

    public float variance = 0.1f;
    public AudioClip[] hitDieSounds;
    private void Start() {
        /*var source = GetComponentInChildren<AudioSource>();
        
        sources.Add(source);

        for (int i = 0; i < 3; i++) {
            sources.Add(Instantiate(source.gameObject, transform).GetComponent<AudioSource>());
        }*/
    }


    private int n = 0;
    public bool skipNextCollision = false;

    public float collisionMagnitudeCutoff = 0.05f;
    public float soundDivider = 2f;
    private void OnCollisionEnter(Collision collision) {
        if (skipNextCollision) {
            skipNextCollision = false;
        } else {
            var otherDie = collision.gameObject.GetComponent<DieCollisionEffectsScript>();
            if (otherDie != null) {
                otherDie.skipNextCollision = true;
            }

            var magnitude = collision.relativeVelocity.magnitude;
            if (magnitude > collisionMagnitudeCutoff) {
                /*sources[n].clip = hitDieSounds[Random.Range(0, hitDieSounds.Length)];
                sources[n].pitch = Random.Range(1 - variance, 1 + variance);
                sources[n].Play();*/

                var effect = Instantiate(collisionSparks, collision.contacts[0].point, Quaternion.identity);

                var soundChange = Mathf.Log(magnitude)/soundDivider;

                //print(soundChange);
                
                Debug.DrawLine(collision.contacts[0].point, collision.contacts[0].point + Vector3.up*soundChange, soundChange > 0.7f?Color.red : Color.green,0.5f);
                effect.GetComponentInChildren<AudioSource>().volume *= Mathf.Clamp(soundChange, 0.5f, 1f);
            }
        }
    }
}
