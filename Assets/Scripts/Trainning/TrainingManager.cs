using UnityEngine;
using System.Collections;

public class TrainingManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float respawnDelay = 2.0f; // זמן המתנה בשניות

    private GameObject currentBallObj;

    private void Start()
    {
        SpawnBall();
    }

    private void SpawnBall()
    {
        // יצירת כדור חדש במיקום וברוטציה של המנהל (ה-SpawnPoint)
        currentBallObj = Instantiate(ballPrefab, transform.position, transform.rotation);

        // חיבור לסקריפט של הכדור כדי לדעת מתי הוא נוגע ברצפה
        TrainingBall ballScript = currentBallObj.GetComponent<TrainingBall>();

        if (ballScript != null)
        {
            ballScript.OnFloorHit += HandleBallHitFloor;
        }
        else
        {
            Debug.LogError("Error: The training ball prefab must have the 'TrainingBall' script attached!");
        }
    }

    private void HandleBallHitFloor()
    {
        // ברגע שהכדור נגע ברצפה, מתחילים את הספירה לאחור
        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        // מחכים את הזמן שהגדרת (למשל 2 שניות)
        yield return new WaitForSeconds(respawnDelay);

        // מחיקת הכדור הישן
        if (currentBallObj != null)
        {
            // חשוב: מנתקים את האירוע לפני המחיקה למניעת שגיאות
            var ballScript = currentBallObj.GetComponent<TrainingBall>();
            if (ballScript != null) ballScript.OnFloorHit -= HandleBallHitFloor;

            Destroy(currentBallObj);
        }

        // יצירת כדור חדש
        SpawnBall();
    }
}