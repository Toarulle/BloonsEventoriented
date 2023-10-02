using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FreezeWeapon : WeaponBehaviour
{
    [SerializeField] private float freezeTime = 1.0f;
    
    protected List<EnemyBehaviour> currentTargets = new List<EnemyBehaviour>(); 
    
    protected override void FixedUpdate()
    {
        shootTime += Time.deltaTime;

        if (shootTime >= freezeTime)
        {
            foreach (var target in currentTargets)
            {
                if (target.IsFrozen())
                {
                    target.UnFreeze();
                    target.Pop(damage);
                }
            }
        }
        if (shootTime >= 1f/shotsPerSecond)
        {
            UpdateTarget();
            if (!hasTarget) return;
            Shoot();
            shootTime = 0f;
            hasTarget = false;
        }
    }
    public override  void UpgradeSpeed()
    {
        shotsPerSecond *= 1.1f;
    }
    public override void UpgradeRange()
    {
        range *= 1.1f;
    }
    protected override void OnValidate()
    {
        if (freezeTime >= (1f/shotsPerSecond))
        {
            freezeTime = (1f/shotsPerSecond) - 0.1f;
            Debug.LogWarning("Freeze Time has to be lower than it takes to freeze again", this);
        }
        base.OnValidate();
    }

    protected override void Shoot()
    {
        tower.ShootAnimation();
        foreach (var target in currentTargets)
        {
            target.Pop(damage);
            target.Freeze();
        }
    }

    protected override void UpdateTarget()
    {
        
        Vector2 pos = transform.position;
        int layerMask = LayerMask.GetMask("Balloons");
        currentTargets = new List<EnemyBehaviour>();
        Collider2D[] colliderList = Physics2D.OverlapCircleAll(pos, range, layerMask);
        if (colliderList.Length < 1)
        {
            hasTarget = false;
            return;
        }
        
        foreach (var collider in colliderList)
        {
            currentTargets.Add(collider.GetComponent<EnemyBehaviour>());
        }

        if (currentTargets.Count > 0)
        {
            hasTarget = true;
        }
    }
}
