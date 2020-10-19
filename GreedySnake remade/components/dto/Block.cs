using System.Windows.Media;

namespace GreedySnake_remade.components
{
    struct Block
    {
        public Vector2 coordinate;
        public SolidColorBrush fill;
        public SolidColorBrush stroke;

        public Block(Vector2 coordinate, SolidColorBrush fill, SolidColorBrush stroke) : this()
        {
            this.coordinate = coordinate;
            this.fill = fill;
            this.stroke = stroke;
        }
    }
}
