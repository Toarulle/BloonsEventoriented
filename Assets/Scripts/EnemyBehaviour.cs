using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] public int health = 1;
    [SerializeField] public float checkpointSensitivity = 0.1f;
    [SerializeField] public int moneyWhenKilled = 1;
    [SerializeField] public EnemyInfoObject enemyInfo = null;
    private float speed = 1f;
    private Animator animator = null;
    private List<WeaponBehaviour> targetedBy = new List<WeaponBehaviour>();
    public DeathPortObject deathPort = null;
    public MoneyPortObject moneyPort = null;
    public HealthPortObject healthPort = null;
    private float totalDistance = 0;

    private Vector2 previousLocation;
    private Transform currentWP = null;
    private int currentWPIndex = 0;
    private Vector2 currentDir;
    private bool popped = false;

    public float DistanceTraveled
    {
        get {
            return totalDistance;
        }
    }

    public bool Popped
    {
        get {
            return popped;
        }
    }
    
    void Start()
    {
        UpdateSpeed();
        animator = GetComponent<Animator>();
        if (currentWP == null)
        {
            SetNextWaypoint(WaypointsHandler.waypointList[currentWPIndex]);
        }
        animator.SetInteger("Health", health);
    }

    private void FixedUpdate()
    {
        MoveTowardsCurrentWaypoint();
        RecordDistance();
    }

    public bool IsFrozen()
    {
        return speed == 0;
    }
    
    public void Freeze()
    {
        speed = 0;
    }

    public void UnFreeze()
    {
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        switch (health)
        {
            case >5:
                speed = enemyInfo.enemySpeeds[4];
                break;
            case >3:
                speed = enemyInfo.enemySpeeds[3];
                break;
            case 3:
                speed = enemyInfo.enemySpeeds[2];
                break;
            case 2:
                speed = enemyInfo.enemySpeeds[1];
                break;
            case 1:
                speed = enemyInfo.enemySpeeds[0];
                break;
        }
    }
    private void RecordDistance()
    {
        var currentPos = transform.position;
        totalDistance += Vector2.Distance(currentPos, previousLocation);
        previousLocation = currentPos;
    }
    
    private void SetNextWaypoint(Transform nextWP)
    {
        currentDir = Vector2.ClampMagnitude(nextWP.transform.position - transform.position,1f);
        currentWP = nextWP;
    }
    
    private void MoveTowardsCurrentWaypoint()
    {
        transform.Translate(currentDir * (Time.deltaTime * speed),Space.World);
        if (Vector2.Distance(transform.position, currentWP.position) <= checkpointSensitivity)
        {
            currentWPIndex++;
            if (!popped)
            {
                if (currentWPIndex >= WaypointsHandler.waypointList.Count)
                {
                    healthPort.LoseHealth(health);
                    deathPort.Pop(gameObject);
                    popped = true;
                }
                else
                {
                    SetNextWaypoint(WaypointsHandler.waypointList[currentWPIndex]);
                }
            }
        }
    }


    public void Pop(int damage)
    {
        if (health <= 0 || damage < 1) return;
        health -= damage;
        UpdateSpeed();
        moneyPort.Earn(moneyWhenKilled);
        animator.SetInteger("Health", health);
        if (health <= 0)
        {
            speed = 0;
            animator.SetTrigger("Popped");
            deathPort.Pop(gameObject);
            popped = true;
        }
    }
}
