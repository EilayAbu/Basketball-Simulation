using UnityEngine;
using VRHoops.Core;

namespace VRHoops.Reactions
{
    public class RefereeWhistleListener : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip whistleClip;

        [Header("Animation (Optional)")]
        [SerializeField] private Animator refereeAnimator;
        [SerializeField] private string whistleTrigger = "Whistle";

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
            if (evt == GameEventType.GameStart)
            {
                Debug.Log("🎺 Referee whistle triggered!");

                if (audioSource && whistleClip)
                    audioSource.PlayOneShot(whistleClip);

                if (refereeAnimator && !string.IsNullOrEmpty(whistleTrigger))
                    refereeAnimator.SetTrigger(whistleTrigger);
            }
        }
    }
}
