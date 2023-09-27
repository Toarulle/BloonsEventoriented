using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] public int health = 1;
    [SerializeField] public float speed = 1f;
    [SerializeField] public float checkpointSensitivity = 0.1f;
    [SerializeField] public int moneyWhenKilled = 1;
    [SerializeField] public List<Sprite> balloonSprites = null;
    [SerializeField] private Animator animator = null;
    private List<WeaponBehaviour> targetedBy = new List<WeaponBehaviour>();
    public DeathPortObject deathPort = null;
    public MoneyPortObject moneyPort = null;
    private float totalDistance = 0;

    private Vector2 previousLocation;
    private Transform currentWP = null;
    private int currentWPIndex = 0;
    private Vector2 currentDir;
    private SpriteRenderer spriteRend = null;

    public float DistanceTraveled
    {
        get {
            return totalDistance;
        }
    }
    
    void Start()
    {
        spriteRend = this.GetComponent<SpriteRenderer>();
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
        if (Vector2.Distance(transform.position, currentWP.position) <= checkpointSensitivity)
        {
            currentWPIndex++;
            if (currentWPIndex == WaypointsHandler.waypointList.Count)
            {
                Debug.Log("Lost 1 Life");
                deathPort.Pop(gameObject);
            }
            else
            {
                SetNextWaypoint(WaypointsHandler.waypointList[currentWPIndex]);
            }
        }
    }

    private void UpdateSprite()
    {
        Debug.Log(spriteRend.sprite + " - " + balloonSprites[health - 1]);
        spriteRend.sprite = balloonSprites[health - 1];
        Debug.Log(spriteRend.sprite + " - " + balloonSprites[health - 1]);
    }
    public void Pop(int damage)
    {
        if (health <= 0) return;
        health -= damage;
        moneyPort.Earn(moneyWhenKilled);
        if (health <= 0)
        {
            speed = 0;
            animator.SetTrigger("Popped");
            deathPort.Pop(gameObject);
            return;
        }
        UpdateSprite();
    }
}
