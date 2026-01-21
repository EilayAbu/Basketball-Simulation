using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace VRHoops.SceneSystem
{
    public class SceneManagerSingleton : MonoBehaviour
    {
        public static SceneManagerSingleton Instance { get; private set; }

        // אירועים למערכת (למשל ל-Fade או סאונד)
        public event Action OnSceneStart;
        public event Action OnSceneEnd;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // שומר רק את המנהל עצמו
        }

        public void LoadScene(string sceneName)
        {
            // הודעה שהסצינה הנוכחית מסתיימת
            OnSceneEnd?.Invoke();

            // טעינת הסצינה הבאה
            SceneManager.LoadScene(sceneName);
        }

        // פונקציות להפעלת האירועים מבחוץ (למשל מ-SceneStartAnchor)
        public void RaiseSceneStart() => OnSceneStart?.Invoke();
        public void RaiseSceneEnd() => OnSceneEnd?.Invoke();
    }
}