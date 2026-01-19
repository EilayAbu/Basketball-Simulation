using UnityEngine;
using VRHoops.Core;

namespace VRHoops.Gameplay
{
    public class BallController : MonoBehaviour
    {
    
        private bool hasScored = false;
        bool touch = false;
        [Header("Tags")]
        [SerializeField] private string floorTag = "Floor";

        public void MarkAsScored()
        {
            hasScored = true;
            GameManager.Instance.OnBallScored();
        }

        private void OnCollisionEnter(Collision collision)
        {

            Debug.Log(collision.gameObject.name);
            // התנאי המתוקן: רק בודק אם זו רצפה ואם עדיין לא קלענו
            if (collision.collider.CompareTag(floorTag) && !hasScored)
            {
                GameManager.Instance.OnBallMissed();
            }
        }
    }
}

