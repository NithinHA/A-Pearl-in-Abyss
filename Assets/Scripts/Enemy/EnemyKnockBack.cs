using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class EnemyKnockBack : EnemyBase
{
    [Header("Knockbackable enemy")] 
    [SerializeField] private float knockBackAmount; 
    
    public override void InflictDamage(float amount)
    {
        base.InflictDamage(amount);
        if(isAlive)
            KnockBack();
    }

    private void KnockBack()
    {
        Vector3 knockbackPos = transform.localPosition + Vector3.up * knockBackAmount;
        canMove = false;
        transform.DOLocalMove(knockbackPos, .1f).SetEase(Ease.OutSine)
            .onComplete += () => canMove = true;
    }
}
