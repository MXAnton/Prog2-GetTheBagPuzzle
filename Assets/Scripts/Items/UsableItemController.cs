using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItemController : MonoBehaviour
{
    [SerializeField]
    private BoxCollider[] cols;
    private Rigidbody rb;

    public Animator animator;
    
    public bool isAction = false;

    public Transform usedByParent;

    private float lerpTimeElapsed;
    private float lerpTransformDuration = 0.4f;
    private Vector3 startPos;
    private Vector3 startRot;

    private void Start() {
        // cols = gameObject.GetComponents<BoxCollider>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (usedByParent) {
            if (lerpTimeElapsed < lerpTransformDuration) {
                transform.localPosition = Vector3.Lerp(startPos, new Vector3(0,0,0), lerpTimeElapsed / lerpTransformDuration);
                transform.localEulerAngles = Vector3.Lerp(startRot, new Vector3(0,0,0), lerpTimeElapsed / lerpTransformDuration);

                lerpTimeElapsed += Time.fixedDeltaTime;
            } else {
                transform.localPosition = new Vector3(0,0,0);
                transform.localEulerAngles = new Vector3(0,0,0);
            }
        }
    }

    // private void Update() {
    //     if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
    //         isAction = false;
    //     }
    // }

    public void GetPickedUp(Transform _newParent) {
        usedByParent = _newParent;

        transform.parent = usedByParent;
        rb.isKinematic = true;
        foreach (BoxCollider _col in cols)
        {
            _col.enabled = false;
        }

        // transform.position = usedByParent.transform.position;
        // transform.rotation = usedByParent.transform.rotation;
        startPos = transform.localPosition;
        startRot = transform.localEulerAngles;

        lerpTimeElapsed = 0;
    }

    public void GetThrown(Vector3 _throwForce) {
        usedByParent = null;

        foreach (BoxCollider _col in cols)
        {
            _col.enabled = true;
        }
        transform.parent = null;
        rb.isKinematic = false;

        rb.AddForce(_throwForce);

        
        if (animator) {
            animator.SetTrigger("idle");
        }

        isAction = false;
    }
}
