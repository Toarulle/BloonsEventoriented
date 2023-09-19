using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBehaviour : MonoBehaviour
{

    [SerializeField] public float speed = 1f;
    [SerializeField] public float sensitivity = 0.1f;
    private Transform currentWP = null;
    private int currentWPIndex = 0;
    private Vector2 currentDir;

    EnemyBehaviour(Transform startWP)
    {
        SetNextWaypoint(startWP);
    }
    
    void Start()
    {
        if (currentWP == null)
        {
            currentWP = transform;
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
        if (Vector2.Distance(transform.position, currentWP.position) < sensitivity)
        {
            if (currentWPIndex == WaypointsHandler.waypointList.Count)
            {
                Destroy();
            }
            else
            {
                SetNextWaypoint(WaypointsHandler.waypointList[++currentWPIndex]);
            }
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
