using UnityEngine;

public class BasketballThrow : MonoBehaviour
{
    public Transform rightHand;   // השורש של יד ימין (Meta XR Hand)
    public float power = 1.4f;
    public float maxSpeed = 14f;
    public float spinBoost = 1.3f;

    private Rigidbody rb;

    private Vector3 lastPos;
    private Vector3 lastVel;
    private Vector3 lastAngularVel;

    private bool wasHeld;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // אם הכדור מוחזק (Meta הופכת אותו ל-Kinematic)
        bool isHeld = rb.isKinematic;

        if (isHeld)
        {
            // מחשבים Velocity של היד לפי שינוי מיקום
            Vector3 currentPos = rightHand.position;
            Vector3 vel = (currentPos - lastPos) / Time.deltaTime;

            lastVel = vel;
            lastAngularVel = rightHand.GetComponent<Rigidbody>()?.angularVelocity ?? Vector3.zero;

            lastPos = currentPos;
        }

        // Transition מ-Kinematic → Non-Kinematic = זריקה
        if (wasHeld && !isHeld)
        {
            ThrowBall();
        }

        wasHeld = isHeld;
    }

    private void ThrowBall()
    {
        Vector3 v = lastVel * power;
        v = Vector3.ClampMagnitude(v, maxSpeed);

        rb.linearVelocity = v;
        rb.angularVelocity = lastAngularVel * spinBoost;
    }
}
