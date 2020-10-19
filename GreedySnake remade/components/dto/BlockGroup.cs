using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace GreedySnake.components
{
    class BlockGroup
    {
        private readonly Queue<Block> buildBlocks = new Queue<Block>();
        private readonly HashSet<Vector2> used;
        private readonly SolidColorBrush fillBrush;

        public BlockGroup(HashSet<Vector2> usd, SolidColorBrush fillBrush)
        {
            used = usd;
            this.fillBrush = fillBrush;
        }

        public HashSet<Block> AllBlocks()
        {
            return new HashSet<Block>(buildBlocks);
        }

        public Block Prepend(Vector2 newHead)
        {
            var block = new Block(newHead, fillBrush, ColorBrushes.blackStroke);

            buildBlocks.Enqueue(block);
            used.Add(newHead);

            return block;
        }

        public Block PopTail()
        {
            var tailBlock = buildBlocks.Dequeue();
            tailBlock.fill = null;
            tailBlock.stroke = ColorBrushes.whiteStroke;

            used.Remove(tailBlock.coordinate);

            return tailBlock;
        }

        public Vector2 Head()
        {
            return buildBlocks.LastOrDefault().coordinate;
        }

        public HashSet<Block> Clear()
        {
            HashSet<Block> blocksToUpdate = new HashSet<Block>();
            foreach (var block in buildBlocks)
            {
                var update = new Block(block.coordinate, null, ColorBrushes.whiteStroke);
            }
            buildBlocks.Clear();
            return blocksToUpdate;
        }
    }
}
