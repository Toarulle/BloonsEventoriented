using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthTextBehaviour : MonoBehaviour
{
    public TextMeshProUGUI healthText = null;
    public HealthCounterObject healthCounter = null;

    private void OnValidate()
    {
        if (healthCounter == null)
        {
            Debug.LogWarning("Missing Money Counter reference.", this);
        }
        if (healthText == null)
        {
            Debug.LogWarning("Missing Money Text reference.", this);
        }
    }

    private void UpdateText()
    {
        healthText.text = healthCounter.CurrentHealth.ToString();
    }

    private void HealthChanged(HealthCounterObject healthCounter, int health)
    {
        UpdateText();
    }

    private void OnEnable()
    {
        UpdateText();
        healthCounter.onHealthChange += HealthChanged;
    }

    private void OnDisable()
    {
        healthCounter.onHealthChange -= HealthChanged;
    }
}
