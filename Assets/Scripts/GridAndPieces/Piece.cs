using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Collider2D))]
public class Piece : MonoBehaviour
{
    public bool isPlaced;
    [SerializeField] private SpriteRenderer jigsawPiece;
    [SerializeField] private SpriteRenderer overlayCloud;
    // [SerializeField] private Color[] attrColors = new[] {Color.green, Color.yellow, Color.red, Color.cyan};
    [Space]
    [SerializeField] private float jigsawSizeInSlot = .8f;
    [SerializeField] private float jigsawSizeInCell = 1.25f;
    [Space] 
    [SerializeField] private float waitTimeInSlot = 10;
    private float timeSinceSpawn;
    private bool isMoving;

    public SortingGroup sortGroup { get; private set; }
    public PieceCluster currentCluster { get; set; }

    private int[] pieceEdges = new int[4];        // L-D-R-U
    public int _L => pieceEdges[0];
    public int _D => pieceEdges[1];
    public int _R => pieceEdges[2];
    public int _U => pieceEdges[3];
    public PieceSpawnPoints spawnPoint { get; set; }
    public Cell currentCell { get; set; }

    [Space]
    [SerializeField] private Color onDamageCloudColor = Color.red;
    private Color normalCloudColor;
    [SerializeField] private GameObject pieceDestroyFx;
    
    private void Start()
    {
        sortGroup = GetComponent<SortingGroup>();
        normalCloudColor = overlayCloud.color;
    }

    protected virtual void Update()
    {
        if (isPlaced)
        {
            // defends the place
            // or
            // attack enemies
        }
        else if (!isMoving)
        {
            timeSinceSpawn += Time.deltaTime;
            if (timeSinceSpawn >= waitTimeInSlot)
                RemovePieceFromSlot();
        }
    }

    public void Init(PieceType type, int orientation, PieceSpawnPoints spawnPoint)
    {
        sortGroup = GetComponent<SortingGroup>();
        this.spawnPoint = spawnPoint;
        
        overlayCloud.gameObject.SetActive(false);
        jigsawPiece.sprite = type.sprite;

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, .4f).SetEase(Ease.InBack);

        Transform jigsawPieceT = jigsawPiece.transform;
        jigsawPieceT.rotation = Quaternion.Euler(new Vector3(0, 0, orientation));
        jigsawPieceT.localScale = new Vector2(jigsawSizeInSlot, jigsawSizeInSlot);

        GetComponent<Collider2D>().enabled = false;
        
        int normalisedOffset = orientation / 90;
        for (int i = 0; i < pieceEdges.Length; i++)
        {
            int edgeIndex = i - normalisedOffset;
            edgeIndex = edgeIndex < 0 ? edgeIndex + pieceEdges.Length : edgeIndex >= pieceEdges.Length ? edgeIndex - pieceEdges.Length : edgeIndex;
            pieceEdges[i] = type.edges[edgeIndex];
        }
    }

    public void Pick()
    {
        // if (isPlaced)
        //     return false;        // need to change this if we're implementing drifters
        transform.parent = null;
        jigsawPiece.transform.DOScale(jigsawSizeInCell, .05f).SetEase(Ease.InBounce);
        isMoving = true;
    }

    /// <summary>
    /// need to check again for additional functionality.
    /// </summary>
    public void Place(Cell cell)
    {
        isPlaced = true;
        isMoving = false;
        
        cell.piece = this;
        var pieceT = transform;
        pieceT.parent = cell.transform;
        pieceT.DOLocalMove(Vector3.zero, .2f).SetEase(Ease.Linear);

        GetComponent<Collider2D>().enabled = true;
        overlayCloud.gameObject.SetActive(true);
        
        spawnPoint.RemovePiece();
        InputControl.OnPiecePlaced?.Invoke(this, cell, spawnPoint);
        spawnPoint = null;
        currentCell = cell;
    }

    public void Drop()
    {
        isMoving = false;
        
        transform.parent = spawnPoint.transform;
        transform.DOLocalMove(Vector3.zero, .2f).SetEase(Ease.InFlash);
        jigsawPiece.transform.DOScale(jigsawSizeInSlot, .05f).SetEase(Ease.OutBounce);
    }

    protected virtual void RemovePieceFromSlot()
    {
        spawnPoint.RemovePiece();
        transform.DOScale(Vector3.zero, .4f).SetEase(Ease.OutBack)
            .onComplete = () => Destroy(this.gameObject);
    }

    public virtual void RemovePieceFromCell()
    {
        currentCell.piece = null;
        Instantiate(pieceDestroyFx, transform.position, Quaternion.identity);
        overlayCloud.gameObject.SetActive(false);
        jigsawPiece.DOColor(Color.clear, .3f)
            .onComplete += () => Destroy(this.gameObject);
    }

    public void PaintPiece(Color color)
    {
        jigsawPiece.color = color;
    }

    public virtual void Attack()
    {
        
    }

    /// <summary>
    /// Plays squish animation
    /// </summary>
    public void AnimateDamageBounce()
    {
        transform.DOScaleY(.7f, .2f)
            .onComplete += () => transform.DOScaleY(1f, .2f);
        overlayCloud.DOColor(onDamageCloudColor, .2f)
            .onComplete += () => overlayCloud.DOColor(normalCloudColor, .2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyBase enemy = other.gameObject.GetComponent<EnemyBase>();
            enemy.InflictDamage(100);
            currentCluster.InflictDamage(enemy.damageDealt, this);
        }
    }

}
