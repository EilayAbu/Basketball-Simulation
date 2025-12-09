using UnityEngine;

namespace VRHoops.SceneSystem
{
    public static class SceneEventFactory
    {
        public static void TriggerSceneStart()
            => SceneManagerSingleton.Instance?.RaiseSceneStart();

        public static void TriggerSceneEnd()
            => SceneManagerSingleton.Instance?.RaiseSceneEnd();

        public static void TriggerCharacterPlaced(GameObject character)
            => SceneManagerSingleton.Instance?.RaiseCharacterPlaced(character);
    }
}
