using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    public int levelIndex;
    public LevelState currentLevelState;
    public Action<LevelState> onLevelStateChange;

    public float levelTotalTime = 120;
    public float passedTime = 0;
    public Action<float> onTick;

    private void Awake()
    {
        ScoreManager.Instance.OnPearlLose += OnPearlLoseGameStateCheck;
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.OnPearlLose -= OnPearlLoseGameStateCheck;
    }

    private void Start()
    {
        currentLevelState = LevelState.running;
        StartCoroutine(StartLevelTimer());
    }

    private IEnumerator StartLevelTimer()
    {
        while(passedTime < levelTotalTime)
        {
            if (currentLevelState == LevelState.running)
            {
                passedTime += 1;
                float progressPercent = passedTime / levelTotalTime;
                onTick?.Invoke(progressPercent);
            }
            yield return new WaitForSeconds(1);
        }

        WinGame();
    }

    private void OnPearlLoseGameStateCheck(int loseCount)
    {
        if(ScoreManager.Instance.RemainingPearls <= 0)
        {
            LoseGame();
        }
    }

    private void WinGame()
    {
        currentLevelState = LevelState.win;
        onLevelStateChange?.Invoke(currentLevelState);
    }

    private void LoseGame()
    {
        currentLevelState = LevelState.lose;
        onLevelStateChange?.Invoke(currentLevelState);
    }

    public void PauseGame()
    {
        currentLevelState = LevelState.paused;
        onLevelStateChange?.Invoke(currentLevelState);
    }

    public void ResumeGame()
    {
        currentLevelState = LevelState.running;
        onLevelStateChange?.Invoke(currentLevelState);
    }
}

public enum LevelState
{
    running, paused, win, lose
}