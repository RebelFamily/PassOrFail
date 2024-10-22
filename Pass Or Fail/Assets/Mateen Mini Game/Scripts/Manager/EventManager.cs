using System;

namespace PassOrFail.MiniGames
{
    public class EventManager
    {
        public static event Action OnStopPlayerFight;

        public static void InvokeStopPlayerFight()
        {
            OnStopPlayerFight?.Invoke();
        }
    }
}