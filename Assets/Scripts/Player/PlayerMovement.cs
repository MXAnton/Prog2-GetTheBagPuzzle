using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Reference scripts")]
    [SerializeField]
    private PlayerCamera playerCamera;

    private Rigidbody rb;
    
    [Space]
    [SerializeField]
    private Transform body;
    [SerializeField]
    private CapsuleCollider groundFrictionCol;
    [SerializeField]
    private float groundFrictionColOffset = 0.35f;

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
        playerCamera.MoveCamToTarget();
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
        float _maxPlayerHeight = getHeightAbove() + body.localScale.y * 2;
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
        Vector3 _rayStartPos = body.position;
        _rayStartPos.y += body.localScale.y * 2; // Start on top of player head
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
        Vector3 _newSize = new Vector3(body.localScale.x, 1, body.localScale.z); // 1 default height
        Vector3 _newGroundFrictionColOffset = new Vector3(0, groundFrictionColOffset, 0); // 0.35f default height

        if (isCrouching) {
            _newSize.y = crouchHeight;
            _newGroundFrictionColOffset.y /= crouchHeight;
        }
        if (isLayingDown) {
            _newSize.y = layDownHeight;
            _newGroundFrictionColOffset.y /= layDownHeight;
        }

        body.localScale = _newSize;
        groundFrictionCol.center = _newGroundFrictionColOffset;
        
        playerCamera.MoveCamToTarget();
    }
}
