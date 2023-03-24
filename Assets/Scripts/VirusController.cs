using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusController : MonoBehaviour
{
    [SerializeField]
    private PlayerHealth playerHealth;

    [Space]
    [SerializeField]
    private float virusDamage = 2f;
    [SerializeField]
    private float damageDelay = 2f;

    [Space]
    public float virusFullySpreadTime = 120;   // 2min * 60sec = 120sec
    public float virusSpreadTimer = 0f;
    
    private void Start() {
        // init vars
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        virusSpreadTimer = 0;

        StartCoroutine(DamagePlayer());
    }

    private void Update() {
        virusSpreadTimer += Time.deltaTime;
            // ??? virusdamage RISE and virusfog RISE, when time goes. ???
                // Makes virus seem to be catching player more intense.

        if (virusSpreadTimer >= virusFullySpreadTime) {
            // Virus fully spread
            // ??? MAX VIRUSDAMAGE and MAX VIRUS FOG etc. ???
            Debug.Log("Virus fully spread, hurry up.");
        }
    }

    private IEnumerator DamagePlayer() {
        yield return new WaitForSeconds(damageDelay);

        playerHealth.RemoveHealth(virusDamage);

        // Continue loop of damage
        StartCoroutine(DamagePlayer());
    }
}
