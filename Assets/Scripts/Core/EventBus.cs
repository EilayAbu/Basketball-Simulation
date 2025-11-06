using System;
using System.Collections.Generic;

namespace VRHoops.Core
{
    public static class EventBus
    {
        // אירוע תוצאת זריקה (עם מידע אם נקלע/הוחטא)
        public static event Action<ShotResult> OnShotResolved;

        // אירועי משחק כלליים (Cheer/Boo/וכו')
        public static event Action<GameEventType> OnGameEvent;

        public static void PublishShotResult(ShotResult result)
        {
            OnShotResolved?.Invoke(result);
        }

        public static void PublishGameEvent(GameEventType evt)
        {
            OnGameEvent?.Invoke(evt);
        }
    }
}
