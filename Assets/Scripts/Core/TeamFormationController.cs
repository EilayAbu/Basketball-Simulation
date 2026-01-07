using UnityEngine;
using System.Collections.Generic;
using VRHoops.Core;
using Unity.XR.CoreUtils; // כדי לגשת ל-EventBus

namespace VRHoops.Characters
{
    public class TeamFormationController : MonoBehaviour
    {
        [Header("The Real Players (NPCs)")]
        [Tooltip("גרור לכאן את המודלים של השחקנים בסצנה")]
        [SerializeField] private List<Transform> teamPlayers;

        [Header("Formations Setup")]
        [Tooltip("רשימה של אובייקטים המכילים את נקודות העמידה לכל שלב")]
        [SerializeField] private List<Transform> formationParents;
        // אינדקס 0 = עונשין, 1 = ימין, 2 = שמאל
        [SerializeField] private GameObject players;
        private void Awake()
        {
            if (formationParents == null)
            {
                teamPlayers = new List<Transform>();
            }


            foreach (Transform child in players.transform)
            {
                teamPlayers.Add(child);
            }
        }
        private void Start()
        {
            UpdatePositions(1);
        }
        private void OnEnable()
        {
            // נרשמים לאירוע שיצרנו קודם לכן (או ניצור מיד)
            EventBus.OnStageChanged += UpdatePositions;
        }

        private void OnDisable()
        {
            EventBus.OnStageChanged -= UpdatePositions;
        }

        private void UpdatePositions(int stageIndex)
        {
            // בדיקות תקינות
            if (stageIndex < 0 || stageIndex >= formationParents.Count)
            {
                Debug.LogWarning("Formation index out of range!");
                return;
            }

            Transform targetFormation = formationParents[stageIndex];
            Debug.Log($"--- Starting Layout for Formation: {targetFormation.name} ---");

            // עוברים על כל שחקן וממקמים אותו בנקודה המתאימה בתבנית החדשה
            for (int i = 0; i < teamPlayers.Count; i++)
            {
                // מוודאים שיש נקודה מתאימה בתבנית עבור השחקן הזה
                if (i < targetFormation.childCount)
                {
                    Transform targetSpot = targetFormation.GetChild(i);
                    Transform player = teamPlayers[i];

                    // --- דיבאג 1: לאן השחקן אמור להגיע ---
                    Debug.Log($"[Plan] Player '{player.name}' (Index {i}) SHOULD move to Target '{targetSpot.name}' at Position: {targetSpot.position}");

                    // העתקת מיקום ורוטציה
                    player.position = targetSpot.position;
                    player.rotation = targetSpot.rotation;

                    // --- דיבאג 2: איפה השחקן נמצא אחרי השינוי ---
                    Debug.Log($"[Result] Player '{player.name}' is NOW at Position: {player.position}");
                }
                else
                {
                    // דיבאג למקרה שאין מספיק מקומות (חשוב מאוד לבעיה שלך)
                    Debug.LogError($"[ERROR] No spot found for Player '{teamPlayers[i].name}' (Index {i})! Formation '{targetFormation.name}' only has {targetFormation.childCount} spots.");
                }
            }

            Debug.Log($"--- Finished moving team to: {targetFormation.name} ---");
        }
    }
}