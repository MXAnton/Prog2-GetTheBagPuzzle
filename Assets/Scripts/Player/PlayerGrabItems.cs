using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabItems : MonoBehaviour
{
    [SerializeField]
    private Transform grabbedItemTarget;

    public GameObject grabbableItemInRange;
    public GameObject currentGrabbedItem;

    [SerializeField]
    private float dropGrabbedItemRange = 5f;

    [SerializeField]
    private float grabForce = 20f;

    private void FixedUpdate() {
        if (currentGrabbedItem) {
            MoveGrabbedItemTowardsTarget();
        }
    }

    private void Update() {
        if (Input.GetMouseButton(1)) {
            if (!currentGrabbedItem) {
                // No item grabbed, grab new item
                currentGrabbedItem = grabbableItemInRange;
            } else if (Vector3.Distance(currentGrabbedItem.transform.position, transform.position) > dropGrabbedItemRange) {
                // Drop if item if to far away
                currentGrabbedItem = null;
            }
        } else {
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
