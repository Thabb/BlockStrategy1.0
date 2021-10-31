using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthEntity : MonoBehaviour
{
    public HealthBar healthBar;
    
    public float maxHealth;
    public float currentHealth;
    public float armor;

    public int team;

    private bool isAlive = true;

    public void Start()
    {
        healthBar.SetHealth(this);
    }

    public void TakeDamage(float incomingDamage)
    {
        currentHealth -= incomingDamage;
        healthBar.SetHealth(this);
        
        if (currentHealth <= 0) DestroySelf();
    }

    private void DestroySelf()
    {
        if (isAlive) Destroy(gameObject);
        isAlive = false;
    }
}
