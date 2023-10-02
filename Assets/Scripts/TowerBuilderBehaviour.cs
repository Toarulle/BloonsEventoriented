using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TowerBuilderBehaviour : MonoBehaviour
{
    [SerializeField] private float distanceFromRoad = 0f; 
    public MoneyPortObject moneyPort = null;
    public MoneyCounterObject moneyCounter = null;
    private bool holdingTower = false;
    private TowerBlueprint towerInHand = new TowerBlueprint();
    private TowerBehaviour towerInHandBehaviour = null;
    private Camera mainCamera = null;
    private Color blockedGizmoColor = new Color(0.8f,0f,0f,0.55f);
    private Color normalGizmoColor = new Color(0.4f,0.4f,0.4f,0.55f);
    private Color currentGizmoColor = Color.white;

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

        currentGizmoColor = normalGizmoColor;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!holdingTower) return;

        bool correctPlacement = CheckCorrectPlacement();
        currentGizmoColor = normalGizmoColor;
        if (!correctPlacement)
        {
            currentGizmoColor = blockedGizmoColor;
        }
        towerInHand.prefab.GetComponent<TowerBehaviour>().UpdateRangeColor(currentGizmoColor);
        
        TowerFollowMouse();
        ReleaseTowerFromHand();
        PlaceTower(correctPlacement);
    }

    private void TowerFollowMouse()
    {
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        towerInHand.prefab.transform.position = new Vector2(mousePos.x, mousePos.y);
    }
    
    public void GrabTower(TowerBlueprint tower)
    {
        if (holdingTower) return;
        
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        towerInHand.prefab = Instantiate(tower.prefab,new Vector2(mousePos.x, mousePos.y),Quaternion.identity);
        towerInHand.cost = tower.cost;
        towerInHand.prefab.GetComponentInChildren<WeaponBehaviour>().enabled = false;
        towerInHand.prefab.layer = LayerMask.NameToLayer("Default");
        holdingTower = true;
        towerInHandBehaviour = towerInHand.prefab.GetComponent<TowerBehaviour>();
        towerInHandBehaviour.UpdateCurrentWeapon();
        towerInHandBehaviour.EnableShowRange();
    }

    private void PlaceTower(bool isPlacedCorrect)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (isPlacedCorrect)
            {
                if (moneyCounter.CurrentMoney >= towerInHand.cost)
                {
                    towerInHand.prefab.layer = LayerMask.NameToLayer("Towers");
                    towerInHand.prefab.GetComponentInChildren<WeaponBehaviour>().enabled = true;
                    moneyPort.Spend(towerInHand.cost);
                    towerInHandBehaviour.DisableShowRange();
                    towerInHand = new TowerBlueprint();
                    towerInHandBehaviour = null;
                    holdingTower = false;
                }
                else
                {
                    Debug.Log("You can't afford this tower. You have $" + 
                              moneyCounter.CurrentMoney + " and the tower costs $" + towerInHand.cost);
                }
            }
            else
            {
                Debug.Log("You can't place towers on the road or on other towers");
            }
        }
    }

    private bool CheckCorrectPlacement()
    {
        Vector2 towerPos = towerInHand.prefab.transform.position;
        int layerMaskRoad = LayerMask.GetMask("Road");
        int layerMaskTowers = LayerMask.GetMask("Towers");

        Collider2D colliderRoad = Physics2D.OverlapBox(towerPos, new Vector2(distanceFromRoad,distanceFromRoad), 0, layerMaskRoad);
        Collider2D colliderTowers = Physics2D.OverlapCircle(towerPos, distanceFromRoad/2f, layerMaskTowers);
        if (colliderRoad != null || colliderTowers != null)
        {
            return false;
        }
        return true;
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

    private void OnDrawGizmos()
    {
        if (holdingTower)
        {
            var towerPos = towerInHand.prefab.transform.position;
            Gizmos.color = currentGizmoColor;
            Gizmos.DrawWireSphere(towerPos,towerInHand.prefab.GetComponentInChildren<DartWeapon>().range);
            Gizmos.DrawWireCube(towerPos,(distanceFromRoad-0.3f)*Vector3.one);
        }
    }
}
