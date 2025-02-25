using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "MoneyCounter", menuName = "Bloons TD/Money Counter", order = 2)]
public class MoneyCounterObject : ScriptableObject
{
    public UnityAction<MoneyCounterObject, int> onMoneyChange = delegate{};

    public int startingMoney = 600;
    private int currentMoney = 0;

    public int CurrentMoney
    {
        get {
            return currentMoney;
        }
        set {
            if (currentMoney != value)
            {
                int moneyExchange = value - currentMoney;
                currentMoney = value;
                onMoneyChange(this, moneyExchange);
            }
        }
    }

    private void SetStartMoney(Scene scene, LoadSceneMode mode)
    {
        CurrentMoney = startingMoney;   
    }
    private void OnEnable()
    {
        CurrentMoney = startingMoney;
        SceneManager.sceneLoaded += SetStartMoney;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SetStartMoney;
    }
}
