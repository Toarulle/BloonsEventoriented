using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MoneyTextBehaviour : MonoBehaviour
{
    public TextMeshProUGUI moneyText = null;
    public MoneyCounterObject moneyCounter = null;

    private void OnValidate()
    {
        if (moneyCounter == null)
        {
            Debug.LogWarning("Missing Money Counter reference.", this);
        }
        if (moneyText == null)
        {
            Debug.LogWarning("Missing Money Text reference.", this);
        }
    }

    private void UpdateText()
    {
        moneyText.text = moneyCounter.CurrentMoney.ToString();
    }

    private void OnMoneyChange(MoneyCounterObject moneyCounter, int money)
    {
        UpdateText();
    }

    private void OnEnable()
    {
        UpdateText();
        moneyCounter.onMoneyChange += OnMoneyChange;
    }

    private void OnDisable()
    {
        moneyCounter.onMoneyChange -= OnMoneyChange;
    }
}
