using UnityEngine;

namespace VRHoops.SceneSystem
{
    public class SceneAutoEnd : MonoBehaviour
    {
        [SerializeField] private float delay = 5f;
        [SerializeField] private string nextScene;

        private void Start()
        {
            Invoke(nameof(EndScene), delay);
        }

        private void EndScene()
        {
            SceneEventFactory.TriggerSceneEnd();
            SceneManagerSingleton.Instance.LoadScene(nextScene);
        }
    }
}
