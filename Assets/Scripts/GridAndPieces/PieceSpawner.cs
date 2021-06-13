using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PieceSpawner : Singleton<PieceSpawner>
{
    [SerializeField] private Piece piecePrefab;
    [SerializeField] private List<PieceType> pieceTypes = new List<PieceType>(4);
    [SerializeField] private int[] orientations = {0, -90, 90, 180};
    [Space]
    [SerializeField] private int[] pieceSpawnRate = new int[] { 5, 7, 11, 14 };
    
    public List<PieceSpawnPoints> spawnPoints = new List<PieceSpawnPoints>();

    [Space]
    [SerializeField] private float nextSpawnTimer = 5;
    [SerializeField] private float timeSinceLastSpawn;
    private PieceSpawnPoints nextSpawnPoint;

    protected override void Start()
    {
        nextSpawnPoint = spawnPoints[0];

        InputControl.OnPiecePlaced += OnPiecePlaced;
    }

    protected override void OnDestroy()
    {
        InputControl.OnPiecePlaced -= OnPiecePlaced;
    }

    private void Update()
    {
        if (LevelManager.Instance.currentLevelState != LevelState.running)
            return;

        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > nextSpawnTimer && nextSpawnPoint.isAvailable)
        {
            SpawnPiece(nextSpawnPoint);
            ComputeNextSpawnData();
        }
    }

    public void SpawnPiece(PieceSpawnPoints spawnPoint)
    {
        PieceType type = pieceTypes[GetRandomValue(new List<float>() {25, 25, 25, 25})];
        int orientation = orientations[GetRandomValue(new List<float>() {40, 20, 20, 20})];

        Piece piece = Instantiate(piecePrefab, spawnPoint.transform);
        piece.transform.localPosition = Vector3.zero;
        piece.Init(type, orientation, spawnPoint);
        spawnPoint.SetPiece(piece);
        timeSinceLastSpawn = 0;
    }

    int GetRandomValue(List<float> probability)
    {
        float rand = Random.Range(0, 100);
        float count = 0;
        for (var i = 0; i < probability.Count; i++)
        {
            count += probability[i];
            if (rand < count)
                return i;
        }

        return probability.Count - 1;
    }

    private void ComputeNextSpawnData()
    {
        int availableSlots = 0;
        for (int i = spawnPoints.Count - 1; i >= 0; i--)
        {
            var item = spawnPoints[i];
            if (item.isAvailable)
            {
                availableSlots += 1;
                nextSpawnPoint = item;
            }
        }

        nextSpawnTimer = availableSlots > 3 ? pieceSpawnRate[0] : availableSlots > 2 ? pieceSpawnRate[1] : availableSlots > 1 ? pieceSpawnRate[2] : pieceSpawnRate[3];
        // nextSpawnTimer = Random.Range(3, 5);
    }

    private void OnPiecePlaced(Piece piece, Cell cell, PieceSpawnPoints spawnPoint)
    {
        // ComputeNextSpawnData();
        if (!nextSpawnPoint.isAvailable)
        {
            nextSpawnPoint = spawnPoint;
            timeSinceLastSpawn = 0;
        }
    }
}

[System.Serializable]
public class PieceType
{
    public Sprite sprite;
    public int[] edges = new int[4];         // Left-Down-Right-Up ie.- counterclockwise
}