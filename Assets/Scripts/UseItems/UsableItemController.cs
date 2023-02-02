using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItemController : MonoBehaviour
{
    private BoxCollider collider;
    private Rigidbody rb;
    
    [SerializeField]
    private GameObject usedByParent;

    private void Start() {
        collider = gameObject.GetComponent<BoxCollider>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void GetPickedUp(GameObject _newParent) {
        usedByParent = _newParent;

        transform.parent = usedByParent.transform;
        rb.isKinematic = true;
        collider.enabled = false;
        transform.position = usedByParent.transform.position;
        transform.rotation = usedByParent.transform.rotation;
    }
}
