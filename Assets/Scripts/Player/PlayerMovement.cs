using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float runMultiplier = 1.5f;

    [SerializeField]
    private Vector2 movementInput = new Vector2(0, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");
        movementInput = Vector2.ClampMagnitude(movementInput, 1);
        
        Move();
    }

    private void Move() {
        Vector3 _moveDir = (movementInput.y * transform.forward) + (movementInput.x * transform.right);
        _moveDir *= movementSpeed * Time.fixedDeltaTime;

        if (Input.GetKey(KeyCode.LeftShift)) {
            _moveDir *= runMultiplier;
        }

        _moveDir.y = rb.velocity.y; // Don't change gravity or jump force
        rb.velocity = _moveDir;
    }
}
