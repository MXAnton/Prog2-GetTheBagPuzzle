using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    private HingeJoint doorHingeJoint;

    [SerializeField]
    private LockController lockController;

    [SerializeField]
    private bool openable = true;

    // Hingejoint limit vars
    private float minLimit = -100;
    private float maxLimit = 100;
    private float limitLerpTime;
    [SerializeField]
    private float limitLerpDurationMultiplier = 1f;

    private void Start() {
        minLimit = doorHingeJoint.limits.min;
        maxLimit = doorHingeJoint.limits.max;
    }

    private void FixedUpdate() {
        if (lockController == null) {
            // No lock to controll door, stay openable
            if (!openable) {
                UnLock();
            }
            return;
        }

        if (lockController.GetLockState()) {
            // Lock locked == door !openable
            if (openable) {
                // Set openable == !openable
                Lock();
            }

            if (limitLerpTime <= 1) {
                LerpLimits();
            }
        } else {
            // Lock unlocked == door openable
            if (!openable) {
                // Set !openable == openable
                UnLock();
            }
        }
    }

    private void Lock() {
        openable = false;

        // Snap position to lockpos and rot
        limitLerpTime = 0;
    }
    private void UnLock() {
        openable = true;
        
        // Remove spring forcing to shut
        JointLimits _limits = doorHingeJoint.limits;
        _limits.min = minLimit;
        _limits.max = maxLimit;
        _limits.bounciness = 0;

        doorHingeJoint.limits = _limits;
    }

    private void LerpLimits() {
        // Door lock, force it shut if not already
        JointLimits _limits = doorHingeJoint.limits;
        _limits.min = Mathf.Lerp(minLimit, 0, limitLerpTime);
        _limits.max = Mathf.Lerp(maxLimit, 0, limitLerpTime);

        // .. and increase the limitLerpTime interpolater
        limitLerpTime += limitLerpDurationMultiplier * Time.fixedDeltaTime;

        if (limitLerpTime > 1) {
            // Fully closed, set to 0 & 0
            _limits.min = 0;
            _limits.max = 0;
        }
        doorHingeJoint.limits = _limits;
    }
}
