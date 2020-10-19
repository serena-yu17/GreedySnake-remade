using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace GreedySnake_remade.components
{
    class RoundController
    {
        private readonly DispatcherTimer gameTimer;
        private uint eid = 0;

        public Dictionary<uint, EventHandler> events = new Dictionary<uint, EventHandler>();

        public RoundController(double intervalMillis)
        {
            gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(intervalMillis)
            };
            gameTimer.Stop();
        }

        public bool IsRunning
        {
            get
            {
                return gameTimer.IsEnabled;
            }
        }

        public void Start()
        {
            gameTimer.Start();
        }

        public void Stop()
        {
            gameTimer.Stop();
        }

        public void UpdateInterval(double intervalMillis)
        {
            gameTimer.Interval = TimeSpan.FromMilliseconds(intervalMillis);
        }

        public uint RegisterEvent(EventHandler eventHandler)
        {
            events[eid] = eventHandler;
            eid++;
            gameTimer.Tick += eventHandler;
            return eid;
        }

        public void RemoveEvent(uint eid)
        {
            events.TryGetValue(eid, out var eventHandler);
            if (eventHandler != null)
            {
                gameTimer.Tick -= eventHandler;
            }
            events.Remove(eid);
        }

        public void Reset()
        {
            Stop();
            foreach (var kp in events)
            {
                gameTimer.Tick -= kp.Value;
            }
            events.Clear();
            eid = 0;
        }
    }
}
