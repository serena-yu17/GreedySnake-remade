using System;

namespace GreedySnake.components
{
    struct Vector2 : IEquatable<Vector2>
    {
        public int x;
        public int y;

        public Vector2(int a, int b)
        {
            x = a;
            y = b;
        }

        public Vector2(Vector2 other)
        {
            x = other.x;
            y = other.y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2 vector && Equals(vector);
        }

        public bool Equals(Vector2 other)
        {
            return x == other.x &&
                   y == other.y;
        }

        public override int GetHashCode()
        {
            int hashCode = 1502939027;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !(left == right);
        }
    }
}
