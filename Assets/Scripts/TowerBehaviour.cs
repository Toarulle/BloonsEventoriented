using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    public int towerUpgradeLevel = 0;
    [HideInInspector]public int nextUpgradeCost = 0;
    private int maxUpgradeLevel = 3;
    [HideInInspector]public bool maxLevel = false;
    private Vector3 originalDirection = Vector3.right;
    private Animator anim = null;
    private WeaponBehaviour currentWeapon = null;
    [SerializeField]private List<WeaponBehaviour> towerWeapons = null;
    [SerializeField]private List<int> upgradeCost = new List<int>();

    public virtual bool UpgradeTower()
    {
        if (towerUpgradeLevel < maxUpgradeLevel)
        {
            towerUpgradeLevel++;
            if (towerUpgradeLevel < towerWeapons.Count)
            {
                currentWeapon.gameObject.SetActive(false);
                currentWeapon = towerWeapons[towerUpgradeLevel];
                currentWeapon.gameObject.SetActive(true);
                currentWeapon.ShowRange();
                anim.SetInteger("UpgradeLevel",towerUpgradeLevel);
            }
            else
            {
                currentWeapon.UpgradeSpeed();
                currentWeapon.UpgradeRange();
                currentWeapon.ShowRange();
            }
            if (towerUpgradeLevel < upgradeCost.Count-1)
            {
                nextUpgradeCost = upgradeCost[towerUpgradeLevel+1];
            }
            if (towerUpgradeLevel == maxUpgradeLevel)
            {
                maxLevel = true;
            }
            return true;
        }
        return false;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        currentWeapon = towerWeapons[towerUpgradeLevel];
        nextUpgradeCost = upgradeCost[towerUpgradeLevel+1];
        maxUpgradeLevel = upgradeCost.Count-1;
    }

    public void UpdateCurrentWeapon()
    {
        Start();
    }


    public void ShootAnimation()
    {
        anim.SetTrigger("Shoot");
    }
    public Quaternion RotateToTarget(Vector3 targetPosition)
    {
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
