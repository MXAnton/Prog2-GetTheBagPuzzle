using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItemController : MonoBehaviour
{
    private BoxCollider col;
    private Rigidbody rb;
    
    [SerializeField]
    private GameObject usedByParent;

    private void Start() {
        col = gameObject.GetComponent<BoxCollider>();
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public void GetPickedUp(GameObject _newParent) {
        usedByParent = _newParent;

        transform.parent = usedByParent.transform;
        rb.isKinematic = true;
        col.enabled = false;
        transform.position = usedByParent.transform.position;
        transform.rotation = usedByParent.transform.rotation;
    }

    public void GetThrown(Vector3 _throwForce) {
        usedByParent = null;

        col.enabled = true;
        transform.parent = null;
        rb.isKinematic = false;

        rb.AddForce(_throwForce);
    }
}
