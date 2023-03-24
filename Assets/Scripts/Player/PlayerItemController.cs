using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    public PlayerController playerController;

    [Space]
    public GameObject usableItemInRange;
    public GameObject usedItem;

    [SerializeField]
    private float throwItemForce = 600f;
    [SerializeField]
    private float throwItemUpwardForce = 1f;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip pickupItemSound;
    [SerializeField]
    private AudioClip throwItemSound;
    

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
        // Play pickup sound
        playerController.audioSource.PlayOneShot(pickupItemSound);

        // pickup item
        usedItem = _objectToPickup;
        usedItem.GetComponent<UsableItemController>().GetPickedUp(gameObject.transform);
    }

    private void ThrowItem() {
        // Play drop sound
        playerController.audioSource.PlayOneShot(throwItemSound);

        // drop item
        Vector3 _throwForce = transform.forward;
        _throwForce.y += throwItemUpwardForce; // Make item thrown little upwards
        _throwForce *= throwItemForce; // Add force multiplier

        usedItem.GetComponent<UsableItemController>().GetThrown(_throwForce);
        usedItem = null;
    }
}
