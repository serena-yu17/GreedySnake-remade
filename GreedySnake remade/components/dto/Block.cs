using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace GreedySnake.components
{
    struct Block : IEquatable<Block>
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

        public override bool Equals(object obj)
        {
            return obj is Block block && Equals(block);
        }

        public bool Equals(Block other)
        {
            return EqualityComparer<Vector2>.Default.Equals(coordinate, other.coordinate);
        }

        public override int GetHashCode()
        {
            return coordinate.GetHashCode();
        }

        public static bool operator ==(Block left, Block right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Block left, Block right)
        {
            return !(left == right);
        }
    }
}
