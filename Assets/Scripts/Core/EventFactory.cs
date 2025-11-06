using System.Collections.Generic;

namespace VRHoops.Core
{
    public static class EventFactory
    {
        // מפיק רשימת אירועים להפצה לפי התוצאה
        public static IEnumerable<GameEventType> Build(ShotResult result)
        {
            if (result == ShotResult.Scored)
            {
                yield return GameEventType.CrowdCheer;
                yield return GameEventType.PlayersCelebrate;
            }
            else
            {
                yield return GameEventType.CrowdBoo;
                yield return GameEventType.PlayersDisappointed;
            }
        }
    }
}
