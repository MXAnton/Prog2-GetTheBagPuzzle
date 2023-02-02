using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField]
    private GameObject usedItem;

    [SerializeField]
    private float useItemRange = 2.5f;

    [SerializeField]
    private float throwItemForce = 600f;
    [SerializeField]
    private float throwItemUpwardForce = 1f;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (!usedItem) {
                TryPickupItem();
            } else {
                Debug.Log("Player already have item used.");
            }
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            if (usedItem) {
                // Throw item
                ThrowItem();
            }
        }
    }

    private void TryPickupItem() {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        // Cast a ray from player center of screen forward.
        if (Physics.Raycast(ray, out hit, useItemRange))
        {
            if (hit.transform.tag == "UsableItem") {
                // Found usable item, grab it
                PickupItem(hit.transform.gameObject);
            }

            // Debug.DrawRay(transform.position, transform.forward * useItemRange, Color.red, 1);
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
