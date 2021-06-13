using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Collider2D))]
public class EnemyBase : MonoBehaviour
{
    public float health = 100;
    public float moveSpeed = 5;
    public float damageDealt = 50;
    public bool isAlive => health > 0;

    [SerializeField] private GameObject hitVfx;
    // [SerializeField] private Animation animator;

    protected bool canMove = true;

    public virtual void Init()
    {
        
    }

    protected virtual void Update()
    {
        if (isAlive && canMove)
        {
            transform.Translate(Vector3.down * (moveSpeed * Time.deltaTime));
        }
    }

    public virtual void InflictDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // animator.Stop();
        transform.DOScale(0, .3f).SetEase(Ease.OutElastic)
            .onComplete += () => Destroy(gameObject);
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Piece"))
    //     {
    //         Piece piece = other.gameObject.GetComponent<Piece>();
    //         InflictDamage(100);
    //         piece.currentCluster.InflictDamage(damageDealt, piece);
    //     }
    // }
}
