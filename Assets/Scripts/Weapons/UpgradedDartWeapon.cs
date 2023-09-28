using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradedDartWeapon : WeaponBehaviour
{
    [SerializeField] private int projectilesPerShot = 3;

    [SerializeField] private List<Vector3> projectilesOffset = new List<Vector3>();

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
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
            ProjectileBehaviour proj = Instantiate(projectile, projectileOrigin.position+projectilesOffset[i], projRot)
                .GetComponent<ProjectileBehaviour>();
            proj.Init(currentTarget.gameObject, damage, pierce);
        }
    }
}
