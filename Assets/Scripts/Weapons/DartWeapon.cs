using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DartWeapon : WeaponBehaviour
{
    [SerializeField] private int projectilesPerShot = 3;

    [SerializeField] private List<Vector3> projectilesOffset = new List<Vector3>();

    public override void UpgradeSpeed()
    {
        shotsPerSecond *= 1.1f;
    }

    protected override void Shoot()
    {
        if (!currentTarget.enabled)
        {
            UpdateTarget();
        }
        var projRot = tower.RotateToTarget(currentTarget.transform.position);
        for (int i = 0; i < projectilesPerShot; i++)
        {
            ProjectileBehaviour proj = Instantiate(projectile, 
                    projectileOrigin.position+projectilesOffset[i], projRot)
                .GetComponent<ProjectileBehaviour>();
            proj.Init(currentTarget.gameObject, damage, pierce);
        }
    }
}
