using UnityEngine;
using System;

public class TrainingBall : MonoBehaviour
{
    // אירוע שמודיע למנהל שהכדור נגע ברצפה
    public event Action OnFloorHit;

    [Tooltip("התג של הרצפה")]
    [SerializeField] private string floorTag = "Floor";

    private bool hasHitFloor = false;

    private void OnCollisionEnter(Collision collision)
    {
        // מונעים דיווח כפול (כדי שהטיימר לא יתאפס בכל קפיצה קטנה על הרצפה)
        if (hasHitFloor) return;

        if (collision.gameObject.CompareTag(floorTag))
        {
            hasHitFloor = true;
            Debug.Log("Training Ball hit the floor.");

            // הפעלת האירוע
            OnFloorHit?.Invoke();
        }
    }
}