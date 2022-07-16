using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class RandomAudioClipPlayer : MonoBehaviour
{
    
    
    [SerializeField]
    private float pitchVariance = 0.1f;
    [SerializeField]
    private AudioClip[] sounds;

    [SerializeField]
    private bool playAtStart = true;

    void Start() {
        PlayRandomClip();
    }

    public void PlayRandomClip() {
        var source = GetComponent<AudioSource>();
        source.Stop();
        source.clip = sounds[Random.Range(0, sounds.Length)];
        source.pitch *= Random.Range(1 - pitchVariance, 1 + pitchVariance);
        source.Play();
    }
}
