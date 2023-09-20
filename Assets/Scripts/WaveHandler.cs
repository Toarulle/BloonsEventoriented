using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class WaveHandler : MonoBehaviour
{
    public UnityAction<WaveHandler> onEnemyKill = delegate{};


    [SerializeField] private int currentWave = 0;
    [SerializeField] private GameObject enemyPrefab = null;
    [SerializeField] private List<int> enemiesPerWave = new List<int>();
    
    private float timeBetweenSpawns = 0.5f;
    private float time = 0.0f;

    private int enemiesAlive = 0;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool spawnEnabled = false;
    

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (enemiesAlive==0 && currentWave < enemiesPerWave.Count)
            {
                NextWave();
            }
        }
        
        
        if (!spawnEnabled) return;

        time += Time.deltaTime;
        if (time >= timeBetweenSpawns)
        {
            SpawnEnemies();
            time -= timeBetweenSpawns;
        }
    }

    private void NextWave()
    {
        currentWave++;
        enemiesAlive = 0;
        spawnedEnemies = new List<GameObject>();
        spawnEnabled = true;
    }
    
    private void SpawnEnemies()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemiesAlive++;
        if (enemiesAlive == enemiesPerWave[currentWave])
        {
            spawnEnabled = false;
        }
    }

    public void EnemyDied()
    {
        enemiesAlive--;
    }
    
}
