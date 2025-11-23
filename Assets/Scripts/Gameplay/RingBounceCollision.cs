using UnityEngine;

public class RingBounceCollision : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float extraBounceForce = 4f;
    public float upwardBoost = 1f;

    [Header("Ball Tag")]
    public string ballTag = "Ball";

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag(ballTag))
            return;

        Rigidbody rb = collision.collider.attachedRigidbody;
        if (rb == null)
            return;

        // Normal vector of collision
        Vector3 normal = collision.contacts[0].normal;

        // Reflect ball velocity
        Vector3 bounce = Vector3.Reflect(rb.linearVelocity, normal);

        // Apply upward boost
        bounce += Vector3.up * upwardBoost;

        rb.linearVelocity = bounce;

        // Add extra force
        rb.AddForce(bounce.normalized * extraBounceForce, ForceMode.Impulse);
    }
}
