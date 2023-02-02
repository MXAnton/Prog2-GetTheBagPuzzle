using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour
{
    [SerializeField]
    private GameObject usedItem;

    [SerializeField]
    private float useItemRange = 2.5f;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (!usedItem) {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                RaycastHit hit;
                // Cast a ray from player center of screen forward.
                if (Physics.Raycast(ray, out hit, useItemRange))
                {
                    Debug.DrawRay(transform.position, transform.forward * useItemRange, Color.red, 1);
                    if (hit.transform.tag == "UsableItem") {
                        // Found usable item, grab it
                        usedItem = hit.transform.gameObject;
                        usedItem.GetComponent<UsableItemController>().GetPickedUp(gameObject);
                    }
                }
            } else {
                Debug.Log("Player already have item used.");
            }
        }
    }
}
