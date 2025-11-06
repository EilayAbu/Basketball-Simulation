using UnityEngine;
using VRHoops.Core;

namespace VRHoops.Reactions
{
    public class AudioReactionListener : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxBus;
        [SerializeField] private AudioClip crowdCheerBig;
        [SerializeField] private AudioClip crowdBooBig;

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
                    if (sfxBus && crowdCheerBig) sfxBus.PlayOneShot(crowdCheerBig);
                    break;
                case GameEventType.CrowdBoo:
                    if (sfxBus && crowdBooBig) sfxBus.PlayOneShot(crowdBooBig);
                    break;
            }
        }
    }
}
