using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieScript : MonoBehaviour {
    public bool isPlayerDie = false;

    public float maxAngularVelocity = 25;

    [HideInInspector] public Rigidbody rg;
    [HideInInspector] public Collider cl;

    float forwardDot;
    float rightDot;
    float upDot;

    public bool isRolling;
    [SerializeField] private float rollingCutoff = 0.1f;

    public bool isUpright;
    [SerializeField] private float uprightSensitivity = 0.9f;
    float alignment;

    public bool affectedByPhysics = true;

    public float moveSpeed = 4f;

    private float diceSize;

    private void Awake() {
        rg = GetComponent<Rigidbody>();
        rg.maxAngularVelocity = maxAngularVelocity;
        cl = GetComponent<Collider>();
        diceSize = GetComponent<BoxCollider>().size.x * transform.localScale.x;
    }


    void Update() {
        var rollDetection = (rg.velocity.magnitude + rg.angularVelocity.magnitude) > rollingCutoff;

        if (rollDetection)
            curRollDetectionCooldown = rollDetectionCooldown;
        else
            curRollDetectionCooldown -= Time.deltaTime;

        isRolling = curRollDetectionCooldown > 0;

        alignment = Mathf.Max(Mathf.Abs(forwardDot), Mathf.Abs(rightDot), Mathf.Abs(upDot));
        isUpright = alignment > uprightSensitivity;

        if (!isMoving && isInBox) {
            transform.position = GetDieBoxSnapPosition();
        }
    }

    public void TogglePhysics(bool state) {
        affectedByPhysics = state;
        rg.isKinematic = !state;
        cl.enabled = state;
    }

    private bool isInBox;
    public Transform myDieBox;
    public Vector3 GetDieBoxSnapPosition() {
        return myDieBox.position + myDieBox.forward*(diceSize/2f);
    }

    class PositionWrapper{
        private readonly bool isDieBox;
        private readonly Transform target;
        private readonly float diceSize;
        public PositionWrapper(Transform _target, bool _isDieBox, float _diceSize) {
            target = _target;
            isDieBox = _isDieBox;
            diceSize = _diceSize;
        }

        public Vector3 GetPosition() {
            if (isDieBox) {
                return target.position + target.forward*(diceSize/2f);
            } else {
                return target.position;
            }
        }
    }
    
    public bool canDiceRoll {
        get { return !(isInBox || isMoving); }
    }

    public float curRollDetectionCooldown = 0;
    public float rollDetectionCooldown = 0.2f;

    public bool isMoving = false;

    public Transform GetEmptyFieldPosition() {
        //return insideBox.ClosestPoint(transform.position);
        return DiceMaster.s.GetEmptyFieldPosition();
    }

    
    private const float sensitivity = 0.5f;
    /*public DieFace GetActiveSide(Die myDie) {
        return myDie.faces[GetActiveSideIndex()];
    }*/

    public int GetActiveSideIndex() {
        var diceValue = -1;
        var forward = DiceMaster.s.forward;

        var rotation = transform.rotation;
			
        var forwardDot = Vector3.Dot( rotation * Vector3.forward, forward);
        var rightDot = Vector3.Dot(rotation * Vector3.right, forward);
        var upDot = Vector3.Dot(rotation * Vector3.up, forward);

        if (Mathf.Abs(forwardDot) > sensitivity) {
            diceValue = forwardDot > 0 ? 1 : 3; // 5 : 2

        } else if (Mathf.Abs(rightDot) > sensitivity) {
            diceValue = rightDot > 0 ? 0 : 2; // 0 : 1

        } else if (Mathf.Abs(upDot) > sensitivity) {
            diceValue = upDot > 0 ? 5 : 4; // 4 : 3

        } else {
            diceValue = -1;
        }
        return diceValue;
    }


    private readonly Quaternion[] alignedRotations = new[] {
        Quaternion.Euler(0, -90, 0),
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(0, 90, 0),
        Quaternion.Euler(0, 180, 0),
        Quaternion.Euler(-90, 0, 0),
        Quaternion.Euler(90, 0, 0)
    };

    public void AlignRotation() {
        StartCoroutine(AlightRotation());
    }
    
    IEnumerator AlightRotation() {
        yield return null;
        Quaternion rot = alignedRotations[GetActiveSideIndex()];
        var time = 0.2f;
        float rotSpeed = Quaternion.Angle(transform.rotation, rot)/time;


        while (time > 0) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotSpeed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }

        transform.rotation = rot;
    }

    void OnlyShowActiveFace(bool isOn) {
        /*if (isOn) {
            var faces = myFaces.GetFaces();
            for (int i = 0; i < faces.Length; i++) {
                faces[i].gameObject.SetActive(false);
            }

            faces[GetActiveSideIndex()].gameObject.SetActive(true);
        } else {
            var faces = myFaces.GetFaces();
            for (int i = 0; i < faces.Length; i++) {
                faces[i].gameObject.SetActive(true);
            }
        }*/
    }
}
