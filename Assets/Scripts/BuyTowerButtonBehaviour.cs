using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyTowerButtonBehaviour : MonoBehaviour
{
    private TextMeshProUGUI buttonText = null;

    private void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetTextOnButton(TowerBlueprint tower)
    {
        if (buttonText == null)
        {
            Start();
        }
        buttonText.text = tower.prefab.name + Environment.NewLine + "$" + tower.cost;
    }
}
