using System;
using System.Windows.Threading;

namespace GreedySnake.components
{
    class Game : IDisposable
    {
        private const int gameoverDelay = 2;
        private readonly Action showGameStartText;
        private readonly Action showGameOverText;
        private readonly Action updateLayout;

        private readonly UIboard uiBoard;
        private readonly GameBoard gameBoard;
        private readonly RoundController roundControl;
        private bool disposedValue;

        public GameStatus GameStatus { get; private set; } = GameStatus.Stopped;

        public Game(
            uint size,
            double interval,
            bool wrap,
            uint obLevel,
            Vector2 defaultDirection,
            UIboard uiBoard,
            Action showGameStartText,
            Action showGameOverText,
            Action updateLayout
        )
        {
            this.showGameStartText = showGameStartText;
            this.showGameOverText = showGameOverText;
            this.updateLayout = updateLayout;
            this.uiBoard = uiBoard;

            roundControl = new RoundController(interval);
            gameBoard = new GameBoard(size, wrap, obLevel, defaultDirection);
        }

        public void Prepare()
        {
            gameBoard.Init();
            uiBoard.Reset();

            GameStatus = GameStatus.Prepare;
            showGameStartText();
            var blocksToUpdate = gameBoard.Init();
            foreach (var block in blocksToUpdate)
            {
                uiBoard.RenderBlock(block);
            }

            roundControl.RegisterEvent((s, e) => Progress());
            roundControl.Stop();
        }

        public void ChangeDirection(Vector2 direction)
        {
            gameBoard.SetIncrement(direction);
        }

        public void ChangeSpeed(double interval)
        {
            roundControl.UpdateInterval(interval);
        }

        public void Start()
        {
            GameStatus = GameStatus.Running;
            roundControl.Start();
        }

        public void Over()
        {
            showGameOverText();
            GameStatus = GameStatus.Stopped;

            roundControl.Reset();

            // After {gameoverDelay} seconds, start game again.
            var nextRound = new DispatcherTimer { Interval = TimeSpan.FromSeconds(gameoverDelay) };
            nextRound.Start();
            nextRound.Tick += (sender, args) =>
            {
                nextRound.Stop();
                Prepare();
            };
        }

        public void Progress()
        {
            var run = gameBoard.Progress();
            if (!run.success)
            {
                Over();
            }
            else
            {
                foreach (var block in run.blocksToUpdate)
                {
                    uiBoard.RenderBlock(block);
                }
                updateLayout();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    roundControl.Reset();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Game()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    enum GameStatus
    {
        Stopped,
        Prepare,
        Running
    }
}
