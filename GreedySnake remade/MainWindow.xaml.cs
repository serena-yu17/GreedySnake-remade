using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GreedySnake_remade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        snakeQueue snake;
        int size = 40;
        int baseInterval = 250;
        Point incre;
        Point increReg;
        Boolean gameon = false;
        Rectangle[,] rects;
        HashSet<Point> used = new HashSet<Point>();
        snakeQueue Obstructs;
        Point apple;
        int Ob;
        static Random rnd;
        static SolidColorBrush appleBrush = new SolidColorBrush(Color.FromRgb(250, 20, 20));
        bool wrap;
        static SolidColorBrush whiteStroke = new SolidColorBrush(Color.FromRgb(200, 250, 200));
        static SolidColorBrush blackStroke = new SolidColorBrush(Color.FromRgb(25, 25, 25));
        public MainWindow()
        {
            InitializeComponent();
            PreviewKeyDown += new KeyEventHandler(OnFormPKD);
            drawGrid();
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(baseInterval);
            timer.Stop();
            snake = new snakeQueue(size, rects, used, Color.FromRgb(0, 225, 0));
            mainGrid.Background = new SolidColorBrush(Color.FromRgb(208, 230, 145));
            Obstructs = new snakeQueue(size, rects, used, Color.FromRgb(25, 25, 25));
            rnd = new Random(Guid.NewGuid().GetHashCode());
            wrap = (bool)chkWrap.IsChecked;
            setOB();
            setApple();
            UpdateLayout();
            gamestart();
        }
        void gamestart()
        {
            chkWrap.IsEnabled = true;
            sldOb.IsEnabled = true;
            timer.Stop();
            lbGameOver.Visibility = Visibility.Visible;
            lbGameOver.Content = "Press any key to start";
            lbGameOver.FontSize = 30;
            snake.clear();
            Obstructs.clear();
            incre = new Point(1, 0);
            increReg = new Point(incre);
            setSnake();
            setOB();
            setApple();
            UpdateLayout();
            gameon = true;
        }
        void gameover()
        {
            gameon = false;
            timer.Stop();
            lbGameOver.Content = "Game Over";
            lbGameOver.FontSize = 50;
            lbGameOver.Visibility = Visibility.Visible;
            DispatcherTimer tempTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            tempTimer.Start();
            tempTimer.Tick += (sender, args) =>
            {
                tempTimer.Stop();
                gamestart();
            };
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (gameon == true)
            {
                if ((incre.x == 0 && incre.y == -increReg.y) || (incre.x == -increReg.x && incre.y == 0))
                {
                    incre.x = increReg.x;
                    incre.y = increReg.y;
                }
                increReg.x = incre.x;
                increReg.y = incre.y;
                Point nxt = new Point(snake.readHead().x + incre.x, snake.readHead().y + incre.y);
                if (wrap)
                    wrapSnake(nxt);
                else if (nxt.x == size || nxt.y == size || nxt.x == -1 || nxt.y == -1)
                {
                    gameover();
                    return;
                }
                if (used.Contains(nxt))
                {
                    gameover();
                }
                else
                {
                    snake.append(nxt);
                    if (nxt.x != apple.x || nxt.y != apple.y)
                        snake.pop();
                    else
                        setApple();
                }
            }
        }
        void wrapSnake(Point nxt)
        {
            if (nxt.x == size)
                nxt.x = 0;
            if (nxt.y == size)
                nxt.y = 0;
            if (nxt.x == -1)
                nxt.x = size - 1;
            if (nxt.y == -1)
                nxt.y = size - 1;
        }
        private void OnFormPKD(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (gameon)
            {
                if (timer.IsEnabled == false && e.Key != Key.Escape)
                {
                    timer.Start();
                    lbGameOver.Visibility = Visibility.Hidden;
                    sldOb.IsEnabled = false;
                }
                switch (e.Key)
                {
                    case Key.Down:
                        incre.x = 0;
                        incre.y = 1;
                        break;
                    case Key.Up:
                        incre.x = 0;
                        incre.y = -1;
                        break;
                    case Key.Right:
                        incre.x = 1;
                        incre.y = 0;
                        break;
                    case Key.Left:
                        incre.x = -1;
                        incre.y = 0;
                        break;
                    case Key.Escape:
                        gamestart();
                        break;
                }
            }
        }
        void setApple()
        {
            if (apple != null && rects[apple.x, apple.y].Fill == appleBrush)
                rects[apple.x, apple.y].Fill = null;
            Point pt;
            do
            {
                pt = new Point(rnd.Next(0, size), rnd.Next(0, size));
            } while (used.Contains(pt));
            apple = pt;
            rects[apple.x, apple.y].Fill = appleBrush;
        }
        void setOB()
        {
            if (Ob == 1 || Ob == 2)
            {
                int ht = size >> 2;
                int len = size >> 1;
                for (int i = 0; i < len; i++)
                {
                    Point pObHigher = new Point(i, ht);
                    Point pObLower = new Point(i + len, ht + len);
                    Obstructs.append(pObHigher);
                    Obstructs.append(pObLower);
                }
            }
            if (Ob == 2)
            {
                int wid = size >> 2;
                int len = size >> 1;
                for (int i = 0; i < len; i++)
                {
                    Point pObLeft = new Point(wid + len, i);
                    Point pObRight = new Point(wid, i + len);
                    Obstructs.append(pObLeft);
                    Obstructs.append(pObRight);
                }
            }
        }
        void setSnake()
        {
            Point mid = new Point(size / 2, size / 2);
            snake.append(mid);
            Point nxt = new Point(mid);
            nxt.x++;
            snake.append(nxt);
        }
        void sldOb_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Ob = (int)sldOb.Value;
            if (Obstructs != null)
            {
                Obstructs.clear();
                setOB();
                setApple();
            }
        }
        void sldSpd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (timer != null)
                timer.Interval = TimeSpan.FromMilliseconds(baseInterval / sldSpd.Value);
        }
        void drawGrid()
        {
            rects = new Rectangle[size, size];
            for (int i = 0; i < size; i++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition());
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    rects[i, j] = new Rectangle
                    {
                        Fill = null,
                        StrokeThickness = 1,
                        Stroke = whiteStroke,
                    };
                    rects[i, j].Width = Width / size;
                    rects[i, j].Height = Height / size;
                    mainGrid.Children.Add(rects[i, j]);
                    Grid.SetRow(rects[i, j], j);
                    Grid.SetColumn(rects[i, j], i);
                }
        }

        class snakeQueue
        {
            private Point[] points;
            private int length = 0;
            private int head = -1;
            private int tail = 0;
            Rectangle[,] rects;
            HashSet<Point> used;
            SolidColorBrush brush;
            public snakeQueue(int sz, Rectangle[,] rc, HashSet<Point> usd, Color clr)
            {
                length = sz * sz;
                points = new Point[length];
                rects = rc;
                used = usd;
                brush = new SolidColorBrush(clr);
            }
            public void append(Point pt)
            {
                head++;
                if (head == length)
                    head = 0;
                points[head] = pt;
                rects[pt.x, pt.y].Fill = brush;
                rects[pt.x, pt.y].Stroke = blackStroke;
                used.Add(pt);
            }
            public void pop()
            {
                rects[points[tail].x, points[tail].y].Fill = null;
                rects[points[tail].x, points[tail].y].Stroke = whiteStroke;
                used.Remove(points[tail]);
                tail++;
                if (tail == length)
                    tail = 0;
            }
            public void clear()
            {
                if (tail <= head)
                {
                    for (int i = tail; i <= head; i++)
                    {
                        rects[points[i].x, points[i].y].Fill = null;
                        rects[points[i].x, points[i].y].Stroke = whiteStroke;
                        used.Remove(points[i]);
                    }
                    head = -1;
                    tail = 0;
                }
                else if (tail > head && head != -1)
                {
                    for (int i = tail; i < length; i++)
                    {
                        rects[points[i].x, points[i].y].Fill = null;
                        rects[points[i].x, points[i].y].Stroke = whiteStroke;
                        used.Remove(points[i]);
                    }
                    for (int i = 0; i <= head; i++)
                    {
                        rects[points[i].x, points[i].y].Fill = null;
                        rects[points[i].x, points[i].y].Stroke = whiteStroke;
                        used.Remove(points[i]);
                    }
                    head = -1;
                    tail = 0;
                }
            }
            public Point readHead()
            {
                if (head != -1)
                    return points[head];
                else
                    return new Point(0, 0);
            }
        }
        class Point
        {
            public int x;
            public int y;
            public Point(int a, int b)
            {
                x = a;
                y = b;
            }
            public Point(Point other)
            {
                x = other.x;
                y = other.y;
            }
            public static bool operator ==(Point pt1, Point pt2)
            {
                if (ReferenceEquals(pt1, null) || ReferenceEquals(pt2, null))
                {
                    return ReferenceEquals(pt1, null) && ReferenceEquals(pt2, null);
                }
                return (pt1.x == pt2.x && pt1.y == pt2.y);
            }
            public static bool operator !=(Point pt1, Point pt2)
            {
                return !(pt1 == pt2);
            }
            public override bool Equals(System.Object obj)
            {
                if (obj == null)
                {
                    return (this == null);
                }
                Point p = obj as Point;
                if ((System.Object)p == null)
                {
                    return false;
                }
                return (x == p.x) && (y == p.y);
            }
            public bool Equals(Point other)
            {
                return (x == other.x) && (y == other.y);
            }
            public override int GetHashCode()
            {
                return (x << 8) + y;
            }
        }

        private void chkWrap_Checked(object sender, RoutedEventArgs e)
        {
            wrap = (bool)chkWrap.IsChecked;
        }

    }
}

