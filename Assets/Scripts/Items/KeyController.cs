using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private UsableItemController usableItemController;

    public int keyId = 1;

    private void Start() {
        usableItemController = GetComponent<UsableItemController>();
    }

    private void Update() {
        // if (usableItemController.usedByParent) {
        //     // Being used by player
        //     if (!usableItemController.isAction 
        //         && usableItemController.usedByParent.GetComponent<PlayerItemController>().playerGrabItems.lockInReach) {
        //         if (Input.GetMouseButtonDown(0)) {
        //             TryUnLock();
        //         }
        //     }
        // }
    }

    private void TryUnLock() {
        usableItemController.isAction = true;
    }
}
