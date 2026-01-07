using UnityEngine;
using UnityEngine.UI;
using TMPro; // חובה ל-Text Mesh Pro
using VRHoops.UI; // כדי לגשת ל-ScoreboardController

namespace VRHoops.EditorTools
{
    // ניתן לשים את הסקריפט על אובייקט ריק בסצנה רק בשביל הבנייה
    public class ScoreboardBuilder : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Vector2 boardSize = new Vector2(400, 250);
        [SerializeField] private Color boardColor = Color.black;
        [SerializeField] private Color teamAColor = Color.red;
        [SerializeField] private Color teamBColor = Color.yellow;
        [SerializeField] private Color timerColor = Color.green;

        // הפונקציה הזו מוסיפה כפתור לתפריט של הקומפוננטה ביוניטי
        [ContextMenu("Build Scoreboard Now")]
        public void BuildScoreboard()
        {
            // 1. יצירת האובייקט הראשי (Canvas)
            GameObject rootObj = new GameObject("Auto_Scoreboard");
            Canvas canvas = rootObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            // הגדרה לגודל מתאים ל-VR
            rootObj.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
            rootObj.transform.position = new Vector3(0, 2, 5); // שם אותו מול המצלמה

            // 2. יצירת הרקע
            GameObject bgObj = new GameObject("Background");
            bgObj.transform.SetParent(rootObj.transform, false);
            Image bgImage = bgObj.AddComponent<Image>();
            bgImage.color = boardColor;
            bgImage.rectTransform.sizeDelta = boardSize;

            // 3. הוספת הסקריפט הלוגי (Controller)
            ScoreboardController controller = rootObj.AddComponent<ScoreboardController>();

            // 4. יצירת הטקסטים
            // --- כותרות ---
            CreateText(rootObj, "Header_Home", "HOME", new Vector2(-100, 80), 40, Color.white);
            CreateText(rootObj, "Header_Guest", "GUEST", new Vector2(100, 80), 40, Color.white);

            // --- ניקוד ---
            TextMeshProUGUI txtScoreA = CreateText(rootObj, "Score_A", "00", new Vector2(-100, 10), 100, teamAColor);
            TextMeshProUGUI txtScoreB = CreateText(rootObj, "Score_B", "00", new Vector2(100, 10), 100, teamBColor);

            // --- שעון ---
            CreateText(rootObj, "Label_Period", "PERIOD", new Vector2(0, -50), 20, Color.white);
            TextMeshProUGUI txtTimer = CreateText(rootObj, "Timer", "00:00", new Vector2(0, -80), 60, timerColor);

            // 5. חיבור הכל ל-Controller באופן אוטומטי (Reflection-like)
            // אנחנו מניחים שהשדות ב-ScoreboardController הם Public או SerializeField
            // דרך סקריפט עורך זה קצת מורכב, אבל כאן נעשה השמה ישירה אם השדות פתוחים,
            // או שנשתמש בשיטה פשוטה של השמה לזיכרון.

            // הערה: כדי שהשורות הבאות יעבדו, וודא שהמשתנים ב-ScoreboardController הם Public
            // או שיש להם Properties. אם הם Private SerializeField, תצטרך לגרור ידנית או לשנות אותם ל-Public זמנית.

            // גישה ישירה (עובד אם הפכת אותם ל-Public בקובץ ScoreboardController)
            controller.timerText = txtTimer;
            controller.scoreTextA = txtScoreA;
            controller.scoreTextB = txtScoreB;

            Debug.Log("Scoreboard created successfully!");
        }

        // פונקציית עזר ליצירת טקסט
        private TextMeshProUGUI CreateText(GameObject parent, string name, string content, Vector2 position, float fontSize, Color color)
        {
            GameObject textObj = new GameObject(name);
            textObj.transform.SetParent(parent.transform, false);

            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = content;
            tmp.fontSize = fontSize;
            tmp.color = color;
            tmp.alignment = TextAlignmentOptions.Center;

            // הגדרת גודל התיבה
            tmp.rectTransform.sizeDelta = new Vector2(200, 150);
            tmp.rectTransform.anchoredPosition = position;

            return tmp;
        }
    }
}