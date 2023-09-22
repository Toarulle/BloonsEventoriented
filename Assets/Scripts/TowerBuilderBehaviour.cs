using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerBuilderBehaviour : MonoBehaviour
{
    public MoneyPortObject moneyPort = null;
    public MoneyCounterObject moneyCounter = null;
    private bool holdingTower = false;
    private TowerBlueprint towerInHand = new TowerBlueprint();
    private Camera mainCamera = null;

    private static TowerBuilderBehaviour instance = null;
    public static TowerBuilderBehaviour Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!holdingTower) return;

        TowerFollowMouse();
        ReleaseTowerFromHand();
        PlaceTower();
    }

    private void TowerFollowMouse()
    {
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        towerInHand.prefab.transform.position = new Vector2(mousePos.x, mousePos.y);
    }
    
    public void GrabTower(TowerBlueprint tower)
    {
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        towerInHand.prefab = Instantiate(tower.prefab,new Vector2(mousePos.x, mousePos.y),Quaternion.identity);
        towerInHand.cost = tower.cost;
        holdingTower = true;
    }

    private void PlaceTower()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (moneyCounter.CurrentMoney >= towerInHand.cost)
            {
                moneyPort.Spend(towerInHand.cost);
                towerInHand = new TowerBlueprint();
                holdingTower = false;
            }
            else
            {
                Debug.Log("You can't afford this tower. You have $"+moneyCounter.CurrentMoney
                                                                   +" and the tower costs $" + towerInHand.cost);
            }
        }
    }
    
    private void ReleaseTowerFromHand()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Destroy(towerInHand.prefab);
            towerInHand = new TowerBlueprint();
            holdingTower = false;
        }
    }
}
