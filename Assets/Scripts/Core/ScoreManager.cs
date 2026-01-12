using UnityEngine;
using System.Collections.Generic;
using VRHoops.UI; // כדי לגשת ל-ScoreboardController
using VRHoops.Core;

namespace VRHoops.Core
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("גרור לכאן את הסקורבורד מהסצנה")]
        [SerializeField] private ScoreboardController scoreboard;

        [Header("Scoring Rules")]
        [Tooltip("רשימת הניקוד לפי שלבים. אינדקס 0 הוא הניקוד לשלב הראשון, אינדקס 1 לשלב השני וכו")]
        [SerializeField] private List<int> pointsPerStage = new List<int> { 1, 2, 3, 3, 5 };
        // דוגמה: בשלב 0 מקבלים נקודה, בשלב 1 מקבלים 2 נקודות וכו'

        // משתנה פרטי למעקב אחר השלב הנוכחי
        private int currentStageIndex = 0;

        private void OnEnable()
        {
            // הרשמה לאירועים
            EventBus.OnShotResolved += HandleShotResult;
            EventBus.OnStageChanged += HandleStageChanged;
        }

        private void OnDisable()
        {
            // ביטול הרשמה (חשוב מאוד למנוע שגיאות)
            EventBus.OnShotResolved -= HandleShotResult;
            EventBus.OnStageChanged -= HandleStageChanged;
        }

        // פונקציה שמופעלת כשהשלב משתנה
        private void HandleStageChanged(int newStageIndex)
        {
            currentStageIndex = newStageIndex;
            Debug.Log($"ScoreManager: Stage updated to {currentStageIndex}. Points for this stage: {GetPointsForCurrentStage()}");
        }

        // פונקציה שמופעלת כשיש תוצאת זריקה
        private void HandleShotResult(ShotResult result)
        {
            // אנחנו מעלים ניקוד רק אם הייתה קליעה (Scored)
            if (result == ShotResult.Scored)
            {
                int pointsToAdd = GetPointsForCurrentStage();

                // עדכון הלוח הגרפי
                if (scoreboard != null)
                {
                    scoreboard.AddScoreTeamA(pointsToAdd);
                    Debug.Log($"ScoreManager: Scored! Added {pointsToAdd} points.");
                }
                else
                {
                    Debug.LogError("ScoreManager: Scoreboard reference is missing!");
                }
            }
        }

        // פונקציית עזר למציאת הניקוד לפי האינדקס הנוכחי
        private int GetPointsForCurrentStage()
        {
            // בדיקת תקינות כדי שלא נקרוס אם השלב חורג מגודל הרשימה
            if (currentStageIndex >= 0 && currentStageIndex < pointsPerStage.Count)
            {
                return pointsPerStage[currentStageIndex];
            }

            // ברירת מחדל אם אין הגדרה לשלב הזה
            Debug.LogWarning($"ScoreManager: No points defined for stage {currentStageIndex}, using default (1).");
            return 1;
        }
    }
}