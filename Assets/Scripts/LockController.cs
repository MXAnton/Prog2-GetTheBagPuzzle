using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerScript requires the GameObject to have a Animator component
[RequireComponent(typeof(Animator))]
public class LockController : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private Animator animator;

    [SerializeField]
    private bool isLocked = true;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        animator.SetBool("locked", isLocked);
    }


    public bool GetLockState() {
        return isLocked;
    }


    public void ToggleLockState() {
        SetLockState(!isLocked);
    }

    public void SetLockState(bool _isLocked) {
        if (_isLocked) {
            Lock();
        } else {
            UnLock();
        }
    }

    public void Lock() {
        isLocked = true;

        // show lock locked graphical with anim etc.
        animator.SetBool("locked", isLocked);
    }
    public void UnLock() {
        isLocked = false;

        // show lock unlocked graphical with anim etc.
        animator.SetBool("locked", isLocked);
    }

    public void BreakLock() {
        UnLock();

        col.isTrigger = false;
        rb.isKinematic = false;
        this.enabled = false;

        gameObject.tag = "Grabbable";
    }
}
