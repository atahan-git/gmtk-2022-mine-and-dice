using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieScript : MonoBehaviour {
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

    private float diceSize;

    public DieFaceCollection myFaces;

    public Die myDie;
    
    private void Awake() {
        rg = GetComponent<Rigidbody>();
        rg.maxAngularVelocity = maxAngularVelocity;
        cl = GetComponent<Collider>();
        diceSize = GetComponent<BoxCollider>().size.x * transform.localScale.x;
        myFaces.ApplyFaces(myDie);
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

    }

    public void TogglePhysics(bool state) {
        affectedByPhysics = state;
        rg.isKinematic = !state;
        cl.enabled = state;
    }

    private bool isInBox => GetComponent<ItemCarrier>().isSnapping;
    
    public bool canDiceRoll {
        get { return !(isInBox || isMoving || isRolling || isDieLockedInSlot); }
    }

    public float curRollDetectionCooldown = 0;
    public float rollDetectionCooldown = 0.2f;

    public float moveSpeed = 20f;

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

    public DieFace GetActiveSide() {
        return myDie.faces[GetActiveSideIndex()];
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

    public bool canDieStillBeActivated = true;
    public void SetDieCanBeActivatedStatus(bool canBeActivated) {
        canDieStillBeActivated = canBeActivated;
        myFaces.SetDieCanBeActivatedStatus(canDieStillBeActivated);
    }

    public bool isDieLockedInSlot => GetComponent<ItemCarrier>().isLockedInSlot;
    
    private bool isRotationLocked = false;
    public void LockRotation(bool _isLocked) {
        
        if(isRotationLocked == _isLocked)
            return;

        isRotationLocked = _isLocked;
        
        RigidbodyConstraints constraints;
        if (isRotationLocked) {
            constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        } else {
            constraints = RigidbodyConstraints.None;
        }

        rg.constraints = constraints;
    }
    
    public void SnapDieIntoField() {
        isMoving = true;
        StartCoroutine(MoveToPosition(new PositionWrapper(GetEmptyFieldPosition(), false, diceSize), true));
    }
    
    IEnumerator MoveToPosition(PositionWrapper pos, bool enablePhysicsAfter) {
        OnlyShowActiveFace(false);
        isMoving = true;
        TogglePhysics(false);

        yield return null;
        float time = Vector3.Distance(transform.position, pos.GetPosition()) / moveSpeed;
        Quaternion rot = alignedRotations[GetActiveSideIndex()];
        float rotSpeed = Quaternion.Angle(transform.rotation, rot)/time;


        while (time > 0) {
            transform.position = Vector3.MoveTowards(transform.position, pos.GetPosition(), moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, rotSpeed * Time.deltaTime);
            time -= Time.deltaTime;
            yield return null;
        }

        transform.position = pos.GetPosition();
        transform.rotation = rot;


        if (enablePhysicsAfter) {
            TogglePhysics(true);
        } else {
            OnlyShowActiveFace(true);
        }
        isMoving = false;
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
    
    public void OnlyShowActiveFace(bool isOn) {
        if (isOn) {
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
        }
    }
}


[System.Serializable]
public class Die {
    public Color color = Color.white;
    //public Sprite emptyFace;
    public DieFace[] faces = new DieFace[6];
    public DieAffinity myAffinity;
    public Sprite gfx;
}

[System.Serializable]
public class DieFace{
    //public bool isFilled = true;
    [SerializeField]
    private int pips = 1;
    //public Sprite gfx;
    
    public int clampedPips {
        get {
            return Mathf.Clamp(pips, 0, 9);
        }
        set {
            pips = value;
        }
    }

}
