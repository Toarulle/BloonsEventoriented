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
    [SerializeField] public List<Sprite> balloons = null;
    [SerializeField] private Animator animator = null;
    public DeathPortObject deathPort = null;
    public MoneyPortObject moneyPort = null;
    private float totalDistance = 0;

    private Vector2 previousLocation;
    private Transform currentWP = null;
    private int currentWPIndex = 0;
    private Vector2 currentDir;

    public float DistanceTraveled
    {
        get {
            return totalDistance;
        }
    }
    
    void Start()
    {
        if (currentWP == null)
        {
            SetNextWaypoint(WaypointsHandler.waypointList[currentWPIndex]);
        }
    }

    private void FixedUpdate()
    {
        MoveTowardsCurrentWaypoint();
        RecordDistance();
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
        if (Vector2.Distance(transform.position, currentWP.position) <= sensitivity)
        {
            currentWPIndex++;
            if (currentWPIndex == WaypointsHandler.waypointList.Count)
            {
                Debug.Log("Lost 1 Life");
                Destroy(gameObject);
            }
            else
            {
                SetNextWaypoint(WaypointsHandler.waypointList[currentWPIndex]);
            }
        }
    }

    public bool Pop(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            speed = 0;
            moneyPort.Earn(moneyWhenKilled);
            animator.SetTrigger("Popped");
            deathPort.Pop(gameObject);
            return true;
        }

        return false;
    }
}
