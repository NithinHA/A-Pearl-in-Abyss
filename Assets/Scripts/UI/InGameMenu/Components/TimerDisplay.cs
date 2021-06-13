using System.Collections;
using System.Collections.Generic;
// using FPop.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace FPop.UI
{
    public class TimerDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerText;
        public static float GameTime = 0;

        void Start()
        {
            // GameTime = LevelManager.Instance.TotalLevelTimer;
            timerText.text = ConvertToTime(GameTime);
        }
        
        void Update()
        {
            // if (GameManager.Instance.gameState != GameManager.GameState.Running || !LevelManager.Instance.HasRunBegun) return;
            if ((int) GameTime < 0) GameTime = 0;
            else if (GameTime <= 0) return;
            GameTime -= Time.unscaledDeltaTime;
            timerText.text = ConvertToTime(GameTime);
        }

        public static string ConvertToTime(float seconds = -1)
        {
            if (seconds == -1)
                seconds = GameTime;
            return $"{(int) (seconds / 60):00} : {(int) (seconds % 60):00}";
        }
    }
}