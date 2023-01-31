using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Start() {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag != "Player") {
            playerMovement.isGrounded = true;
        }
    }
    private void OnTriggerStay(Collider other) {
        if (other.gameObject.tag != "Player") {
            playerMovement.isGrounded = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag != "Player") {
            playerMovement.isGrounded = false;
        }
    }
}
