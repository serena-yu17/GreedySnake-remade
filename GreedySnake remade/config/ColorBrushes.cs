using System.Windows.Media;

namespace GreedySnake_remade.components
{
    static class ColorBrushes
    {
        public static readonly SolidColorBrush whiteStroke = new SolidColorBrush(Color.FromRgb(200, 250, 200));
        public static readonly SolidColorBrush blackStroke = new SolidColorBrush(Color.FromRgb(25, 25, 25));
        public static readonly SolidColorBrush grassBrush = new SolidColorBrush(Color.FromRgb(208, 230, 145));

        public static readonly SolidColorBrush snakeBrush = new SolidColorBrush(Color.FromRgb(0, 250, 0));
        public static readonly SolidColorBrush stoneBrush = new SolidColorBrush(Color.FromRgb(25, 25, 25));
        public static readonly SolidColorBrush appleBrush = new SolidColorBrush(Color.FromRgb(250, 20, 20));
    }
}
