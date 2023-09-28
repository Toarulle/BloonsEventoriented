using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HealthCounter", menuName = "Bloons TD/Health Counter", order = 3)]
public class HealthCounterObject : ScriptableObject
{
    public UnityAction<HealthCounterObject, int> onHealthChange = delegate{};

    public int startingHealth = 40;
    private int currentHealth = 0;

    public int CurrentHealth
    {
        get {
            return currentHealth;
        }
        set {
            if (currentHealth != value)
            {
                int healthExchange = value - currentHealth;
                currentHealth = value;
                onHealthChange(this, healthExchange);
            }
        }
    }

    private void OnValidate()
    {
        if (startingHealth <= 0)
        {
            startingHealth = 1;
            Debug.LogWarning("Starting Health must be a positive value.", this);
        }
    }

    private void OnEnable()
    {
        CurrentHealth = startingHealth;
    }
}
