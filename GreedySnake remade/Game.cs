using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GreedySnake_remade.components
{
    class Game
    {
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

        public void GamePrepare()
        {
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

        public void GameStart()
        {
            GameStatus = GameStatus.Running;
            roundControl.Start();
        }

        public void GameOver()
        {
            showGameOverText();
            GameStatus = GameStatus.Stopped;

            roundControl.Reset();

            // After 3 seconds, start game again.
            var nextRound = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            nextRound.Start();
            nextRound.Tick += (sender, args) =>
            {
                nextRound.Stop();
                GamePrepare();
            };
        }

        public void Progress()
        {
            var run = gameBoard.Progress();
            if (!run.isRunning)
            {
                GameOver();
            }
            else
            {
                run.blocksToUpdate.ForEach(uiBoard.RenderBlock);
                updateLayout();
            }
        }
    }

    enum GameStatus
    {
        Stopped,
        Prepare,
        Running
    }
}
