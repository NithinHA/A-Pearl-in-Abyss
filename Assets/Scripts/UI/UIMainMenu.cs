using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject infoPanel;

    bool isLevelSelectActive = false;

    public void OnTogglePlayButton()
    {
        if (!isLevelSelectActive)
        {
            isLevelSelectActive = true;
            levelSelectPanel.SetActive(true);
            levelSelectPanel.transform.DOScaleY(1, .25f).SetEase(Ease.OutBack);
        }
        else
        {
            isLevelSelectActive = false;
            levelSelectPanel.transform.DOScaleY(0, .25f).SetEase(Ease.Linear)
                .onComplete += () => levelSelectPanel.SetActive(false);
        }
    }

    public void OnLevelSelect(int index)
    {
        GameManager.Instance.LoadLevel(index);
    }

    public void ToggleInfoPanel(bool active)
    {
        infoPanel.SetActive(active);
    }
}
