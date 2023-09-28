using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTowerBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject buttonPanel = null;
    [SerializeField] private GameObject buttonPrefab = null;
    [SerializeField] private SelectTowerPortObject selectTowerPort = null;

    public MoneyPortObject moneyPort = null;
    public MoneyCounterObject moneyCounter = null;

    private List<GameObject> buttons = new List<GameObject>();
    private bool showingUpgradeButton = false;

    private void UpgradeTower(TowerBehaviour towerBehaviour)
    {
        int nextUpgradeCost = towerBehaviour.nextUpgradeCost;
        if (moneyCounter.CurrentMoney > nextUpgradeCost)
        {
            if (towerBehaviour.UpgradeTower())
            {
                moneyPort.Spend(nextUpgradeCost);
            }
        }

        if (towerBehaviour.maxLevel)
        {
            buttons[0].GetComponent<BuyTowerButtonBehaviour>().SetTextOnButton("Max level");
        }
    }
    
    private void ShowUpgradePanel(SelectTowerPortObject selectTowerPort, TowerBehaviour towerBehaviour)
    {
        if (!showingUpgradeButton)
        {
            towerBehaviour.EnableShowRange();
            showingUpgradeButton = true;
            buttons.Add(Instantiate(buttonPrefab, buttonPanel.transform));
            if (!towerBehaviour.maxLevel)
            {
                buttons[0].GetComponent<Button>().onClick.AddListener(() => UpgradeTower(towerBehaviour));
                buttons[0].GetComponent<BuyTowerButtonBehaviour>().SetTextOnButton("Upgrade Tower", towerBehaviour.nextUpgradeCost);
                
                return;
            }
            buttons[0].GetComponent<BuyTowerButtonBehaviour>().SetTextOnButton("Max level");
        }
    }

    private void RemoveUpgradePanel(SelectTowerPortObject selectTowerPortObject, TowerBehaviour towerBehaviour)
    {
        foreach (var button in buttons)
        {
            Destroy(button);
        }
        towerBehaviour.DisableShowRange();
        buttons = new List<GameObject>();
        showingUpgradeButton = false;
    }
    
    private void OnEnable()
    {
        selectTowerPort.onSelect += ShowUpgradePanel;
        selectTowerPort.onDeSelect += RemoveUpgradePanel;
    }

    private void OnDisable()
    {
        selectTowerPort.onSelect -= ShowUpgradePanel;
        selectTowerPort.onDeSelect -= RemoveUpgradePanel;
    }
}
