using System.Collections.Generic;

namespace GreedySnake.components.dto
{
    struct GameRunResult
    {
        public bool success;
        public HashSet<Block> blocksToUpdate;

        public GameRunResult(bool success, HashSet<Block> blocksToUpdate)
        {
            this.success = success;
            this.blocksToUpdate = blocksToUpdate;
        }
    }
}
