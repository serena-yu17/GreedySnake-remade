using GreedySnake.components;
using System.Windows;
using System.Windows.Input;

namespace GreedySnake
{
    public partial class MainWindow : Window
    {
        private const int size = 40;
        private const int baseInterval = 250;
        private readonly Vector2 defaultDirection = DirectionControl.right;
        private readonly UIboard uiBoard;

        private uint obLevel = 0;
        private bool wrap = true;
        private double interval = baseInterval;

        private Game game;

        public MainWindow()
        {
            InitializeComponent();
            mainGrid.Background = ColorBrushes.grassBrush;
            PreviewKeyDown += new KeyEventHandler(HandleKeyDown);
            uiBoard = new UIboard(size, mainGrid, Width, Height);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PrepareGame();
        }

        private void PrepareGame()
        {
            game?.Dispose();
            game = new Game(
                size,
                interval,
                wrap,
                obLevel,
                defaultDirection,
                uiBoard,
                ShowGameStartText,
                ShowGameOverText,
                UpdateLayout
            );
            game.Prepare();
        }

        private void KickStart()
        {
            lbGameOver.Visibility = Visibility.Hidden;
            sldOb.IsEnabled = false;
            chkWrap.IsEnabled = false;
            sldSpd.IsEnabled = false;
            game.Start();
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (game.GameStatus == GameStatus.Prepare && e.Key != Key.Escape)
            {
                KickStart();
            }
            switch (e.Key)
            {
                case Key.Down:
                    game.ChangeDirection(DirectionControl.down);
                    break;
                case Key.Up:
                    game.ChangeDirection(DirectionControl.up);
                    break;
                case Key.Right:
                    game.ChangeDirection(DirectionControl.right);
                    break;
                case Key.Left:
                    game.ChangeDirection(DirectionControl.left);
                    break;
                case Key.Escape:
                    if (IsLoaded)
                    {
                        PrepareGame();
                    }
                    break;
            }
        }

        void SldSpd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            interval = baseInterval / sldSpd.Value;
            if (IsLoaded)
            {
                game?.ChangeSpeed(interval);
            }
        }


        void SldOb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            obLevel = (uint)sldOb.Value;
            if (IsLoaded)
            {
                PrepareGame();
            }
        }

        private void ChkWrap_Checked(object sender, RoutedEventArgs e)
        {
            wrap = chkWrap.IsChecked ?? false;
            if (IsLoaded)
            {
                PrepareGame();
            }
        }

        private void ShowGameStartText()
        {
            chkWrap.IsEnabled = true;
            sldOb.IsEnabled = true;
            sldSpd.IsEnabled = true;
            lbGameOver.Visibility = Visibility.Visible;
            lbGameOver.Content = "Press any key to start";
            lbGameOver.FontSize = 30;
        }

        private void ShowGameOverText()
        {
            lbGameOver.Content = "Game Over";
            lbGameOver.FontSize = 50;
            lbGameOver.Visibility = Visibility.Visible;
        }
    }
}

