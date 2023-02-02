using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Space]
    [SerializeField]
    private GameObject usableItemInRange = null;

    [Header("UI")]
    [SerializeField]
    private Image crosshair;

    private void Update() {
        RayForUsableItem();

        SetCrosshair();

        if (Input.GetKeyDown(KeyCode.E) && usableItemInRange) {
            if (!usedItem) {
                PickupItem(usableItemInRange);
            } else {
                // Player already have item used, throw old and pickup new
                ThrowItem();
                PickupItem(usableItemInRange);
            }
        }

        if (Input.GetKeyDown(KeyCode.G) && usedItem) {
            // Throw item
            ThrowItem();
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


    #region UI
    
    private void SetCrosshair() {
        if (usableItemInRange && !usedItem) {
            crosshair.color = Color.green;
        } else if (usableItemInRange) {
            crosshair.color = Color.red;
        } else {
            crosshair.color = Color.white;
        }
    }

    #endregion
}
