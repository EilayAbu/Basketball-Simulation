using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine.InputSystem;

public class BallSpawner : MonoBehaviour
{
    [Header("Ball Setup")]
    public GameObject ballPrefab;
    public Transform spawnPoint;

    [Header("Hands (Assign in Inspector)")]
    public Transform rightHand;
    public Transform leftHand;

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SpawnBall();
        }
    }

    public void SpawnBall()
    {
        // יוצרים כדור חדש
        GameObject ball = Instantiate(ballPrefab, spawnPoint.position, spawnPoint.rotation);

        // מוסרים רפרנס לידיים
        //var thrower = ball.GetComponent<BasketballThrowMetaHands>();
        //thrower.rightHandWrist = rightHand;
        //thrower.leftHandWrist = leftHand;

        Debug.Log("New ball spawned");
    }
}
