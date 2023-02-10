using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowbarController : MonoBehaviour
{
    private UsableItemController usableItemController;

    [SerializeField]
    private float damage = 20;
    [SerializeField]
    private float attackDuration = 1f;

    private void Start() {
        usableItemController = GetComponent<UsableItemController>();
    }

    private void Update() {
        if (usableItemController.usedByParent) {
            // Being used by player
            if (!usableItemController.isAction) {
                if (Input.GetMouseButtonDown(0)) {
                    Attack();
                }
            }
        }
    }

    private void Attack() {
        // Debug.Log("Crowbar attack");
        usableItemController.animator.SetTrigger("attack");
        usableItemController.isAction = true;
        StopAllCoroutines();
        StartCoroutine(IsActionFalseIn(attackDuration));
    }


    public void Hit(GameObject _hit) {
        if (_hit.tag == "Lock") {
            _hit.GetComponent<LockController>().BreakLock();
        }
    }

    private IEnumerator IsActionFalseIn(float _seconds) {
        yield return new WaitForSeconds(_seconds);
        usableItemController.isAction = false;
    }
}
