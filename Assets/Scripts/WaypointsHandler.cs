using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsHandler : MonoBehaviour
{
    public static List<Transform> waypointList;

    private void Awake()
    {
        waypointList = new List<Transform>();
        foreach (Transform child in transform)
        {
            waypointList.Add(child.transform);
        }
    }
}
