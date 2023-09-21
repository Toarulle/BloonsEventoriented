using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] public int health = 1;
    [SerializeField] public float speed = 1f;
    [SerializeField] public float sensitivity = 0.1f;
    [SerializeField] public int moneyWhenKilled = 1;
    public DeathPortObject deathPort = null;
    public MoneyPortObject moneyPort = null;

    private Transform currentWP = null;
    private int currentWPIndex = 0;
    private Vector2 currentDir;

    void Start()
    {
        if (currentWP == null)
        {
            SetNextWaypoint(WaypointsHandler.waypointList[currentWPIndex]);
        }
    }

    void Update()
    {
        MoveTowardsCurrentWaypoint();
    }

    private void SetNextWaypoint(Transform nextWP)
    {
        currentDir = Vector2.ClampMagnitude(nextWP.transform.position - transform.position,1f);
        currentWP = nextWP;
    }
    
    private void MoveTowardsCurrentWaypoint()
    {
        transform.Translate(currentDir * (Time.deltaTime * speed),Space.World);
        if (Vector2.Distance(transform.position, currentWP.position) <= sensitivity)
        {
            currentWPIndex++;
            if (currentWPIndex == WaypointsHandler.waypointList.Count)
            {
                Destroy(gameObject);
            }
            else
            {
                SetNextWaypoint(WaypointsHandler.waypointList[currentWPIndex]);
            }
        }
    }

    public void Pop(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            deathPort.Pop(gameObject);
            moneyPort.Earn(moneyWhenKilled);
        }
    }
}
