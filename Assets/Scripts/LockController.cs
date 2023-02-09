using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerScript requires the GameObject to have a Animator component
[RequireComponent(typeof(Animator))]
public class LockController : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private bool isLocked = true;

    private void Start() {
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
}
