using System.Collections.Generic;
using System.Windows.Media;

namespace GreedySnake_remade.components
{
    class BlockChain
    {
        private readonly Vector2[] points;

        private readonly Block[,] buildBlocks;
        private readonly HashSet<Vector2> used;
        private readonly SolidColorBrush fillBrush;

        private readonly uint length = 0;
        private int head = -1;
        private int tail = 0;

        public BlockChain(uint size, HashSet<Vector2> usd, SolidColorBrush fillBrush)
        {
            length = size * size;
            points = new Vector2[length];
            used = usd;
            this.fillBrush = fillBrush;

            buildBlocks = new Block[size, size];
            for (int row = 0; row < buildBlocks.GetLength(0); row++)
                for (int col = 0; col < buildBlocks.GetLength(1); col++)
                {
                    buildBlocks[row, col] = new Block(new Vector2(row, col), null, ColorBrushes.whiteStroke);
                }
        }

        public List<Block> AllBlocks()
        {
            List<Block> blocks = new List<Block>();
            for (int row = 0; row < buildBlocks.GetLength(0); row++)
                for (int col = 0; col < buildBlocks.GetLength(1); col++)
                {
                    blocks.Add(buildBlocks[row, col]);
                }
            return blocks;
        }

        public Block Prepend(Vector2 pt)
        {
            head++;
            if (head == length) { head = 0; }
            points[head] = pt;
            var block = buildBlocks[pt.x, pt.y];
            block.fill = fillBrush;
            block.stroke = ColorBrushes.blackStroke;

            used.Add(pt);

            return block;
        }

        public Block PopTail()
        {
            var tailBlock = ClearBlock(tail);

            tail++;
            if (tail == length)
                tail = 0;

            return tailBlock;
        }

        private Block ClearBlock(int index)
        {
            var point = points[index];
            var block = buildBlocks[point.x, point.y];
            block.fill = null;
            block.stroke = ColorBrushes.whiteStroke;
            used.Remove(point);
            return block;
        }

        public Vector2 Head()
        {
            if (head != -1)
                return points[head];
            else
                return new Vector2(0, 0);
        }
    }
}
