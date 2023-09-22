using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicDartWeapon : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float timeBetweenShots = 1f;
    [SerializeField] private float shootTime = 0f;

    [SerializeField] private List<EnemyBehaviour> enemiesInRange = new List<EnemyBehaviour>();
    private EnemyBehaviour currentTarget = null;
    
    private void FixedUpdate()
    {
        shootTime += Time.deltaTime;
        if (enemiesInRange.Count == 0) return;

        if (shootTime >= timeBetweenShots)
        {
            UpdateTarget();
            Shoot();
            shootTime = 0f;
            currentTarget = null;
        }
    }

    private void Shoot()
    {
        if (currentTarget.Pop(damage))
        {
            enemiesInRange.Remove(currentTarget);
        }
    }

    private void UpdateTarget()
    {
        foreach (var enemy in enemiesInRange)
        {
            if (currentTarget == null || enemy.DistanceTraveled > currentTarget.DistanceTraveled)
            {
                currentTarget = enemy;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        enemiesInRange.Add(col.gameObject.GetComponent<EnemyBehaviour>());
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        enemiesInRange.Remove(other.gameObject.GetComponent<EnemyBehaviour>());
    }
}
