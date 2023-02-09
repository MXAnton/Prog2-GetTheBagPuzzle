using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBool : MonoBehaviour
{
    public CrowbarController crowbarController;

    private void OnTriggerEnter(Collider _col) {
        crowbarController.Hit(_col.gameObject);
    }
}
