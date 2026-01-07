using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // חובה להוסיף את זה
using VRHoops.Core;
using VRHoops.SceneSystem;

namespace VRHoops.DebugTools
{
    public class KeyboardDebugController : MonoBehaviour
    {
        [Header("Scene Navigation")]
        [SerializeField] private string targetSceneName;

        // במערכת החדשה אנחנו בודקים ישירות את המקלדת
        private void Update()
        {
            if (Keyboard.current == null) return; // הגנה למקרה שאין מקלדת מחוברת

            HandleNavigationInput();
            HandleGameplayInput();
        }

        private void HandleNavigationInput()
        {
            // בדיקה אם נלחץ מקש N
            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                Debug.Log($"[Debug] Loading scene: {targetSceneName}");

                if (SceneManagerSingleton.Instance != null)
                {
                    SceneManagerSingleton.Instance.LoadScene(targetSceneName);
                }
                else
                {
                    SceneManager.LoadScene(targetSceneName);
                }
            }
        }

        private void HandleGameplayInput()
        {
            if (GameManager.Instance == null) return;

            // S = Scored (סל)
            if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                Debug.Log("[Debug] Simulating: Player Scored");
                GameManager.Instance.OnBallScored();
            }

            // M = Missed (החטאה)
            if (Keyboard.current.mKey.wasPressedThisFrame)
            {
                Debug.Log("[Debug] Simulating: Player Missed");
                GameManager.Instance.OnBallMissed();
            }

            // R = Restart Round (סיבוב חדש)
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                Debug.Log("[Debug] Simulating: Force New Round");
                GameManager.Instance.StartNewRound();
            }
        }
    }
}