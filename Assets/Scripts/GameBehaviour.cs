using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    public DeathPortObject deathPort = null;
    public MoneyPortObject moneyPort = null;
    public MoneyCounterObject moneyCounter = null;


    private void OnValidate()
    {
        if (deathPort == null)
        {
            Debug.LogWarning("Missing Death Port reference.", this);
        }
        if (moneyPort == null)
        {
            Debug.LogWarning("Missing Money Port reference.", this);
        }
        if (moneyCounter == null)
        {
            Debug.LogWarning("Missing Money cCounter reference.", this);
        }
    }

    public void GetMoney(MoneyPortObject moneyPort, int money)
    {
        moneyCounter.CurrentMoney += money;
    }

    public void SpendMoney(MoneyPortObject moneyPort, int money)
    {
        moneyCounter.CurrentMoney -= money;
    }

    private void Start()
    {
        moneyCounter.CurrentMoney = 0;
    }

    public void OnEnable()
    {
        moneyPort.onEarn += GetMoney;
        moneyPort.onSpend += SpendMoney;
    }

    private void OnDisable()
    {
        moneyPort.onEarn -= GetMoney;
        moneyPort.onSpend -= SpendMoney;
    }
}
