using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] public int damage = 1;
    [SerializeField] public int pierce = 0;
    [SerializeField] public float shotsPerSecond = 1f;
    [SerializeField] public float range = 1f;
    [SerializeField] public GameObject projectile = null;
    [SerializeField] public GameObject rangeIndicator = null;
    [SerializeField] public Transform projectileOrigin = null;
    protected float shootTime = 0f;

    protected EnemyBehaviour currentTarget = null;
    protected TowerBehaviour tower = null;
    protected bool hasTarget = false;
    private SpriteRenderer rangeSpriteRenderer;

    protected virtual void Start()
    {
        tower = GetComponentInParent<TowerBehaviour>();
        rangeSpriteRenderer = rangeIndicator.GetComponent<SpriteRenderer>();
    }

    protected virtual void FixedUpdate()
    {
        shootTime += Time.deltaTime;

        if (shootTime >= 1f/shotsPerSecond)
        {
            UpdateTarget();
            if (!hasTarget) return;
            Shoot();
            shootTime = 0f;
            currentTarget = null;
            hasTarget = false;
        }
    }

    protected virtual void OnValidate()
    {
        if (range <= 0f)
        {
            range = 0f;
            Debug.LogWarning("Range has to be larger than zero");
        }
        if (damage < 0)
        {
            damage = 0;
            Debug.LogWarning("Range has to be non-negative");
        }
        if (shotsPerSecond <= 0f)
        {
            shotsPerSecond = 0f;
            Debug.LogWarning("Time Between Shots has to be non-negative, non-zero");
        }

        rangeIndicator.transform.localScale = new Vector3(range*4, range*4, 1);
    }

    public virtual void UpgradeSpeed()
    {
        shotsPerSecond *= 1.05f;
    }
    public virtual void UpgradeRange()
    {
        range *= 1.1f;
    }
    protected virtual void Shoot()
    {
        if (!currentTarget.enabled)
        {
            UpdateTarget();
        }
        tower.ShootAnimation();
        var projRot = tower.RotateToTarget(currentTarget.transform.position);
        ProjectileBehaviour proj = Instantiate(projectile, projectileOrigin.position, projRot)
            .GetComponent<ProjectileBehaviour>();
        proj.Init(currentTarget.gameObject, damage, pierce);
    }

    protected virtual void UpdateTarget()
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

    public void ShowRange()
    {
        rangeIndicator.transform.localScale = new Vector3(range * 4, range * 4, 1);
        if (rangeSpriteRenderer == null)
        {
            Start();
        }
        rangeSpriteRenderer.enabled = true;
    }

    public void DontShowRange()
    {
        rangeSpriteRenderer.enabled = false;
    }
    
    public void UpdateRangeColor(Color color)
    {
        rangeIndicator.GetComponent<SpriteRenderer>().color = color;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
