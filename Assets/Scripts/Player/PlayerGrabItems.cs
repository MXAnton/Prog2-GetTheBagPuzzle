using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabItems : MonoBehaviour
{
    [SerializeField]
    private Transform grabbedItemTarget;

    // [SerializeField]
    public GameObject currentGrabbedItem;
    [SerializeField]
    private float maxGrabItemRange = 5f;

    [SerializeField]
    private float grabItemRange = 2f;
    // [SerializeField]
    public GameObject grabbableItemInRange;
    public bool isGrabbableItemLocked = false;
    public bool lockInReach = false;

    [SerializeField]
    private float grabForce = 20f;

    private void FixedUpdate() {
        if (currentGrabbedItem) {
            Vector3 _startPos = currentGrabbedItem.transform.position;
            Vector3 _targetPosition = grabbedItemTarget.position;

            Vector3 _itemMoveDir = (_targetPosition - _startPos).normalized;

            float _distance = Vector3.Distance(_targetPosition, _startPos);

            Vector3 _newGrabbedItemVelocity = _itemMoveDir * grabForce * _distance;
            _newGrabbedItemVelocity /= currentGrabbedItem.GetComponent<Rigidbody>().mass; // Heavier object harder to move
            currentGrabbedItem.GetComponent<Rigidbody>().velocity = _newGrabbedItemVelocity;
        }
    }

    private void Update() {
        RayForGrabbableItem();

        if (Input.GetMouseButton(0)) {
            if (!currentGrabbedItem) {
                // No item grabbed, grab new item
                currentGrabbedItem = grabbableItemInRange;
            } else if (Vector3.Distance(currentGrabbedItem.transform.position, transform.position) > maxGrabItemRange) {
                // Drop if item if to far away
                currentGrabbedItem = null;
            }
        } else {
            currentGrabbedItem = null;
        }
    }


    private void RayForGrabbableItem() {
        bool _newIsGrabbableItemLocked = false;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        // Cast a ray from player center of screen forward.
        if (Physics.Raycast(ray, out hit, grabItemRange))
        {
            if (hit.transform.tag == "Lock") {
                lockInReach = true;
            } else {
                lockInReach = false;
            }

            if (hit.transform.tag == "Grabbable" 
                    || hit.transform.tag == "UsableItem"
                    || hit.transform.tag == "Door") {
                // Found usable item, return it
                grabbableItemInRange = hit.transform.gameObject;

                if (hit.transform.tag == "Door") {
                    // Check if locked
                    if (!hit.transform.parent.gameObject.GetComponent<DoorController>().openable) {
                        _newIsGrabbableItemLocked = true;
                    }
                }

                isGrabbableItemLocked = _newIsGrabbableItemLocked;
                return;
            }
        }

        isGrabbableItemLocked = _newIsGrabbableItemLocked;
        grabbableItemInRange = null;
    }
}
