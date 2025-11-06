using UnityEngine;

namespace VRHoops.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Config")]
        [Tooltip("למניעת ספירה כפולה של סל (שבריר שנייה הגנה).")]
        [SerializeField] private float basketCooldownSeconds = 0.5f;

        private float _lastBasketTime = -999f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // נקרא ע״י HoopScoreDetector כשהכדור נקלע/הוחטא
        public void ResolveShot(ShotResult result)
        {
            // הגנה בסיסית מספירה כפולה רציפה
            if (result == ShotResult.Scored)
            {
                if (Time.unscaledTime - _lastBasketTime < basketCooldownSeconds)
                    return;

                _lastBasketTime = Time.unscaledTime;
            }

            // קודם מפיצים את תוצאת הזריקה הגולמית
            EventBus.PublishShotResult(result);

            // אחר כך מפיקים ומפיצים אירועי תגובה (קהל/שחקנים/אודיו)
            foreach (var evt in EventFactory.Build(result))
            {
                EventBus.PublishGameEvent(evt);
            }
        }
    }
}
