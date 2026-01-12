using System;

namespace VRHoops.Core
{
    [Serializable]
    public class GameModel
    {
        // הגדרות
        public float GameDuration;

        // מצב נוכחי
        public int ScoreHome { get; private set; } // השחקן שלנו (A)
        public int ScoreGuest { get; private set; } // יריב/מחשב (B)
        public float TimeRemaining { get; private set; }

        // נתונים נוספים לניסוי
        public int ShotsInCurrentStage { get; private set; }
        public int CurrentStageIndex { get; private set; }
        public int ThrowsPerStage;
        public int TotalStages;

        public GameModel(int throwsPerStage, int totalStages, float duration)
        {
            ThrowsPerStage = throwsPerStage;
            TotalStages = totalStages;
            GameDuration = duration;
            Reset();
        }

        public void Reset()
        {
            // כאן אתה יכול להגדיר ניקוד התחלתי אם תרצה (למשל 50-60)
            ScoreHome = 0;
            ScoreGuest = 0;

            ShotsInCurrentStage = 0;
            CurrentStageIndex = 0;
            TimeRemaining = GameDuration;
        }

        public void AddScore(int homePoints, int guestPoints)
        {
            ScoreHome += homePoints;
            ScoreGuest += guestPoints;
        }

        public void TickTime(float deltaTime)
        {
            TimeRemaining -= deltaTime;
            if (TimeRemaining < 0) TimeRemaining = 0;
        }

        // ... שאר הפונקציות (RegisterShot וכו') נשארות דומות ...
        public void RegisterShot() { ShotsInCurrentStage++; }
        public bool ShouldAdvance() => ShotsInCurrentStage >= ThrowsPerStage;
        public bool NextStage()
        {
            ShotsInCurrentStage = 0;
            CurrentStageIndex++;
            return CurrentStageIndex < TotalStages;
        }
    }
}