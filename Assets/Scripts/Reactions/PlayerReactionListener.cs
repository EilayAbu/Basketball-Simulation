using UnityEngine;
using VRHoops.Core;

namespace VRHoops.Reactions
{
    public class PlayerReactionListener : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string celebrateTrigger = "Celebrate";
        [SerializeField] private string disappointedTrigger = "Disappointed";

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
                case GameEventType.PlayersCelebrate:
                    if (animator && !string.IsNullOrEmpty(celebrateTrigger))
                        animator.SetTrigger(celebrateTrigger);
                    break;

                case GameEventType.PlayersDisappointed:
                    if (animator && !string.IsNullOrEmpty(disappointedTrigger))
                        animator.SetTrigger(disappointedTrigger);
                    break;
            }
        }
    }
}
