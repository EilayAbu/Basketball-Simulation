using UnityEngine;
using System.Collections.Generic;

public class EnhancedThrow : MonoBehaviour
{
    [Header("Throw Settings")]
    public float throwMultiplier = 1.3f;
    public int smoothingFrames = 5;

    private Rigidbody rb;
    private Queue<Vector3> velocitySamples;
    private Vector3 lastPosition;
    private bool isHeld = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocitySamples = new Queue<Vector3>();

        // --- תוספת חשובה: הקפאת הכדור בהתחלה ---
        rb.isKinematic = true;
        // ---------------------------------------
    }

    void FixedUpdate()
    {
        if (isHeld)
        {
            Vector3 currentVelocity = (transform.position - lastPosition) / Time.fixedDeltaTime;
            velocitySamples.Enqueue(currentVelocity);

            if (velocitySamples.Count > smoothingFrames)
            {
                velocitySamples.Dequeue();
            }

            lastPosition = transform.position;
        }
    }

    public void OnGrab()
    {
        isHeld = true;
        velocitySamples.Clear();
        lastPosition = transform.position;

        // מוודאים שהוא נשאר קינמטי בזמן האחיזה
        rb.isKinematic = true;
    }

    public void OnRelease()
    {
        isHeld = false;

        // משחררים את הפיזיקה
        rb.isKinematic = false;

        // --- הוסף את השורה הזו: ---
        // מודיעים למנהל המשחק שהכדור באוויר
       
        if(VRHoops.Core.GameManager.Instance != null)
            VRHoops.Core.GameManager.Instance.OnBallThrown();
        // --------------------------

        ApplyThrowForce();
    }

    private void ApplyThrowForce()
    {
        if (velocitySamples.Count == 0) return;

        Vector3 averageVelocity = Vector3.zero;
        foreach (Vector3 v in velocitySamples)
        {
            averageVelocity += v;
        }
        averageVelocity /= velocitySamples.Count;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(averageVelocity * throwMultiplier, ForceMode.VelocityChange);
    }
}