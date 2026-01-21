using UnityEngine;
using VRHoops.Core;

namespace VRHoops.Gameplay
{
    public class DifficultyManager : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("מספר הקליעות שאחריהן העזרה תכבה ונעבור ללוח רגיל")]
        [SerializeField] private int shotsToSwapToNormal = 3;

        [Tooltip("מספר הקליעות (סה\"כ מתחילת הסיבוב) שאחריהן החוסם יידלק")]
        [SerializeField] private int shotsToEnableBlocker = 6;

        [Header("Backboard References")]
        [Tooltip("הלוח החכם (SmartBackboard)")]
        [SerializeField] private GameObject assistBackboard;

        [Tooltip("הלוח הרגיל (פיזיקה רגילה)")]
        [SerializeField] private GameObject normalBackboard;

        [Header("Obstacle Reference")]
        [Tooltip("האובייקט שמפריע לקלוע")]
        [SerializeField] private GameObject blockerObject;

        private int currentScoreInRound = 0;

        private void OnEnable()
        {
            EventBus.OnShotResolved += HandleShotResolved;
            EventBus.OnGameEvent += HandleGameEvent;
        }

        private void OnDisable()
        {
            EventBus.OnShotResolved -= HandleShotResolved;
            EventBus.OnGameEvent -= HandleGameEvent;
        }

        private void HandleGameEvent(GameEventType evt)
        {
            if (evt == GameEventType.GameStart)
            {
                ResetDifficulty();
            }
        }

        private void HandleShotResolved(ShotResult result)
        {
            if (result == ShotResult.Scored)
            {
                currentScoreInRound++;
                CheckDifficultyProgression();
            }
        }

        private void ResetDifficulty()
        {
            currentScoreInRound = 0;

            // מצב התחלתי:
            // 1. עזרה דולקת
            // 2. רגיל כבוי (כדי שלא יהיו כפילויות)
            // 3. חוסם כבוי
            if (assistBackboard != null) assistBackboard.SetActive(true);
            if (normalBackboard != null) normalBackboard.SetActive(false);
            if (blockerObject != null) blockerObject.SetActive(false);

            Debug.Log("DifficultyManager: Reset. Assist Mode ON.");
        }

        private void CheckDifficultyProgression()
        {
            // שלב 1: החלפה ללוח רגיל
            if (currentScoreInRound >= shotsToSwapToNormal)
            {
                // אם אנחנו עדיין במצב עזרה, נחליף לרגיל
                if (assistBackboard != null && assistBackboard.activeSelf)
                {
                    assistBackboard.SetActive(false);
                    if (normalBackboard != null) normalBackboard.SetActive(true);

                    Debug.Log($"DifficultyManager: {currentScoreInRound} shots. Switched to Normal Backboard.");
                }
            }

            // שלב 2: הפעלת החוסם (מתווסף ללוח הרגיל שכבר פועל)
            if (blockerObject != null && currentScoreInRound >= shotsToEnableBlocker)
            {
                if (!blockerObject.activeSelf)
                {
                    blockerObject.SetActive(true);
                    Debug.Log($"DifficultyManager: {currentScoreInRound} shots. Blocker Enabled!");
                }
            }
        }
    }
}