using System;
using System.Collections;
using System.Collections.Generic;
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
            var buttonBehaviour = newButton.GetComponent<BuyTowerButtonBehaviour>();
            buttonBehaviour.SetTextOnButton(tower.prefab.name, tower.cost);
            buttonBehaviour.SetSprite(tower.prefab.GetComponent<SpriteRenderer>().sprite);
        }
    }
}
