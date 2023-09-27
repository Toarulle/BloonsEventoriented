using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class Wave
{
    public List<int> amountOfBalloonsPerStrength = new List<int>(5);
}
    
public class WaveHandler : MonoBehaviour
{
    [SerializeField] private int currentWave = 0;
    [SerializeField] private List<GameObject> enemyPrefab = null;
    [SerializeField] private List<Wave> waveList = new List<Wave>();
    [SerializeField] private DeathPortObject deathPort = null;
    
    private float timeBetweenSpawns = 0.3f;
    private float time = 0.0f;

    private List<GameObject> enemies = new List<GameObject>();
    private bool spawnEnabled = false;
    private int balloonToSpawn = 0;

    

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.N))
        {
            if (enemies.Count == 0 && currentWave < waveList.Count-1)
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
        enemies = new List<GameObject>();
        spawnEnabled = true;
        balloonToSpawn = 0;
        while (waveList[currentWave].amountOfBalloonsPerStrength[balloonToSpawn] == 0)
        {
            balloonToSpawn++;
        }
    }
    
    private void SpawnEnemies()
    {
        if (waveList[currentWave].amountOfBalloonsPerStrength[balloonToSpawn] == 0)
        {
            balloonToSpawn++;
            if (waveList[currentWave].amountOfBalloonsPerStrength[balloonToSpawn] == 0)
            {
                spawnEnabled = false;
                return;
            }
        }
        enemies.Add(Instantiate(enemyPrefab[balloonToSpawn], transform.position, Quaternion.identity));
        waveList[currentWave].amountOfBalloonsPerStrength[balloonToSpawn]--;
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
