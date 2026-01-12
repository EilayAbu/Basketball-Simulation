using UnityEngine;
using VRHoops.Core;

public class BallControllerTraining : MonoBehaviour
{
    private bool canBeThrown = false;
    private bool hasScored = false;
    bool touch = false;
    [Header("Tags")]
    [SerializeField] private string floorTag = "Floor";

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform spawnPoint;

    



    

    private void OnCollisionEnter(Collision collision)
    {

        Debug.Log(collision.gameObject.name);
        // התנאי המתוקן: רק בודק אם זו רצפה ואם עדיין לא קלענו
        if (collision.collider.CompareTag(floorTag) && !hasScored)
        {
            var newBallObj = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);

        }
    }
}
