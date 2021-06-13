using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIClusterHealthBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthAmountText;
    private Slider slider;

    public void Init(PieceCluster cluster)
    {
        slider = GetComponent<Slider>();
        healthAmountText.text = cluster.remainingHealth.ToString();
        slider.value = cluster.CurrentHealthPercent;
    }
    
    public void UpdateHealthValue(float percent, float value)
    {
        slider.value = percent;
        healthAmountText.text = value.ToString();
    }
    
}
