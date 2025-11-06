using UnityEngine;

namespace VRHoops.Core
{
    public enum ShotResult { Scored, Missed }

    // אירועים לוגיים כלליים במשחק
    public enum GameEventType
    {
        CrowdCheer,
        CrowdBoo,
        PlayersCelebrate,
        PlayersDisappointed
    }

    // לא חובה כרגע, שימושי אם תרצה לסווג מאזינים
    public enum ActorType
    {
        Crowd,
        Player,
        GlobalAudio
    }
}
