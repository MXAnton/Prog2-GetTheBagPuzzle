using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Player jump vars")]
    [SerializeField]
    private float jumpPower = 10f;
    [SerializeField]
    private bool playerJump = false;
    public bool isGrounded = false;

    [Header("Player movement speeds")]
    [SerializeField]
    private float movementSpeed = 90f;
    [SerializeField]
    private float runMultiplier = 1.8f;
    [SerializeField]
    private float crouchMovementSpeedMultiplier = 0.6f;
    [SerializeField]
    private float layDownMovementSpeedMultiplier = 0.3f;
    [Space]
    [SerializeField]
    private float inAirMovementMultiplier = 0.8f;

    [Header("Player states")]
    [SerializeField]
    private bool isCrouching = false;
    [SerializeField]
    private float crouchHeight = 0.6f;
    
    [SerializeField]
    private bool isLayingDown = false;
    [SerializeField]
    private float layDownHeight = 0.4f;

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

    private void Update() {
        GetPlayerCrouchLayingState();

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isGrounded) {
                playerJump = true;
            } else {
                playerJump = false;
            }
        }
    }

    private void Move() {
        Vector3 _newVelocity = (movementInput.y * transform.forward) + (movementInput.x * transform.right);
        _newVelocity *= movementSpeed * Time.fixedDeltaTime;

        float _jumpPower = jumpPower;

        if (Input.GetKey(KeyCode.LeftShift)) {
            _newVelocity *= runMultiplier;
        }
        if (isCrouching) {
            _newVelocity *= crouchMovementSpeedMultiplier;
            _jumpPower *= crouchMovementSpeedMultiplier; // Jump lower on crouch
        }
        if (isLayingDown) {
            _newVelocity *= layDownMovementSpeedMultiplier;
            _jumpPower *= layDownMovementSpeedMultiplier; // Jump even lower on laydown
        }

        if (!isGrounded) {
            // Add minimal movement when player in air
            _newVelocity *= inAirMovementMultiplier;
        }

        _newVelocity.y = rb.velocity.y; // Don't change gravity or jump force
        if (playerJump) {
            _newVelocity.y = _jumpPower; // Set y velocity to modified _jumpPower
        }

        rb.velocity = _newVelocity;

        playerJump = false;
    }

    private void GetPlayerCrouchLayingState() {
        // --- Crouch and lay down when pressed ---
        /* 
        bool _newIsCrouching = false; // Default false, change on correct input
        bool _newIsLayingDown = false; // Default false, change on correct input

        if (Input.GetKey(KeyCode.LeftControl)) {
            _newIsCrouching = true;
        }
        if (Input.GetKey(KeyCode.Z)) {
            _newIsCrouching = false; // Player isnt crouching if laying down
            _newIsLayingDown = true;
        }
        
        isCrouching = _newIsCrouching;
        isLayingDown = _newIsLayingDown;
        */
        
        // --- Crouch and lay down toggle ---
        bool _newIsCrouching = isCrouching; // Default to current value, change on correct input
        bool _newIsLayingDown = isLayingDown; // Default to current value, change on correct input

        // Toggle isCrouching and islayingdown on key down
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            _newIsLayingDown = false; // Player isnt laying down if crouching
            _newIsCrouching = !_newIsCrouching;
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            _newIsCrouching = false; // Player isnt crouching if laying down
            _newIsLayingDown = !_newIsLayingDown;
        }

        // Check if can stand up from crouch or lay down - if above height as much as needed
        float _maxPlayerHeight = getHeightAbove() + transform.localScale.y * 2;
        // Debug.Log("Height above: " + getHeightAbove() + " - Maxplayerheight: " + _maxPlayerHeight);
        if (_maxPlayerHeight <= crouchHeight * 2) {
            // Cant crouch, to little space above.
            _newIsCrouching = false;
            // Laydown instead
            _newIsLayingDown = true;

            // Debug.Log("Cant stand up or cruch, only lay down");
        } 
        else if (_maxPlayerHeight <= 1 * 2) { // 1 = default player size. Default player height: 1 * 2 = 2
            // Cant stand up, to little space above.
            // IF the player doesn't want to lay down
            //  start crouching to be as tall as possible
            if (!_newIsLayingDown) {
                _newIsCrouching = true;
                _newIsLayingDown = false;
                
                // Debug.Log("Cant stand up, cruch or lay down");
            }
        }
        
        isCrouching = _newIsCrouching;
        isLayingDown = _newIsLayingDown;

        SetPlayerHeight();
    }
    private float getHeightAbove() {
        float _heightAbove = Mathf.Infinity; // Default to unlimited height above player

        RaycastHit hit;
        Vector3 _rayStartPos = transform.position;
        _rayStartPos.y += transform.localScale.y * 2; // Start on top of player head
        Ray upRay = new Ray(_rayStartPos, Vector3.up);
        // Cast a ray straight upwards.
        if (Physics.Raycast(upRay, out hit))
        {
            // the height measured by the raycast distance.
            _heightAbove = hit.distance;
            // Debug.Log("Height above: " + _heightAbove);
            // Debug.DrawRay(_rayStartPos, Vector3.up * _heightAbove, Color.green, 0.1f);
        }

        return _heightAbove;
    }
    private void SetPlayerHeight() {
        Vector3 _newSize = new Vector3(transform.localScale.x, 1, transform.localScale.z); // 1 default height

        if (isCrouching) {
            _newSize.y = crouchHeight;
        }
        if (isLayingDown) {
            _newSize.y = layDownHeight;
        }

        transform.localScale = _newSize;
    }


    // Check if grounded with downwards raycast - old
    /*
    private bool CheckIfGroundedDownRay() {
        bool _isGrounded = false; // Default to false

        RaycastHit hit;
        Vector3 _rayStartPos = transform.position; // Starts on bottom of player
        _rayStartPos.y += 0.1f;
        Ray downRay = new Ray(_rayStartPos, -Vector3.up);

        // Cast a ray straight downwards.
        // If distance to ground less than 0.1f
        if (Physics.Raycast(downRay, out hit, 5))
        {
            if (hit.distance < 0.3f) {
                _isGrounded = true;
            }
            Debug.DrawRay(_rayStartPos, -Vector3.up * hit.distance, Color.blue, 0.1f);
        }

        return _isGrounded;
    }
    */
}
