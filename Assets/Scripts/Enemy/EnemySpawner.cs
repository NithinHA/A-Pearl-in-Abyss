using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private EnemyBase[] enemyVariety;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [Space]
    [SerializeField] private float[] waveWaitTimes = new float[4] { 15, 18, 18, 20 };
    [SerializeField] private float[] waveRunTimes = new float[4] { 16, 18, 18, 22 };
    bool waveState = false;     // false: waiting for wave, true: experiencing wave
    [SerializeField] private int firstWaveEnemyCount = 4;
    [SerializeField] private float waveEnemyincrement = 1;

    private int waveIndex = 0;
    private float waitingTime = 0;

    private float thisWaveSpawnInterval;
    private int enemiesSpawnedThisWave;

    public Action<bool> OnWaveStateChange;

    private void Update()
    {
        if (LevelManager.Instance.currentLevelState != LevelState.running)
            return;

        waitingTime += Time.deltaTime;
        if (!waveState)
        {
            Debug.Log("End Wave!");
            if(waitingTime > waveWaitTimes[waveIndex])
            {
                waitingTime = 0;
                waveState = true;
                thisWaveSpawnInterval = waveRunTimes[waveIndex] / ((waveEnemyincrement * waveIndex) + firstWaveEnemyCount);
                OnWaveStateChange?.Invoke(waveState);
            }
        }
        else
        {
            Debug.Log("Begin Wave!");
            if(waitingTime > waveRunTimes[waveIndex])
            {
                waitingTime = 0;
                waveState = false;
                waveIndex++;
                if (waveIndex >= waveWaitTimes.Length)
                    waveIndex = 1;
                enemiesSpawnedThisWave = 0;
                OnWaveStateChange?.Invoke(waveState);
            }
            else if(waitingTime > thisWaveSpawnInterval * (enemiesSpawnedThisWave + 1))
            {
                SpawnEnemy(spawnPoints[UnityEngine.Random.Range((int)0, spawnPoints.Count)]);
                enemiesSpawnedThisWave += 1;
            }
        }
    }

	public void SpawnEnemy(Transform spawnPoint)
    {
        EnemyBase enemyType = enemyVariety[UnityEngine.Random.Range(0, enemyVariety.Length)];
        EnemyBase enemyBase = Instantiate(enemyType, spawnPoint.position, Quaternion.identity);
        enemyBase.Init();
        //timeSinceLastSpawn = 0;
    }
}
