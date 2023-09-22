using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class WaveHandler : MonoBehaviour
{
    [SerializeField] private int currentWave = 0;
    [SerializeField] private GameObject enemyPrefab = null;
    [SerializeField] private List<int> enemiesPerWave = new List<int>();
    [SerializeField] private DeathPortObject deathPort = null;
    
    private float timeBetweenSpawns = 0.5f;
    private float time = 0.0f;

    private int spawnedAmount = 0;
    private List<GameObject> enemies = new List<GameObject>();
    private bool spawnEnabled = false;
    

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
        {
            if (enemies.Count == 0 && currentWave < enemiesPerWave.Count-1)
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
        spawnedAmount = 0;
        enemies = new List<GameObject>();
        spawnEnabled = true;
    }
    
    private void SpawnEnemies()
    {
        enemies.Add(Instantiate(enemyPrefab, transform.position, Quaternion.identity));
        spawnedAmount++;
        if (spawnedAmount == enemiesPerWave[currentWave])
        {
            spawnEnabled = false;
        }
    }

    public void EnemyDied(DeathPortObject deathPort, GameObject poppedBalloon)
    {
        enemies.Remove(poppedBalloon);
    }

    private void OnEnable()
    {
        deathPort.onPop += EnemyDied;
    }

    private void OnDisable()
    {
        deathPort.onPop -= EnemyDied;
    }
}
