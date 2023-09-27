using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
    private Vector3 originalDirection = Vector3.right;
    private Animator anim = null;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public Quaternion RotateToTarget(Vector3 targetPosition)
    {
        anim.SetTrigger("Shoot");
        var newDirection = targetPosition-transform.position;
        var newRotation = Quaternion.FromToRotation(originalDirection, newDirection);
        transform.rotation = newRotation;
        return newRotation;
    }
}
