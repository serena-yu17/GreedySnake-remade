namespace GreedySnake_remade.components
{
    struct Vector2
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

        public static bool operator ==(Vector2 pt1, Vector2 pt2)
        {
            return pt1.x == pt2.x && pt1.y == pt2.y;
        }

        public static bool operator !=(Vector2 pt1, Vector2 pt2)
        {
            return !(pt1 == pt2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return this == null;
            }
            if (!(obj is Vector2 p))
            {
                return false;
            }
            return (x == p.x) && (y == p.y);
        }

        public bool Equals(Vector2 other)
        {
            return (x == other.x) && (y == other.y);
        }

        public override int GetHashCode()
        {
            return (x << 8) + y;
        }
    }
}
