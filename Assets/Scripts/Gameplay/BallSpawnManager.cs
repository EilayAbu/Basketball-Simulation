using UnityEngine;
using System.Collections; // חובה בשביל Coroutine
using VRHoops.Core;
using VRHoops.Gameplay;

namespace VRHoops.Gameplay
{
    public class BallSpawnManager : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("הכדור שרוצים לייצר")]
        [SerializeField] private GameObject ballPrefab;

        [Tooltip("כמה זמן לחכות מרגע התוצאה ועד יצירת הכדור הבא")]
        [SerializeField] private float respawnDelay = 1.0f;
        private Coroutine respawnCoroutine;
        private GameObject currentBall;

        private void OnEnable()
        {
            // האזנה לאירוע תוצאת זריקה - כדי לייצר כדור חדש אחרי כל זריקה
            EventBus.OnShotResolved += HandleShotResolved;

            // האזנה לאירוע התחלת משחק - כדי לייצר את הכדור הראשון בלבד
            EventBus.OnGameEvent += HandleGameEvent;
        }

        private void OnDisable()
        {
            EventBus.OnShotResolved -= HandleShotResolved;
            EventBus.OnGameEvent -= HandleGameEvent;
        }

        // מטפל ביצירת הכדור הראשון במשחק
        private void HandleGameEvent(GameEventType evt)
        {
            if (evt == GameEventType.GameStart)
            {
                // אם כבר יש כדור (למשל אם עשינו ריסטרט), לא ניצור כפול
                if (currentBall == null)
                {
                    SpawnBall();
                }
            }
        }

        // מטפל ביצירת כדור אחרי כל זריקה
        private void HandleShotResolved(ShotResult result)
        {
            // מתחילים תהליך של יצירת כדור עם השהייה
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            yield return new WaitForSeconds(respawnDelay);
            SpawnBall();
            respawnCoroutine = null;
        }

        private void SpawnBall()
        {
            // 1. ניקוי הכדור הקודם אם קיים
            if (currentBall != null)
            {
                Destroy(currentBall);
            }

            // 2. יצירת כדור חדש במיקום של הספונואר
            currentBall = Instantiate(ballPrefab, transform.position, transform.rotation);

            // 3. אתחול הכדור
            var ballController = currentBall.GetComponent<BallController>();
            if (ballController != null)
            {
                ballController.gameObject.SetActive(true);
            }
        }
    }
}