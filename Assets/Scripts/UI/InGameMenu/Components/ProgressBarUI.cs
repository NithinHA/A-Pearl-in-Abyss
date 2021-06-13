using System.Collections;
using System.Collections.Generic;
// using FPop.Platform;
using UnityEngine;
using UnityEngine.UI;

namespace FPop.UI
{
    public class ProgressBarUI : MonoBehaviour
    {
        public float LevelProgress { get; private set; }
        public float RemainingDist => totalDist - distCovered;

        [SerializeField] private Slider progressionSlider;

        private float startPos, endPos, totalDist, distCovered;
        private Transform playerTransform;
        
        void Start()
        {
            // playerTransform = FindObjectOfType<PlayerControl>().transform;
            startPos = playerTransform.position.y;
            // endPos = FindObjectOfType<LastPlatform>().transform.position.y;
            totalDist = endPos - startPos;
        }

        void Update()
        {
            distCovered = playerTransform.position.y - startPos;
            LevelProgress = distCovered / totalDist;
            progressionSlider.value = LevelProgress;
        }
    }
}