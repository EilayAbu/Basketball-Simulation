using UnityEngine;
using TMPro; // חובה בשביל TextMeshPro

namespace VRHoops.UI
{
    public class ScoreboardController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] public TextMeshProUGUI timerText;
        [SerializeField] public TextMeshProUGUI scoreTextA; // Home
        [SerializeField] public TextMeshProUGUI scoreTextB; // Guest/Away

        [Header("Settings")]
        [Tooltip("זמן המשחק בשניות")]
        [SerializeField] private float gameDuration = 60f;

        // משתנים פרטיים לניהול מצב
        [SerializeField] private float currentTime;
         private int scoreA = 0;
         private int scoreB = 0;
        [SerializeField] private int resetScoreA , resetScoreB;
        
        private bool isTimerRunning = false;

        private void Start()
        {
            
            ResetGame();
        }

        private void Update()
        {
            if (isTimerRunning)
            {
                currentTime -= Time.deltaTime;

                if (currentTime <= 0)
                {
                    currentTime = 0;
                    EndGame();
                }

                UpdateTimerDisplay();
            }
        }

        // --- פעולות לוגיות ---

        public void AddScoreTeamA(int points)
        {
            scoreA += points;
            scoreTextA.text = scoreA.ToString("00"); // פורמט דו-ספרתי
        }

        public void AddScoreTeamB(int points)
        {
            scoreB += points;
            scoreTextB.text = scoreB.ToString("00");
        }

        public void StartTimer()
        {
            isTimerRunning = true;
        }

        public void StopTimer()
        {
            isTimerRunning = false;
        }

        public void ResetGame()
        {
            scoreA = resetScoreA;
            scoreB = resetScoreB;
            currentTime = gameDuration;

            // איפוס טקסטים
            AddScoreTeamA(0);
            AddScoreTeamB(0);
            UpdateTimerDisplay();
            isTimerRunning = false; // מחכה לפקודת התחלה
        }

        // --- פונקציות עזר ---

        private void UpdateTimerDisplay()
        {
            // המרה לדקות ושניות (למשל 65 שניות יציג 01:05)
            int minutes = Mathf.FloorToInt(currentTime / 60F);
            int seconds = Mathf.FloorToInt(currentTime % 60F);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        private void EndGame()
        {
            isTimerRunning = false;
            Debug.Log("Game Over!");
            // כאן אפשר להוסיף קריאה ל-EventBus למשל:
            // VRHoops.Core.EventBus.PublishGameEvent(GameEventType.GameEnded);
        }
    }
}