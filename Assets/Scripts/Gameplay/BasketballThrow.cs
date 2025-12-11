using UnityEngine;
using Oculus.Interaction;

public class BasketballThrow : MonoBehaviour
{
    [Header("Hand Reference")]
    [Tooltip("Transform of the player's right hand (from Meta XR Hand Rig)")]
    public Transform rightHand;

    [Header("Throw Settings")]
    public float power = 1.4f;
    public float maxSpeed = 14f;
    public float spinBoost = 1.2f;

    private Rigidbody rb;

    private bool beingHeld = false;

    private Vector3 lastPos;
    private Vector3 lastVel;
    private Vector3 lastAngularVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Only calculate velocity when ball is held
        if (!beingHeld || rightHand == null)
            return;

        // Position velocity
        Vector3 newPos = rightHand.position;
        lastVel = (newPos - lastPos) / Time.deltaTime;
        lastPos = newPos;

        // Angular velocity (optional: if hand has a Rigidbody)
        if (rightHand.TryGetComponent<Rigidbody>(out var handRb))
        {
            lastAngularVel = handRb.angularVelocity;
        }
    }

    // Called when the ball is grabbed
    public void OnSelect(PointerEvent evt)
    {
        beingHeld = true;
        lastPos = rightHand.position;

        // Optional: stop movement when picked up
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // Called when the ball is released
    public void OnUnselect(PointerEvent evt)
    {
        beingHeld = false;
        ThrowBall();
    }

    private void ThrowBall()
    {
        // Apply velocity
        Vector3 throwVel = lastVel * power;
        throwVel = Vector3.ClampMagnitude(throwVel, maxSpeed);

        rb.linearVelocity = throwVel;

        // Apply spin
        rb.angularVelocity = lastAngularVel * spinBoost;
    }
}
