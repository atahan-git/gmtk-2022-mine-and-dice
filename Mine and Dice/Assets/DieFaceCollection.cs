using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieFaceCollection : MonoBehaviour {[SerializeField]
    private DieFaceDisplay[] faces;

    private Die myDie;
    
    public void ApplyFaces(Die die) {
        myDie = die;
        for (int i = 0; i < faces.Length; i++) {
            faces[i].SetDieFace(die, die.faces[i]);
        }
    }

    public void ShowActiveFace(int index) {
        ResetActiveFace();
        faces[index].SetHighlightStatus(true);
    }

    public void ResetActiveFace() {
        for (int i = 0; i < faces.Length; i++) {
            faces[i].SetHighlightStatus(false);
        }
    }

    public Die GetActiveDie() {
        return myDie;
    }

    public void SetDieCanBeActivatedStatus(bool status) {
        for (int i = 0; i < faces.Length; i++) {
            faces[i].SetCanBeActivated(status);
        }
    }

    public DieFaceDisplay[] GetFaces() {
        return faces;
    }
}
