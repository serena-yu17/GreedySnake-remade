using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace GreedySnake.components
{
    class RoundController
    {
        private readonly DispatcherTimer gameTimer;

        public List<EventHandler> eventHandlers = new List<EventHandler>();

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

        public void RegisterEvent(EventHandler eventHandler)
        {
            eventHandlers.Add(eventHandler);
            gameTimer.Tick += eventHandler;
        }

        public void Reset()
        {
            Stop();
            foreach (var eh in eventHandlers)
            {
                gameTimer.Tick -= eh;
            }
            eventHandlers.Clear();
        }
    }
}
