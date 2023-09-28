using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HealthPort", menuName = "Bloons TD/Health Port", order = 3)]
public class HealthPortObject : ScriptableObject
{
    public UnityAction<HealthPortObject, int> onLostHealth = delegate{};

    public void LoseHealth(int health)
    {
        onLostHealth(this, health);
    }
}
