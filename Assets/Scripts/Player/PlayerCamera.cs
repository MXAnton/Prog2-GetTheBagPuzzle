using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform camTransform;

    [SerializeField]
    private float mouseSensivity = 1f;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        float _rotateHorizontal = Input.GetAxis("Mouse X");
        float _rotateVertical = Input.GetAxis("Mouse Y");
        
        transform.Rotate(-transform.up * _rotateHorizontal * mouseSensivity);
        camTransform.Rotate(Vector3.right * _rotateVertical * mouseSensivity);
    }
}
