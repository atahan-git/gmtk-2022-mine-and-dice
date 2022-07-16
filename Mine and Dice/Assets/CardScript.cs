using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour {

    private ItemBox itemBox;

    public int pipRequirement;
    public DieAffinity myAffinity;

    public int curPips;
    public Transform rewardSpawnPos;

    public Slider progressSlider;
    //public Image sliderBGImg;

    public ProductionCardRecipe myRecipe;
    
    // Start is called before the first frame update
    void Start() {
        itemBox = GetComponentInChildren<ItemBox>();
        itemBox.itemAdded.AddListener(ItemAdded);
    }

    void ItemAdded() {
        var die = itemBox.myItem.GetComponent<DieScript>();
        if (die != null) {
            var pips = die.GetActiveSide().clampedPips;
            itemBox.myItem.isLockedInSlot = true;
            if (myAffinity != DieAffinity.none) {
                if (die.myDie.myAffinity == myAffinity)
                    pips *= 2;
            }

            StartCoroutine(DoRecipe(pips));
        }
    }

    public float delay = 0.1f;
    public float newItemDelay = 0.2f;
    IEnumerator DoRecipe(int pips) {
        for (int i = 0; i < pips; i++) {
            curPips += pips;

            if (curPips >= pipRequirement) {
                AwardItem();
                UpdateSlider();
                curPips = 0;
                yield return new WaitForSeconds(newItemDelay);
            }
            
            yield return new WaitForSeconds(delay);
            UpdateSlider();
        }
        
        
        
        itemBox.myItem.isLockedInSlot = false;
        var die = itemBox.myItem.GetComponent<DieScript>();
        die.SetDieCanBeActivatedStatus(false);
        die.SnapDieIntoField();
    }

    void UpdateSlider() {
        progressSlider.value = curPips;
    }

    void AwardItem() {
        var item = myRecipe.GetRandomItem();
        Instantiate(item, rewardSpawnPos.position, rewardSpawnPos.rotation);
    }
}
