using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyTowerButtonBehaviour : MonoBehaviour
{
    [SerializeField] private Image icon = null;
    private TextMeshProUGUI buttonText = null;

    private void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetSprite(Sprite sprite)
    {
        icon.sprite = sprite;
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
