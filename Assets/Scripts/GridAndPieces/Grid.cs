using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : Singleton<Grid>
{
    public List<Cell> cells = new List<Cell>(15);
    [SerializeField] private float minThresholdDistToSnap = 1f;

    public bool TryPlace(Piece piece, Vector3 pos)
    {
        // check for closest cell
        Cell nearestCell = FindNearestCell(pos);
        if (nearestCell != null)
        {
            if (nearestCell.isOccupied)
                return false;
            
            Piece leftPiece = GetNeighbouringPiece(nearestCell, 0);
            Piece downPiece = GetNeighbouringPiece(nearestCell, 1);
            Piece rightPiece = GetNeighbouringPiece(nearestCell, 2);
            Piece upPiece = GetNeighbouringPiece(nearestCell, 3);
            
            if (leftPiece != null)
                if (leftPiece._R + piece._L != 0)
                    return false;
            if(downPiece != null)
                if (downPiece._U + piece._D != 0)
                    return false;
            if(rightPiece != null)
                if (rightPiece._L + piece._R != 0)
                    return false;
            if(upPiece != null)
                if (upPiece._D + piece._U != 0)
                    return false;

            PlacePiece(piece, nearestCell);
            return true;
        }

        return false;
    }

    private Cell FindNearestCell(Vector3 pos)
    {
        float distToNearestCell = float.MaxValue;
        Cell nearestCell = null;
        foreach (var cell in cells)
        {
            float distToThisCell = Vector2.Distance(cell.transform.position, pos);              // Need to optimize here! Use square magnitude instead.
            if (distToThisCell < distToNearestCell)
            {
                distToNearestCell = distToThisCell;
                nearestCell = cell;
            }
        }

        return distToNearestCell < minThresholdDistToSnap ? nearestCell : null;
    }

    private void PlacePiece(Piece piece, Cell cell)
    {
        piece.Place(cell);
        PieceCluster newCluster = new PieceCluster(new List<Piece>() {piece});

        List<PieceCluster> clustersToCombine = new List<PieceCluster>();
        for (int i = 0; i < 4; i++)
        {
            Piece pieceItem = GetNeighbouringPiece(cell, i);
            if (pieceItem != null)
            {
                if (pieceItem.currentCluster != newCluster)
                {
                    var oldCluster = pieceItem.currentCluster;
                    ClusterController.Instance.RemoveCluster(oldCluster);
                    // newCluster.CombineCluster(oldCluster);
                    clustersToCombine.Add(oldCluster);
                }
            }
        }

        ClusterController.Instance.CombineCluster(newCluster, clustersToCombine);
        ClusterController.Instance.AddCluster(newCluster);
        // ClusterController.Instance.RefactorAllClusters();
    }

    /// <summary>
    ///  Direction -> L-D-R-U
    /// </summary>
    private Piece GetNeighbouringPiece(Cell cell, int direction)
    {
        Cell neighbouringCell = cell.neighbors[direction];
        return neighbouringCell?.piece;
    }
}


