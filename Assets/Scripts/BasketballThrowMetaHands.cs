using UnityEngine;

public class BasketballThrowMetaHands : MonoBehaviour
{
    [Header("Tracked Wrist Points")]
    [SerializeField] public Transform rightHandWrist;
    [SerializeField] public Transform leftHandWrist;

    [Header("Throw Settings")]
    public float throwPower = 2f;
    public float maxThrowSpeed = 24f;
    public float spinMultiplier = 1.2f;

    private Rigidbody rb;

    private Transform activeHand;
    private Vector3 lastPos;
    private Quaternion lastRot;

    private Vector3 velocity;
    private Vector3 angularVelocity;

    private bool isHeld = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isHeld || activeHand == null)
            return;

        // Linear velocity
        Vector3 newPos = activeHand.position;
        velocity = (newPos - lastPos) / Time.deltaTime;
        lastPos = newPos;

        // Angular velocity
        Quaternion newRot = activeHand.rotation;
        Quaternion delta = newRot * Quaternion.Inverse(lastRot);
        delta.ToAngleAxis(out float angle, out Vector3 axis);
        angularVelocity = axis * angle * Mathf.Deg2Rad / Time.deltaTime;
        lastRot = newRot;
    }

    public void OnSelect(Oculus.Interaction.PointerEvent evt)
    {
        // pick the nearest wrist
        activeHand = GetClosestWrist();

        if (activeHand == null)
            return;

        isHeld = true;

        lastPos = activeHand.position;
        lastRot = activeHand.rotation;

        velocity = Vector3.zero;
        angularVelocity = Vector3.zero;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void OnUnselect(Oculus.Interaction.PointerEvent evt)
    {
        if (!isHeld || activeHand == null)
            return;

        isHeld = false;

        Vector3 throwVel = velocity * throwPower;
        throwVel = Vector3.ClampMagnitude(throwVel, maxThrowSpeed);

        rb.linearVelocity = throwVel;
        rb.angularVelocity = angularVelocity * spinMultiplier;

        activeHand = null;
    }

    private Transform GetClosestWrist()
    {
        float rightDist = Vector3.Distance(rightHandWrist.position, transform.position);
        float leftDist = Vector3.Distance(leftHandWrist.position, transform.position);

        return rightDist < leftDist ? rightHandWrist : leftHandWrist;
    }
}
