using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    private VirusController virusController;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] backgroundMusics;

    [Space]
    [SerializeField]
    private float backgroundMusicPitchMultiplier = 0.5f;
    [SerializeField]
    private float backgroundMusicPitchStart = 0.7f;
    [SerializeField]
    private float backgroundMusicPitchMax = 1.5f;

    private void Start() {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = backgroundMusics[0];
        audioSource.loop = true;
        audioSource.Play();
    }

    private void FixedUpdate() {
        setBackgroundMusicPitch();
    }


    private void setBackgroundMusicPitch() {
        if (audioSource.pitch >= backgroundMusicPitchMax) {
            audioSource.pitch = backgroundMusicPitchMax;
            return;
        }

        float newPitch = virusController.virusSpreadTimer / virusController.virusFullySpreadTime;
        newPitch *= backgroundMusicPitchMultiplier;
        newPitch += backgroundMusicPitchStart;
        newPitch = Mathf.Min(newPitch, backgroundMusicPitchMax);
        audioSource.pitch = newPitch;
    }
}
