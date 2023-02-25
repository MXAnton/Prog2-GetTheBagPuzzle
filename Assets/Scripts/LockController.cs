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
    private int lockId = 1;
    [SerializeField]
    private bool isLocked = true;
    [SerializeField]
    private bool breakable = false;

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
    private void UnLock() {
        isLocked = false;

        // show lock unlocked graphical with anim etc.
        animator.SetBool("locked", isLocked);
    }
    public void UnLock(int _keyId) {
        if (_keyId != lockId) {
            Debug.Log("Wrong key id");
            return;
        }

        isLocked = false;

        // show lock unlocked graphical with anim etc.
        animator.SetBool("locked", isLocked);
    }

    public void BreakLock() {
        if (!breakable) {
            Debug.Log("Lock not breakable");
            return;
        }

        UnLock();

        col.isTrigger = false;
        rb.isKinematic = false;
        this.enabled = false;

        gameObject.tag = "Grabbable";
    }
}
