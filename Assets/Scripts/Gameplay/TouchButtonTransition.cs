using UnityEngine;
using VRHoops.SceneSystem;

public class TouchButtonTransition : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("שם הסצינה שרוצים לטעון")]
    [SerializeField] private string nextSceneName;

    [Tooltip("תג היד/שחקן (למשל Player)")]
    [SerializeField] private string triggerTag = "Player";

    [Tooltip("כמה זמן לחכות מרגע הנגיעה ועד המעבר")]
    [SerializeField] private float delay = 0.5f;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        // אם כבר נגעו, לא לעשות כלום
        if (isTriggered) return;
        isTriggered = true;
        Debug.Log($"Button touched by {other.name}. Loading {nextSceneName} in {delay}s...");

        // טעינת הסצינה אחרי ההשהיה
        Invoke(nameof(LoadScene), delay);
        // בדיקה שמי שנגע זה השחקן
        if (other.CompareTag(triggerTag))
        {
            
        }
    }

    private void LoadScene()
    {
        if (SceneManagerSingleton.Instance != null)
        {
            SceneManagerSingleton.Instance.LoadScene(nextSceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}