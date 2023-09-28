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

    public void SetTextOnButton(string text, int cost)
    {
        if (buttonText == null)
        {
            Start();
        }
        buttonText.text = text + Environment.NewLine + "$" + cost;
    }
    
    public void SetTextOnButton(string text)
    {
        if (buttonText == null)
        {
            Start();
        }
        buttonText.text = text;
    }
}
