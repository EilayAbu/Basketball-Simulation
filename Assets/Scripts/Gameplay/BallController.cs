using UnityEngine;
using VRHoops.Core;

namespace VRHoops.Gameplay
{
    public class BallController : MonoBehaviour
    {
        private GameManager gameManager;
        private bool canBeThrown = false;
        private bool hasScored = false;

        [Header("Tags")]
        [SerializeField] private string floorTag = "Floor";

        public void Initialize(GameManager manager)
        {
            gameManager = manager;
        }

        public void EnableThrow()
        {
            canBeThrown = true;
            hasScored = false;
        }

        public void MarkAsThrown()
        {
            if (!canBeThrown)
            {
                Debug.Log("🚫 Cannot throw yet! Wait for referee whistle.");
                return;
            }

            canBeThrown = false;
            gameManager.OnBallThrown();
        }

        public void MarkAsScored()
        {
            hasScored = true;
            gameManager.OnBallScored();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(floorTag) && !hasScored)
            {
                gameManager.OnBallMissed();
            }
        }
    }
}
