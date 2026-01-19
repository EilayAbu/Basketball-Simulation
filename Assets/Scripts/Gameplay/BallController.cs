using UnityEngine;
using VRHoops.Core;

namespace VRHoops.Gameplay
{
    public class BallController : MonoBehaviour
    {
        private GameManager gameManager;
        private bool hasScored = false;
        bool touch = false;
        [Header("Tags")]
        [SerializeField] private string floorTag = "Floor";

        public void Initialize(GameManager manager)
        {
            gameManager = manager;
        }

        void Start() 
        {
            hasScored = false;
        }

       

        public void MarkAsScored()
        {
            hasScored = true;
            gameManager.OnBallScored();
        }

        private void OnCollisionEnter(Collision collision)
        {

            Debug.Log(collision.gameObject.name);
            // התנאי המתוקן: רק בודק אם זו רצפה ואם עדיין לא קלענו
            if (collision.collider.CompareTag(floorTag) && !hasScored)
            {
                gameManager.OnBallMissed();
            }
        }
    }
}

