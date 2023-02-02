using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform camTransform;

    [SerializeField]
    private float mouseSensivity = 2f;

    [SerializeField]
    private float minCameraXRot = -85;
    [SerializeField]
    private float maxCameraXRot = 75;

    private float xRotation = 0;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float _rotateHorizontal = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime * 100;
        float _rotateVertical = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime * 100;
        
        xRotation -= _rotateVertical;
        xRotation = Mathf.Clamp(xRotation, minCameraXRot, maxCameraXRot);

        camTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(transform.up * _rotateHorizontal);

        // camTransform.Rotate(Vector3.right * _rotateVertical);
    }
}
