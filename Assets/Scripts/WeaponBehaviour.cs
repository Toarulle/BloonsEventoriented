using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private int pierce = 0;
    [SerializeField] private float shotsPerMinute = 60f;
    [SerializeField] public float range = 0f;
    [SerializeField] public GameObject projectile = null;
    [SerializeField] public Transform projectileOrigin = null;
    private float shootTime = 0f;

    private EnemyBehaviour currentTarget = null;
    private TowerBehaviour tower = null;
    private bool hasTarget = false;

    private void Start()
    {
        tower = GetComponentInParent<TowerBehaviour>();
    }

    private void FixedUpdate()
    {
        shootTime += Time.deltaTime;

        if (shootTime >= 60f/shotsPerMinute)
        {
            UpdateTarget();
            if (!hasTarget) return;
            Shoot();
            shootTime = 0f;
            currentTarget = null;
            hasTarget = false;
        }
    }

    private void OnValidate()
    {
        if (range < 0f)
        {
            range = 0f;
            Debug.LogWarning("Range has to be non-negative");
        }
        if (damage < 0)
        {
            damage = 0;
            Debug.LogWarning("Range has to be non-negative");
        }
        if (shotsPerMinute <= 0f)
        {
            shotsPerMinute = 0f;
            Debug.LogWarning("Time Between Shots has to be non-negative, non-zero");
        }
    }

    private void Shoot()
    {
        if (!currentTarget.enabled)
        {
            UpdateTarget();
            Debug.Log(currentTarget);
        }
        var projRot = tower.RotateToTarget(currentTarget.transform.position);
        ProjectileBehaviour proj = Instantiate(projectile, projectileOrigin.position, projRot)
            .GetComponent<ProjectileBehaviour>();
        proj.Init(currentTarget.gameObject, damage, pierce);
    }

    private void UpdateTarget()
    {
        Vector2 pos = transform.position;
        int layerMask = LayerMask.GetMask("Balloons");

        Collider2D[] collider = Physics2D.OverlapCircleAll(pos, range, layerMask);
        if (collider.Length < 1)
        {
            hasTarget = false;
            return;
        }
        foreach (var col in collider)
        {
            var temp = col.GetComponent<EnemyBehaviour>();
            if (currentTarget == null || temp.DistanceTraveled > currentTarget.DistanceTraveled)
            {
                currentTarget = temp;
                hasTarget = true;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
