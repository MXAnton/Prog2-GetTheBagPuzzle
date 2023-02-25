using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnScreenUI : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField]
    private PlayerItemController playerItemController;
    [SerializeField]
    private PlayerGrabItems playerGrabItems;
    [SerializeField]
    private PlayerController playerController;

    [Header("UI")]
    [SerializeField]
    private Image crosshair;
    [Space]
    [SerializeField]
    private Sprite crosshairNormal;
    [SerializeField]
    private Sprite crosshairGrab;
    [SerializeField]
    private Sprite crosshairGrabbed;
    [SerializeField]
    private Sprite crosshairLocked;
    [SerializeField]
    private Sprite crosshairUnLock;

    private void Update() {
        SetCrosshair();
    }


    private void SetCrosshair() {
        if (playerItemController.usableItemInRange && !playerItemController.usedItem) {
            crosshair.color = Color.green;
        } else if (playerItemController.usableItemInRange) {
            crosshair.color = Color.red;
        } else {
            crosshair.color = Color.white;
        }

        if (playerController.lockInRange) {
            crosshair.sprite = crosshairUnLock;
        } else if (playerController.lockedObjectInReach) {
            crosshair.sprite = crosshairLocked;
        } else if (playerGrabItems.currentGrabbedItem) {
            crosshair.sprite = crosshairGrabbed;
        } else if (playerGrabItems.grabbableItemInRange) {
            crosshair.sprite = crosshairGrab;
        } else {
            crosshair.sprite = crosshairNormal;
        }
    }
}
