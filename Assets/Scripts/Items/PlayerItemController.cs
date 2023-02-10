using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    // [SerializeField]
    public GameObject usedItem;

    [SerializeField]
    private float useItemRange = 2f;

    [SerializeField]
    private float throwItemForce = 600f;
    [SerializeField]
    private float throwItemUpwardForce = 1f;

    [Space]
    // [SerializeField]
    public GameObject usableItemInRange = null;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G) && usedItem) {
            // Throw item
            ThrowItem();
        }

        if (usedItem) {
            if (usedItem.GetComponent<UsableItemController>().isAction) {
                // Disable item pickup etc when attacking etc.
                usableItemInRange = null;
                return;
            }
        }

        RayForUsableItem();

        if (Input.GetKeyDown(KeyCode.E) && usableItemInRange) {
            if (!usedItem) {
                PickupItem(usableItemInRange);
            } else {
                // Player already have item used, throw old and pickup new
                ThrowItem();
                PickupItem(usableItemInRange);
            }
        }
    }

    private void RayForUsableItem() {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        // Cast a ray from player center of screen forward.
        if (Physics.Raycast(ray, out hit, useItemRange))
        {
            if (hit.transform.tag == "UsableItem") {
                // Found usable item, return it
                usableItemInRange = hit.transform.gameObject;
                return;
            }
        }

        usableItemInRange = null;
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
