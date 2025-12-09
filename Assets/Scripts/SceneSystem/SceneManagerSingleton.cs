using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace VRHoops.SceneSystem
{
    public class SceneManagerSingleton : MonoBehaviour
    {
        public static SceneManagerSingleton Instance { get; private set; }

        private GameObject player;

        private Vector3 pendingPosition;
        private bool hasPendingPosition = false;

        public event Action OnSceneStart;
        public event Action OnSceneEnd;
        public event Action<GameObject> OnCharacterPlaced;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void RegisterPlayer(GameObject playerObj)
        {
            player = playerObj;
            DontDestroyOnLoad(player);
        }

        public void SetNextSceneSpawnPosition(Vector3 pos)
        {
            pendingPosition = pos;
            hasPendingPosition = true;
        }

        public void LoadScene(string sceneName)
        {
            OnSceneEnd?.Invoke();
            SceneManager.LoadScene(sceneName);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnSceneStart?.Invoke();

            if (player != null && hasPendingPosition)
            {
                player.transform.position = pendingPosition;
                hasPendingPosition = false;

                OnCharacterPlaced?.Invoke(player);
            }
        }
        public void RaiseSceneStart() => OnSceneStart?.Invoke();
        public void RaiseSceneEnd() => OnSceneEnd?.Invoke();
        public void RaiseCharacterPlaced(GameObject character)
            => OnCharacterPlaced?.Invoke(character);

    }
}
