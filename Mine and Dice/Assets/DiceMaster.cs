using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DiceMaster : MonoBehaviour {
    public static DiceMaster s;

    private void Awake() {
        s = this;
    }

    public List<DieScript> allDice = new List<DieScript>();

    public GameObject diceRollArea;
    public Vector3 gravityDirection;
    public float gravityStrength;
    public Vector2 jumpStrength;
    public Vector2 midPushStrength;
    public Vector2 randomPushStrength;
    public Vector2 torqueStrength;

    public Vector3 forward {
        get { return diceRollArea.transform.forward; }
    }

    public Vector3 up {
        get { return diceRollArea.transform.up; }
    }

    public bool isRolling = false;

    //public Collider insideBox;

    public FieldDiceSlot[] fieldDiceSlots;

    private void Start() {
        //allDice = new List<DieScript>(GameObject.FindObjectsOfType<DieScript>());
    }


    void FixedUpdate() {
        for (int i = 0; i < allDice.Count; i++) {
            if (allDice[i].affectedByPhysics) {
                allDice[i].rg.AddForce(diceRollArea.transform.TransformDirection(gravityDirection) * gravityStrength * Time.fixedDeltaTime);
            }
        }
    }

    private void Update() {
        isRolling = false;
        for (int i = 0; i < allDice.Count; i++) {
            isRolling = isRolling || allDice[i].isRolling;
        }

        if (isRolling) {
            curRollTime += Time.deltaTime;

            if (curRollTime > rollTimeout) {
                isRolling = false;
            }
        }

        if (!isRolling) {
            for (int i = 0; i < allDice.Count; i++) {
                allDice[i].LockRotation(true);
            }
        }
    }

    public int dicePosScroll = 0;

    public Transform GetEmptyFieldPosition() {
        for (int i = 0; i < fieldDiceSlots.Length; i++) {
            var real_i = (i + dicePosScroll) % fieldDiceSlots.Length;
            if (!fieldDiceSlots[real_i].CheckIfDiceInside()) {
                dicePosScroll = real_i + 1;
                return fieldDiceSlots[real_i].transform;
            }
        }


        return fieldDiceSlots[0].transform;
    }


    [SerializeField] private float rollTimeout = 2f;
    [SerializeField] private float curRollTime = 0;

    public RandomAudioClipPlayer diceRollSounds;

    public void RollPossibleDice() {
        for (int i = 0; i < allDice.Count; i++) {
            var die = allDice[i];
            if (die.canDiceRoll) {
                die.LockRotation(false);
                var randomJumpForce = diceRollArea.transform.TransformDirection(-gravityDirection) * Random.Range(jumpStrength.x, jumpStrength.y);
                die.rg.AddForce(randomJumpForce);
                var midPush = (diceRollArea.transform.position - die.transform.position);
                midPush.z = 0;
                //midPush = midPush.normalized;
                midPush *= Random.Range(midPushStrength.x, midPushStrength.y);
                die.rg.AddForce(midPush);

                var randomPush = Random.insideUnitCircle.normalized * Random.Range(randomPushStrength.x, randomPushStrength.y);
                die.rg.AddForce(randomPush);

                //var randomTorque = Random.onUnitSphere * Random.Range(torqueStrength.x, torqueStrength.y);
                var randomTorque = Random.insideUnitCircle.normalized * Random.Range(torqueStrength.x, torqueStrength.y);
                die.rg.AddTorque(new Vector3(randomTorque.x, randomTorque.y, 0), ForceMode.Impulse);

                die.curRollDetectionCooldown = die.rollDetectionCooldown;
                die.isRolling = true;
            }

        }

        diceRollSounds.PlayRandomClip();
        curRollTime = 0;
        isRolling = true;
    }

    public bool CanAllDiceRoll() {
        for (int i = 0; i < allDice.Count; i++) {
            var die = allDice[i];
            if (!die.canDiceRoll) {
                return false;
            }

        }

        return !isRolling;
    }

    public void PutDiceInTheRollField() {
        for (int i = 0; i < allDice.Count; i++) {
            allDice[i].SnapDieIntoField();

        }

        curRollTime = 0;
    }

    public void PutDiceBackToModules() {
        for (int i = 0; i < allDice.Count; i++) {
            //allDice[i].PutDieInDieBox();

        }
    }
}


public enum DieAffinity {
    mining, chopping, attacking
}


public enum ItemType {
    die, ingredient, tool
}
