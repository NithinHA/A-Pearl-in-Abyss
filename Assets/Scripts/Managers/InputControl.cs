using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputControl : Singleton<InputControl>
{
    [SerializeField] private float minThresholdDistToSnap = 1;
    
    private Piece selectedPieceItem;
    private Camera _mainCam;

    /// <summary>
    /// Called when "Piece" in placed in "Cell" from "Spawner".
    /// </summary>
    public static Action<Piece, Cell, PieceSpawnPoints> OnPiecePlaced;

    protected override void Start()
    {
        _mainCam = Camera.main;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            selectedPieceItem = FindNearestPiece((Vector2)_mainCam.ScreenToWorldPoint(Input.mousePosition));
            if (selectedPieceItem)
            {
                selectedPieceItem.sortGroup.sortingOrder = 2;
                selectedPieceItem.Pick();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (selectedPieceItem)
                selectedPieceItem.transform.position = (Vector2) _mainCam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(!selectedPieceItem) return;
            if (Grid.Instance.TryPlace(selectedPieceItem, (Vector2) _mainCam.ScreenToWorldPoint(Input.mousePosition)))
            {
                // selectedPieceItem.ResetOriginalTransforms();
                selectedPieceItem.sortGroup.sortingOrder = 0;
                // PuzzleExperimentManager.Instance.CheckPuzzleCompletion();
            }
            else
            {
                // selectedPieceItem.ResetRandomisedTransforms();            // Enable this if you want pieces to return back to their original places when does not fit.
                selectedPieceItem.sortGroup.sortingOrder = 1;
                selectedPieceItem.Drop();
            }
            selectedPieceItem = null;
        }
//#elif UNITY_ANDROID
        // Touch touch = Input.GetTouch(0);
        // if (touch.phase == TouchPhase.Began)
        // {
        //     selectedPieceItem = FindNearestPiece(_mainCam.ScreenToWorldPoint(touch.position));
        //     if(selectedPieceItem) selectedPieceItem.PieceGroup.sortingOrder = 1;
        // }
        // else if (touch.phase == TouchPhase.Moved)
        // {
        //     if (selectedPieceItem)
        //         selectedPieceItem.transform.position = (Vector2) _mainCam.ScreenToWorldPoint(touch.position);
        // }
        // else if (touch.phase == TouchPhase.Ended)
        // {
        //     if(!selectedPieceItem) return;
        //     if (selectedPieceItem.TryPiece())
        //     {
        //         selectedPieceItem.ResetOriginalTransforms();
        //         PuzzleExperimentManager.Instance.CheckPuzzleCompletion();
        //     }
        //     else
        //     {
        //         // selectedPieceItem.ResetRandomisedTransforms();            // Enable this if you want pieces to return back to their original places when does not fit.
        //     }
        //     selectedPieceItem.PieceGroup.sortingOrder = 0;
        //     selectedPieceItem = null;
        // }
#endif
    }

    private Piece FindNearestPiece(Vector2 cursorPosition)
    {
        float distToNearestSP = float.MaxValue;
        PieceSpawnPoints nearestSP = null;
        foreach (var spawner in PieceSpawner.Instance.spawnPoints)
        {
            float distToThisSP = Vector2.Distance(spawner.transform.position, cursorPosition);              // Need to optimize here! Use square magnitude instead.
            if (distToThisSP < distToNearestSP)
            {
                distToNearestSP = distToThisSP;
                nearestSP = spawner;
            }
        }

        if (nearestSP == null)
            return null;

        return distToNearestSP < minThresholdDistToSnap ? nearestSP.pieceHeldAtSpawner : null;
    }

    private void OnDrawGizmos()
    {
        if (_mainCam == null)
            return;
        
        Vector2 cursorPos = _mainCam.ScreenToWorldPoint(Input.mousePosition);
        
        Gizmos.color = Color.HSVToRGB(.13f, .68f, .96f);
        foreach (var cell in Grid.Instance.cells)
        {
            Gizmos.DrawLine(cursorPos, cell.transform.position);
        }

        Gizmos.color = Color.HSVToRGB(.07f, .80f, .98f);
        foreach (var spawner in PieceSpawner.Instance.spawnPoints)
        {
            Gizmos.DrawLine(cursorPos, spawner.transform.position);
        }
    }
}