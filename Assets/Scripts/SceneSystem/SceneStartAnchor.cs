using UnityEngine;

namespace VRHoops.SceneSystem
{
    public class SceneStartAnchor : MonoBehaviour
    {
        private void Start()
        {
            SceneManagerSingleton.Instance.SetNextSceneSpawnPosition(transform.position);
            SceneEventFactory.TriggerSceneStart();
        }
    }
}
