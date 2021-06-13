using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isOccupied => piece != null;
    public Cell[] neighbors = new Cell[4];          // 0:left, 1:down, 2:right, 3:up
    public Piece piece;
}
