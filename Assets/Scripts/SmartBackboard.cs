using UnityEngine;

public class SmartBackboard : MonoBehaviour
{
    [Header("Where to aim?")]
    public Transform hoopCenter; // גרור לכאן את האובייקט הריק שבמרכז הטבעת

    [Header("Physics Settings")]
    public float assistPower = 4f; // כמה חזק לדחוף לסל
    public float upwardArc = 1.5f; // קשת גובה כדי שזה ייראה יפה

    private void OnCollisionEnter(Collision collision)
    {
        // בדיקה: האם מה שפגע בי זה כדור?
        // חובה לתת לכדור את התג "Ball"
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();

            if (ballRb != null)
            {
                RedirectBall(ballRb);
            }
        }
    }

    private void RedirectBall(Rigidbody ballRb)
    {
        // 1. חישוב הכיוון מהכדור הנוכחי אל מרכז הטבעת
        Vector3 directionToHoop = (hoopCenter.position - ballRb.transform.position).normalized;

        // 2. איפוס המהירות הנוכחית של הכדור (כדי למנוע התנגשויות כפולות ובלגן)
        ballRb.linearVelocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;

        // 3. יצירת כוח חדש: לכיוון הסל + קצת למעלה
        Vector3 newVelocity = directionToHoop * assistPower + Vector3.up * upwardArc;

        // 4. שיגור הכדור
        ballRb.linearVelocity = newVelocity;
    }
}