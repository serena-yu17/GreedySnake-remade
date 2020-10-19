using System;
using System.Collections.Generic;
using System.Linq;

namespace GreedySnake_remade.components
{
    class GameBoard
    {
        private static readonly Random rnd = new Random(Guid.NewGuid().GetHashCode());

        // configurations
        private readonly uint size;
        private Vector2 increment;
        private Vector2 lastIncrement;
        private readonly uint obstructLevel;
        private readonly bool wrap;
        private readonly HashSet<Vector2> occupiedBlocks = new HashSet<Vector2>();
        private Block apple;
        private readonly BlockChain snake;
        private readonly BlockChain stone;

        public GameBoard(uint size, bool wrap, uint obstructLevel, Vector2 increment)
        {
            this.size = size;
            this.wrap = wrap;
            this.obstructLevel = obstructLevel;
            this.increment = increment;
            this.lastIncrement = new Vector2(increment);

            snake = new BlockChain(size, occupiedBlocks, ColorBrushes.snakeBrush);
            stone = new BlockChain(size, occupiedBlocks, ColorBrushes.stoneBrush);
        }

        public void SetIncrement(Vector2 newIncrement)
        {
            increment = newIncrement;
        }

        public HashSet<Block> Init()
        {
            SetSnake();
            SetStone();
            SetApple();

            HashSet<Block> blocksToUpdate = new HashSet<Block>();
            blocksToUpdate.UnionWith(snake.AllBlocks());
            blocksToUpdate.UnionWith(stone.AllBlocks());
            blocksToUpdate.Add(apple);
            return blocksToUpdate;
        }

        public GameRunResult Progress()
        {
            List<Block> blocksToUpdate = new List<Block>();

            if ((increment.x == 0 && increment.y == -lastIncrement.y) || (increment.x == -lastIncrement.x && increment.y == 0))
            {
                increment.x = lastIncrement.x;
                increment.y = lastIncrement.y;
            }
            lastIncrement.x = increment.x;
            lastIncrement.y = increment.y;
            Vector2 nextPoint = new Vector2(snake.Head().x + increment.x, snake.Head().y + increment.y);

            if (wrap)
            {
                // allow wrap through walls
                nextPoint = WrapSnake(nextPoint);
            }
            else if (nextPoint.x == size || nextPoint.y == size || nextPoint.x == -1 || nextPoint.y == -1)
            {
                return new GameRunResult(false, blocksToUpdate);
            }

            if (occupiedBlocks.Contains(nextPoint))
            {
                new GameRunResult(false, blocksToUpdate);
            }
            else
            {
                var nextBlock = snake.Prepend(nextPoint);
                blocksToUpdate.Add(nextBlock);

                if (nextPoint.x != apple.coordinate.x || nextPoint.y != apple.coordinate.y)
                {
                    // Move forward
                    var tailBlock = snake.PopTail();
                    blocksToUpdate.Add(tailBlock);
                }
                else
                {
                    // Eaten an apple. Growing
                    var apple = SetApple();
                    blocksToUpdate.Add(apple);
                }
            }

            return new GameRunResult(true, blocksToUpdate);
        }

        private Vector2 WrapSnake(Vector2 nxt)
        {
            if (nxt.x == size)
                return new Vector2(0, nxt.y);
            if (nxt.y == size)
                return new Vector2(nxt.x, 0);
            if (nxt.x == -1)
                return new Vector2((int)size - 1, nxt.y);
            if (nxt.y == -1)
                return new Vector2(nxt.x, (int)size - 1);
            return nxt;
        }

        private Block SetApple()
        {
            Vector2 pt;
            do
            {
                pt = new Vector2(rnd.Next(0, (int)size), rnd.Next(0, (int)size));
            } while (occupiedBlocks.Contains(pt));
            apple = new Block(pt, ColorBrushes.appleBrush, ColorBrushes.blackStroke);

            return apple;
        }

        private void SetStone()
        {
            int dimension = (int)size / 4;
            int len = (int)size / 2;

            switch (obstructLevel)
            {
                case 1:
                    {
                        for (int i = 0; i < len; i++)
                        {
                            Vector2 pObTop = new Vector2(i, dimension);
                            Vector2 pObLower = new Vector2(i + len, dimension + len);
                            stone.Prepend(pObTop);
                            stone.Prepend(pObLower);
                        }
                        goto case 2;
                    }
                case 2:
                    {
                        for (int i = 0; i < len; i++)
                        {
                            Vector2 pObLeft = new Vector2(dimension + len, i);
                            Vector2 pObRight = new Vector2(dimension, i + len);
                            stone.Prepend(pObLeft);
                            stone.Prepend(pObRight);
                        }
                        break;
                    }
            }
        }

        private void SetSnake()
        {
            Vector2 mid = new Vector2((int)(size / 2), (int)size / 2);
            snake.Prepend(mid);
            Vector2 nxt = new Vector2(mid);
            nxt.x++;
            snake.Prepend(nxt);
        }
    }

    struct GameRunResult
    {
        public bool isRunning;
        public List<Block> blocksToUpdate;

        public GameRunResult(bool isRunning, List<Block> blocksToUpdate)
        {
            this.isRunning = isRunning;
            this.blocksToUpdate = blocksToUpdate;
        }
    }
}
