using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameBehaviour : MonoBehaviour
{
    public DeathPortObject deathPort = null;
    public MoneyPortObject moneyPort = null;
    public MoneyCounterObject moneyCounter = null;
    public HealthPortObject healthPort = null;
    public HealthCounterObject healthCounter = null;
    public SelectTowerPortObject selectTowerPort = null;

    private TowerBehaviour selectedTower = null;
    private bool towerIsSelected = false;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

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
            Debug.LogWarning("Missing Money Counter reference.", this);
        }
        if (healthPort == null)
        {
            Debug.LogWarning("Missing Health Port reference.", this);
        }
        if (healthCounter == null)
        {
            Debug.LogWarning("Missing Health Counter reference.", this);
        }
        if (selectTowerPort == null)
        {
            Debug.LogWarning("Missing Select Tower Port reference.", this);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && towerIsSelected && !CheckForUIAtClick())
        {
            DeSelectTower();
        }

        if (Input.GetMouseButtonDown(0) && !CheckForUIAtClick())
        {
            SelectTower();
        }
    }

    private void SelectTower()
    {
        if (towerIsSelected)
        {
            DeSelectTower();
        }

        if (CheckForTowerAtClick())
        {
            selectTowerPort.Select(selectedTower);
            towerIsSelected = true;
        }
    }

    private bool CheckForTowerAtClick()
    {
        int layerMaskTowers = LayerMask.GetMask("Towers");
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var colliderTowers = Physics2D.OverlapCircle(mousePos, 0.2f, layerMaskTowers);
        if (colliderTowers != null)
        {
            selectedTower = colliderTowers.GetComponent<TowerBehaviour>();
            return true;
        }
        return false;
    }

    private bool CheckForUIAtClick()
    {
        int layerMaskUI = LayerMask.GetMask("UI");
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        var colliderUI = Physics2D.OverlapCircle(mousePos, 0.2f, layerMaskUI);
        if (colliderUI != null)
        {
            return true;
        }
        return false;
    }
    
    private void DeSelectTower()
    {
        selectTowerPort.DeSelect(selectedTower);
    }
    
    public void GetMoney(MoneyPortObject moneyPort, int money)
    {
        moneyCounter.CurrentMoney += money;
    }

    public void SpendMoney(MoneyPortObject moneyPort, int money)
    {
        moneyCounter.CurrentMoney -= money;
    }

    public void LoseHealth(HealthPortObject healthPort, int health)
    {
        healthCounter.CurrentHealth -= health;
    }

    public void OnEnable()
    {
        moneyPort.onEarn += GetMoney;
        moneyPort.onSpend += SpendMoney;
        healthPort.onLostHealth += LoseHealth;
    }

    private void OnDisable()
    {
        moneyPort.onEarn -= GetMoney;
        moneyPort.onSpend -= SpendMoney;
        healthPort.onLostHealth -= LoseHealth;
    }
}
