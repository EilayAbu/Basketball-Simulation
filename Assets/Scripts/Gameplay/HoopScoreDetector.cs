using UnityEngine;
using VRHoops.Core;

namespace VRHoops.Gameplay
{
    public class HoopScoreDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                Debug.Log("Ball entered the hoop zone!");
                GameManager.Instance.ResolveShot(ShotResult.Scored);
            }
        }
    }
}
