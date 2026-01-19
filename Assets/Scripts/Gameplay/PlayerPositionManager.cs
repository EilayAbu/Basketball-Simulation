using UnityEngine;
using System.Collections.Generic;
using VRHoops.Core;

namespace VRHoops.Gameplay
{
    public class PlayerPositionManager : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("גרור לכאן את ה-OVRCameraRig או אובייקט השחקן")]
        [SerializeField] private GameObject playerRig;

        [Header("Stands Configuration")]
        [Tooltip("רשימת נקודות (Transforms) שהשחקן יעבור ביניהן")]
        [SerializeField] private List<Transform> shootingStands;

        private void OnEnable()
        {
            // הרשמה לאירוע התחלת המשחק (כפי שביקשת)
            EventBus.OnStart += MoveToStand;

            // הרשמה לאירוע שינוי שלב (כדי לזוז תוך כדי משחק)
            EventBus.OnStageChanged += MoveToStand;
        }

        private void OnDisable()
        {
            EventBus.OnStart -= MoveToStand;
            EventBus.OnStageChanged -= MoveToStand;
        }

        // הפונקציה שמבצעת את ההזזה בפועל
        private void MoveToStand(int index)
        {
            if (playerRig == null)
            {
                Debug.LogError("PlayerPositionManager: Player Rig is missing!");
                return;
            }

            if (shootingStands == null || shootingStands.Count == 0)
            {
                Debug.LogError("PlayerPositionManager: No shooting stands defined!");
                return;
            }

            // בדיקת תקינות אינדקס
            if (index >= 0 && index < shootingStands.Count)
            {
                Transform targetTransform = shootingStands[index];

                // עדכון מיקום ורוטציה
                playerRig.transform.position = targetTransform.position;
                playerRig.transform.rotation = targetTransform.rotation;

                Debug.Log($"[PositionManager] Moved player to Stand #{index}: {targetTransform.name}");
            }
            else
            {
                Debug.LogWarning($"[PositionManager] Index {index} is out of range for shooting stands.");
            }
        }
    }
}