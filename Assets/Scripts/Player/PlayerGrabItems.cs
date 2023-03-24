using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabItems : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Transform grabbedItemTarget;

    public GameObject grabbableItemInRange;
    public GameObject currentGrabbedItem;

    [SerializeField]
    private float dropGrabbedItemRange = 5f;

    [SerializeField]
    private float grabForce = 20f;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip lockedObjectSound;
    [SerializeField]
    private AudioClip grabItemSound;
    [SerializeField]
    private AudioClip dropObjectSound;

    private void FixedUpdate() {
        if (currentGrabbedItem) {
            MoveGrabbedItemTowardsTarget();
        }
    }

    private void Update() {
        if (Input.GetMouseButton(1)) {
            if (currentGrabbedItem == null) {
                // No item grabbed, grab new item
                if (grabbableItemInRange != null) {
                    currentGrabbedItem = grabbableItemInRange;
                    if (playerController.lockedObjectInReach) {
                        playerController.audioSource.PlayOneShot(lockedObjectSound);
                    } else {
                        playerController.audioSource.PlayOneShot(grabItemSound);
                    }
                }
            } else if (Vector3.Distance(currentGrabbedItem.transform.position, transform.position) > dropGrabbedItemRange) {
                // Drop if item if to far away
                playerController.audioSource.PlayOneShot(dropObjectSound);
                currentGrabbedItem = null;
            }
        } else if (currentGrabbedItem != null) {
            // Drop item
            playerController.audioSource.PlayOneShot(dropObjectSound);
            currentGrabbedItem = null;
        }
    }

    private void MoveGrabbedItemTowardsTarget() {
        Vector3 _startPos = currentGrabbedItem.transform.position;
        Vector3 _targetPosition = grabbedItemTarget.position;

        Vector3 _itemMoveDir = (_targetPosition - _startPos).normalized;

        float _distance = Vector3.Distance(_targetPosition, _startPos);

        Vector3 _newGrabbedItemVelocity = _itemMoveDir * grabForce * _distance;
        _newGrabbedItemVelocity /= currentGrabbedItem.GetComponent<Rigidbody>().mass; // Heavier object harder to move
        currentGrabbedItem.GetComponent<Rigidbody>().velocity = _newGrabbedItemVelocity;
    }
}
