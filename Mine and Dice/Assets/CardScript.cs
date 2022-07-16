using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour {

    public Transform dieBox;
    public DieScript currentDie;

    public int pipRequirement;
    public DieAffinity myAffinity;

    public int curPips;
    public GameObject finishReward;
    public Transform rewardSpawnPos;

    public Slider progressSlider;
    public Image sliderBGImg;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
