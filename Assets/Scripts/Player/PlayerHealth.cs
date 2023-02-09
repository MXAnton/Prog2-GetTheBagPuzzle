using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth = 100;

    private void Start() {
        SetHealth(maxHealth);
    }

    public void SetHealth(float _newHealth) {
        health = _newHealth;
    }

    public void AddHealth(float _amount) {
        float _newHealth = health + _amount;
        if (_newHealth > maxHealth) {
            _newHealth = maxHealth;
        }
        health = _newHealth;
    }
    public void RemoveHealth(float _amount) {
        float _newHealth = health - _amount;
        if (_newHealth <= 0) {
            _newHealth = 0;
        }
        health = _newHealth;

        if (health <= 0) {
            // Player dead
            Die();
        }
    }

    private void Die() {
        Debug.Log("Player dead");
    }
}
