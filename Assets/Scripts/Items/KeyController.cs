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
        if (!usableItemController.usedByParent || usableItemController.isAction) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            PlayerController _playerController = usableItemController.usedByParent.GetComponent<PlayerItemController>().playerController;
            if (_playerController.lockInRange) {
                // Try unlock lock with key
                _playerController.lockInRange.GetComponent<LockController>().UnLock(keyId);
            }
        }
    }
}
