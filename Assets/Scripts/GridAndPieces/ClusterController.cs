using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterController : Singleton<ClusterController>
{
    [SerializeField] private UIClusterHealthBar clusterHealthBarPrefab;
    [SerializeField] private Transform healthBarsParentUI;
    [SerializeField] private List<ClusterColor> clusterDistinctionColors = new List<ClusterColor>(9);
    [Space]
    [SerializeField] private float clusterCombineMultiplier = .5f;

    public List<PieceCluster> pieceClusters = new List<PieceCluster>();

    private Camera _mainCam;

    protected override void Start()
    {
        base.Start();
        _mainCam = Camera.main;
    }

    public void AddCluster(PieceCluster cluster)
    {
        // set cluster color
        var availableClusterColor = clusterDistinctionColors.Find(item => !item.isUsed);
        availableClusterColor.isUsed = true;
        cluster.usedColor = availableClusterColor;
        foreach (var piece in cluster.clusterPieces)
            piece.PaintPiece(availableClusterColor.color);

        // set health bar
        UIClusterHealthBar healthBar = Instantiate(clusterHealthBarPrefab, healthBarsParentUI);
        healthBar.GetComponent<RectTransform>().position = _mainCam.WorldToScreenPoint(cluster.clusterPieces[0].currentCell.transform.position);
        cluster.healthBar = healthBar;
        healthBar.Init(cluster);
        pieceClusters.Add(cluster);
    }

    /// <summary>
    /// destroy associated items of cluster
    /// remove cluster from list
    /// </summary>
    public void RemoveCluster(PieceCluster cluster)
    {
        cluster.usedColor.isUsed = false;
        Destroy(cluster.healthBar.gameObject);
        pieceClusters.Remove(cluster);
    }

    public void CombineCluster(PieceCluster newCluster, List<PieceCluster> clustersToCombine)
    {
        
        float remainingClusterHealth = 100;
        foreach (var cluster in clustersToCombine)
        {
            remainingClusterHealth += cluster.strength * 100 * cluster.CurrentHealthPercent;
            newCluster.CombineCluster(cluster);
        }

        newCluster.totalHealth = newCluster.strength * 100 + (newCluster.strength - 1) * 100 * clusterCombineMultiplier;
        newCluster.remainingHealth = remainingClusterHealth + (remainingClusterHealth - 100) * clusterCombineMultiplier;
    }
    
    /// <summary>
    /// maybe called by an Action as soon as the enemy destroys a particular Piece.
    /// </summary>
    /// <param name="cluster"></param>
    public void OnClusterDestroyed(PieceCluster cluster)
    {
        foreach (var piece in cluster.clusterPieces)
            piece.RemovePieceFromCell();
        RemoveCluster(cluster);
        cluster.DestroySelf();
    }
}

[System.Serializable]
public class PieceCluster
{
    public List<Piece> clusterPieces = new List<Piece>();
    public int strength => clusterPieces.Count;
    public UIClusterHealthBar healthBar;
    
    public float totalHealth = 100;
    public float remainingHealth = 100;
    public float CurrentHealthPercent => remainingHealth / totalHealth;
    public ClusterColor usedColor;

    public PieceCluster(List<Piece> pieces)
    {
        AddPieces(pieces);
    }

    public void AddPieces(List<Piece> pieces)
    {
        foreach (var item in pieces)
        {
            this.clusterPieces.Add(item);
            item.currentCluster = this;
        }
    }

    public void CombineCluster(PieceCluster cluster)
    {
        AddPieces(cluster.clusterPieces);
        cluster.DestroySelf();
    }

    public void DestroySelf()
    {
        
    }

    public void InflictDamage(float amount, Piece inflictedPiece)
    {
        remainingHealth -= amount;
        healthBar.UpdateHealthValue(CurrentHealthPercent, remainingHealth);
        if (remainingHealth <= 0)
        {
            // KILL!
            // destroy every single piece in this
            foreach (var piece in clusterPieces)
            {
                piece.RemovePieceFromCell();
            }
            ClusterController.Instance.RemoveCluster(this);
        }
        else
        {
            inflictedPiece.AnimateDamageBounce();
        }
    }
}

[System.Serializable]
public class ClusterColor
{
    public Color color;
    public bool isUsed;
}