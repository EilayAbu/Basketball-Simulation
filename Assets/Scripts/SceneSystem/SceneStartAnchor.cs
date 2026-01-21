using UnityEngine;

namespace VRHoops.SceneSystem
{
    public class SceneStartAnchor : MonoBehaviour
    {
        private void Start()
        {
            
            SceneEventFactory.TriggerSceneStart();
        }
    }
}
