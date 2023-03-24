using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player scripts")]
    [SerializeField]
    private PlayerItemController playerItemController;
    [SerializeField]
    private PlayerGrabItems playerGrabItems;

    [Header("Components")]
    public AudioSource audioSource;

    [Header("General player vars")]
    [SerializeField]
    private float reach = 2f;
    [SerializeField]
    private LayerMask interactLayerMask;
    [Space]
    // public bool lockInReach = false;
    public GameObject lockInRange;
    public bool lockedObjectInReach = false;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        RayForward();
    }

    private void RayForward() {
        bool _newLockedObjectInReach = false;
        // bool _newLockInReach = false;
        GameObject _newLockInRange = null;

        GameObject _newUsableItemInRange = null;
        GameObject _newGrabbableItemInRange = null;
        
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        // Cast a ray from player center of screen forward.
        if (Physics.Raycast(ray, out hit, reach, interactLayerMask))
        {          
            switch (hit.transform.tag)
            {
                case "UsableItem":
                    // Found usable item, return it
                    _newUsableItemInRange = hit.transform.gameObject;
                    // Found grabbable item, return it
                    _newGrabbableItemInRange = hit.transform.gameObject;
                    break;
                case "Grabbable":
                    // Found grabbable item, return it
                    _newGrabbableItemInRange = hit.transform.gameObject;
                    break;
                case "Door":
                    // Found grabbable item, return it
                    _newGrabbableItemInRange = hit.transform.gameObject;
                    // Check if locked
                    if (!hit.transform.parent.gameObject.GetComponent<DoorController>().openable) {
                        _newLockedObjectInReach = true;
                    }
                    break;
                case "Lock":
                    // if (Input.GetKeyDown(KeyCode.E)) {            
                    //     // Found lock item, lock/unlock
                    //     hit.transform.gameObject.GetComponent<LockController>().ToggleLockState();
                    // }
                    _newLockInRange = hit.transform.gameObject;
                    break;
                default:
                    // Debug.Log("Hit: " + hit.transform);
                    break;
            }
        }

        lockedObjectInReach = _newLockedObjectInReach;
        // lockInReach = _newLockInReach;

        lockInRange = _newLockInRange;

        playerItemController.usableItemInRange = _newUsableItemInRange;
        playerGrabItems.grabbableItemInRange = _newGrabbableItemInRange;
    }
}
