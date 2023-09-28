using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEditor.Build;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    private int towerTypeID; 
    public int towerUpgradeLevel = 0;
    [HideInInspector]public int nextUpgradeCost = 0;
    private int maxUpgradeLevel = 1;
    [HideInInspector]public bool maxLevel = false;
    private Vector3 originalDirection = Vector3.right;
    private Animator anim = null;
    private WeaponBehaviour currentWeapon = null;
    [SerializeField]private List<WeaponBehaviour> towerWeapons = null;
    [SerializeField]private List<Sprite> towerSprites = null;
    [SerializeField]private List<AnimatorController> animations = null;
    [SerializeField]private List<int> upgradeCost = new List<int>();

    public bool UpgradeTower()
    {
        if (towerUpgradeLevel < maxUpgradeLevel)
        {
            anim.enabled = false;
            towerUpgradeLevel++;
            currentWeapon.gameObject.SetActive(false);
            currentWeapon = towerWeapons[towerUpgradeLevel];
            currentWeapon.gameObject.SetActive(true);
            currentWeapon.ShowRange();
            nextUpgradeCost = upgradeCost[towerUpgradeLevel+1];
            anim.runtimeAnimatorController = animations[towerUpgradeLevel];
            GetComponent<SpriteRenderer>().sprite = towerSprites[towerUpgradeLevel];
            if (towerUpgradeLevel == maxUpgradeLevel)
            {
                maxLevel = true;
            }

            anim.enabled = true;
            return true;
        }
        
        return false;
    }
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        currentWeapon = towerWeapons[towerUpgradeLevel];
        nextUpgradeCost = upgradeCost[towerUpgradeLevel+1];
        anim.runtimeAnimatorController = animations[towerUpgradeLevel];
        GetComponent<SpriteRenderer>().sprite = towerSprites[towerUpgradeLevel];
    }

    public void UpdateCurrentWeapon()
    {
        Start();
    }
    
    public Quaternion RotateToTarget(Vector3 targetPosition)
    {
        anim.SetTrigger("Shoot");
        var newDirection = targetPosition-transform.position;
        var newRotation = Quaternion.FromToRotation(originalDirection, newDirection);
        transform.rotation = newRotation;
        return newRotation;
    }

    public void EnableShowRange()
    {
        currentWeapon.ShowRange();
    }
    public void DisableShowRange()
    {
        currentWeapon.DontShowRange();
    }

    public void UpdateRangeColor(Color color)
    {
        currentWeapon.UpdateRangeColor(color);
    }
}
