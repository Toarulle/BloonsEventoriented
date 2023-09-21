using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MoneyCounter", menuName = "Bloons TD/Money Counter", order = 2)]
public class MoneyCounterObject : ScriptableObject
{
    public UnityAction<MoneyCounterObject, int> onChange = delegate{};

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
                onChange(this, moneyExchange);
            }
        }
    }
}
