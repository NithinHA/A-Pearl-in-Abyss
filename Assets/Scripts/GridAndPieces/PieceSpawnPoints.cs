using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawnPoints : MonoBehaviour
{
    public bool isAvailable = true;
    public Piece pieceHeldAtSpawner { get; private set; }

    public void SetPiece(Piece piece)
    {
        isAvailable = false;
        pieceHeldAtSpawner = piece;
    }
    
    public void RemovePiece()
    {
        isAvailable = true;
        pieceHeldAtSpawner = null;
    }
}