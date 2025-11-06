using UnityEngine;
using VRHoops.Core;

namespace VRHoops.Reactions
{
    public class CrowdReactionListener : MonoBehaviour
    {
        [Header("Animation")]
        [SerializeField] private Animator animator;
        [SerializeField] private string cheerTrigger = "Cheer";
        [SerializeField] private string booTrigger = "Boo";

        [Header("Audio (optional)")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip cheerClip;
        [SerializeField] private AudioClip booClip;

        private void OnEnable()
        {
            EventBus.OnGameEvent += OnGameEvent;
        }

        private void OnDisable()
        {
            EventBus.OnGameEvent -= OnGameEvent;
        }

        private void OnGameEvent(GameEventType evt)
        {
            switch (evt)
            {
                case GameEventType.CrowdCheer:
                    if (animator && !string.IsNullOrEmpty(cheerTrigger)) animator.SetTrigger(cheerTrigger);
                    if (audioSource && cheerClip) audioSource.PlayOneShot(cheerClip);
                    break;

                case GameEventType.CrowdBoo:
                    if (animator && !string.IsNullOrEmpty(booTrigger)) animator.SetTrigger(booTrigger);
                    if (audioSource && booClip) audioSource.PlayOneShot(booClip);
                    break;
            }
        }
    }
}
