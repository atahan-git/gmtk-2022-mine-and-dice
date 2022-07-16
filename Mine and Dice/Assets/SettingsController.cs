using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SettingsController : MonoBehaviour {
    public Transform settingsParent;

    public GameObject settingsMenu;
    
    private void Start() {
        var initRequiredSettings = settingsParent.GetComponentsInChildren<IInitRequired>();
        for (int i = 0; i < initRequiredSettings.Length; i++) {
            initRequiredSettings[i].Initialize();
        }
    }

    public void ToggleSettingsMenu() {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }


}

public interface IInitRequired {
    public void Initialize();
}