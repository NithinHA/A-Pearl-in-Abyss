using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pearlStoreText;
    [SerializeField] private Slider levelProgress;
    [SerializeField] private RectTransform waveInfoPanel;
    [SerializeField] private TextMeshProUGUI waveInfo;
    [Space]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TextMeshProUGUI levelNameText;

    private void Start()
    {
        ScoreManager.Instance.UpdatePerlData += OnUpdatePerlData;
        LevelManager.Instance.onTick += OnTick;
        LevelManager.Instance.onLevelStateChange += OnLevelStateChange;
        EnemySpawner.Instance.OnWaveStateChange += OnWaveStateChange;

        levelNameText.text = "Level: " + LevelManager.Instance.levelIndex;
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.UpdatePerlData -= OnUpdatePerlData;
        LevelManager.Instance.onTick -= OnTick;
        LevelManager.Instance.onLevelStateChange -= OnLevelStateChange;
        EnemySpawner.Instance.OnWaveStateChange -= OnWaveStateChange;
    }

    private void OnUpdatePerlData()
    {
        pearlStoreText.text = ScoreManager.Instance.RemainingPearls.ToString();
    }

    private void OnTick(float progressPercent)
    {
        levelProgress.value = progressPercent;
    }

    private void OnWaveStateChange(bool isWaveActive)
    {
        waveInfo.text = isWaveActive ? "Incoming Wave!" : "Cooldown";
        // display the panel for 2 sec and off
        waveInfoPanel.gameObject.SetActive(true);

        Sequence waveStateDisplay = DOTween.Sequence();
        waveStateDisplay.Append(waveInfoPanel.DOScaleX(1, .4f))
            .AppendInterval(1)
            .Append(waveInfoPanel.DOScaleX(0, .4f))
            .onComplete += () => waveInfoPanel.gameObject.SetActive(false);
    }

    public void OnPauseButton()
    {
        // open pause menu screen
        pauseScreen.SetActive(true);
        LevelManager.Instance.PauseGame();
    }

    public void OnResumeButton()
    {
        // close pause menu screen
        pauseScreen.SetActive(false);
        LevelManager.Instance.ResumeGame();
    }

    public void OnHomeButton()
    {
        SceneManager.LoadScene("MainMenu_1");
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnLevelStateChange(LevelState state)
    {
        if(state == LevelState.win)
        {
            winScreen.SetActive(true);
        }
        else if(state == LevelState.lose)
        {
            loseScreen.SetActive(true);
        }
    }
}
