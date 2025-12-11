using UnityEngine;
using System.Collections.Generic;
using VRHoops.Core; // כדי לגשת ל-EventBus

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

            // עוברים על כל שחקן וממקמים אותו בנקודה המתאימה בתבנית החדשה
            for (int i = 0; i < teamPlayers.Count; i++)
            {
                // מוודאים שיש נקודה מתאימה בתבנית עבור השחקן הזה
                if (i < targetFormation.childCount)
                {
                    Transform targetSpot = targetFormation.GetChild(i);

                    // העתקת מיקום ורוטציה
                    teamPlayers[i].position = targetSpot.position;
                    teamPlayers[i].rotation = targetSpot.rotation;
                }
            }

            Debug.Log($"Team moved to formation: {targetFormation.name}");
        }
    }
}