using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarController : MonoBehaviour
{
    private UsableItemController usableItemController;

    [SerializeField]
    private float damage = 20;

    private void Start() {
        usableItemController = GetComponent<UsableItemController>();
    }

    private void Update() {
        if (usableItemController.usedByParent) {
            // Being used by player
            if (Input.GetMouseButtonDown(0)) {
                Attack();
            }
        }
    }

    private void Attack() {
        // Debug.Log("Crowbar attack");
        usableItemController.animator.SetTrigger("attack");
    }


    public void Hit(GameObject _hit) {
        if (_hit.tag == "Lock") {
            _hit.GetComponent<LockController>().BreakLock();
        }
    }
}
