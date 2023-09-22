using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class ShopBehaviour : MonoBehaviour
{
    public List<TowerBlueprint> towers = new List<TowerBlueprint>();

    [SerializeField] private GameObject buttonPanel = null;
    [SerializeField] private GameObject buttonPrefab = null;

    private TowerBuilderBehaviour towerBuilder;
    void Start()
    {
        towerBuilder = TowerBuilderBehaviour.Instance;
        foreach (var tower in towers)
        {
            var newButton = Instantiate(buttonPrefab, buttonPanel.transform);
            newButton.GetComponent<Button>().onClick.AddListener(() => towerBuilder.GrabTower(tower));
            newButton.GetComponent<BuyTowerButtonBehaviour>().SetTextOnButton(tower);
        }
    }
}
