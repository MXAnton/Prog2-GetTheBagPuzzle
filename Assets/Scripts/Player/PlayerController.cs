using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float reach = 2f;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            RayForward();
        }
    }

    private void RayForward() {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        // Cast a ray from player center of screen forward.
        if (Physics.Raycast(ray, out hit, reach))
        {
            if (hit.transform.tag == "Lock") {
                // Found lock item, lock/unlock
                hit.transform.gameObject.GetComponent<LockController>().ToggleLockState();
            }
        }
    }
}
