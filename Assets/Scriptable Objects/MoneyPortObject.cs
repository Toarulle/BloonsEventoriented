using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MoneyPort", menuName = "Bloons TD/Money Port", order = 2)]
public class MoneyPortObject : ScriptableObject
{
    public UnityAction<MoneyPortObject, int> onEarn = delegate{};
    public UnityAction<MoneyPortObject, int> onSpend = delegate{};

    public void Spend(int money)
    {
        onSpend(this, money);
    }

    public void Earn(int money)
    {
        onEarn(this, money);
    }
}
