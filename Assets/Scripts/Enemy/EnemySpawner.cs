using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyBase[] enemyVariety;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    [Space]
    [SerializeField] private float nextSpawnTimer = 8;
    private float timeSinceLastSpawn;

    private void Update()
    {
        if (LevelManager.Instance.currentLevelState != LevelState.running)
            return;

        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > nextSpawnTimer)
        {
            SpawnEnemy(spawnPoints[Random.Range((int) 0, spawnPoints.Count)]);
            // ComputeNextSpawnData();
        }
    }

    public void SpawnEnemy(Transform spawnPoint)
    {
        EnemyBase enemyType = enemyVariety[Random.Range(0, enemyVariety.Length)];
        EnemyBase enemyBase = Instantiate(enemyType, spawnPoint.position, Quaternion.identity);
        enemyBase.Init();
        timeSinceLastSpawn = 0;
    }
}
