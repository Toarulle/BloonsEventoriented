using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject target = null;
    private int damage = 0;
    private int pierce = 0;
    private Rigidbody2D rb = null;
    private Vector2 direction;
    
    public void Init(GameObject target, int damage, int pierce)
    {
        this.target = target;
        this.pierce = pierce;
        this.damage = damage;
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = target.transform.position - transform.position;
        rb.velocity = direction * speed;
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        EnemyBehaviour enemy = col.GetComponent<EnemyBehaviour>(); 
        if (enemy.Popped)
        {
            return;
        }
        enemy.Pop(damage);
        if (pierce <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            pierce--;
        }    
    }
}
