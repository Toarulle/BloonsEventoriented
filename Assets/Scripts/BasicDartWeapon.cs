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

    [SerializeField] private List<GameObject> enemiesInRange = new List<GameObject>();
    private GameObject currentTarget = null;
    
    private void Update()
    {
        shootTime += Time.deltaTime;
        if (enemiesInRange.Count == 0) return;

        if (shootTime >= timeBetweenShots)
        {
            UpdateTarget();
            Shoot();
            shootTime = 0f;
        }
    }

    private void Shoot()
    {
        currentTarget.GetComponent<EnemyBehaviour>().Pop(damage);
    }

    private void UpdateTarget()
    {
        currentTarget = enemiesInRange.First();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        enemiesInRange.Add(col.gameObject);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        enemiesInRange.Remove(other.gameObject);
    }
}
