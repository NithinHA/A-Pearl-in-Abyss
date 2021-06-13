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
    [Space]
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;
    [SerializeField] private TextMeshProUGUI levelNameText;

    private void Start()
    {
        ScoreManager.Instance.OnPearlLose += OnPearlLose;
        LevelManager.Instance.onTick += OnTick;
        LevelManager.Instance.onLevelStateChange += OnLevelStateChange;

        levelNameText.text = "Level: " + LevelManager.Instance.levelIndex;
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.OnPearlLose -= OnPearlLose;
        LevelManager.Instance.onTick -= OnTick;
        LevelManager.Instance.onLevelStateChange -= OnLevelStateChange;
    }

    private void OnPearlLose(int count)
    {
        pearlStoreText.text = ScoreManager.Instance.RemainingPearls.ToString();
    }

    private void OnTick(float progressPercent)
    {
        levelProgress.value = progressPercent;
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
        SceneManager.LoadScene("MainMenu");
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
