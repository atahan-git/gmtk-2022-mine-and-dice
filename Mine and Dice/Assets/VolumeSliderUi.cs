using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliderUi : MonoBehaviour, IInitRequired {
    public AudioMixer mixer;
    public string exposedName;
    public Slider mySlider;
    
    public void Initialize() {
        var audVal = PlayerPrefs.GetFloat(exposedName, 1f);
        mySlider.value = audVal;
        SetVol(audVal);
    }

    public void OnSliderUpdated() {
        PlayerPrefs.SetFloat(exposedName, mySlider.value);
        SetVol(mySlider.value);
    }

    void SetVol(float sliderVal) {
        sliderVal = Mathf.Clamp(sliderVal, 0.001f, 1f);
        mixer.SetFloat(exposedName, Mathf.Log(sliderVal) * 20);
    }
}
