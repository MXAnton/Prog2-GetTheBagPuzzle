using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    public GameObject usableItemInRange;
    public GameObject usedItem;

    [SerializeField]
    private float throwItemForce = 600f;
    [SerializeField]
    private float throwItemUpwardForce = 1f;

    private void Update() {
        if (usedItem) {
            if (usedItem.GetComponent<UsableItemController>().isAction) {
                // Disable item pickup etc when attacking etc.
                usableItemInRange = null;
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && usableItemInRange) {
            if (usedItem) {
                // Player already have item used, throw old first and then pickup new
                ThrowItem();
            }
            PickupItem(usableItemInRange);
        }

        if (Input.GetKeyDown(KeyCode.G) && usedItem) {
            // Throw item
            ThrowItem();
        }
    }

    private void PickupItem(GameObject _objectToPickup) {
        usedItem = _objectToPickup;
        usedItem.GetComponent<UsableItemController>().GetPickedUp(gameObject);
    }

    private void ThrowItem() {
        Vector3 _throwForce = transform.forward;
        _throwForce.y += throwItemUpwardForce; // Make item thrown little upwards
        _throwForce *= throwItemForce; // Add force multiplier

        usedItem.GetComponent<UsableItemController>().GetThrown(_throwForce);
        usedItem = null;
    }
}
