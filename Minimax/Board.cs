namespace Minimax
{
    public abstract class Board
    {
        /// <summary>
        /// Set up the board with the given values
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public virtual void Initialise(int width, int height)
        {
            this.width = width;
            this.height = height;

            grid = new int[width * height];
        }

        /// <summary>
        /// Replaces all values in this board with the target board's values
        /// </summary>
        /// <param name="board"></param>
        public virtual void CopyBoard(Board target)
        {
            playerPly = target.playerPly;

            width = target.width;
            height = target.height;

            grid = new int[width * height];
            for (int i = 0; i < width * height; ++i) {
                grid[i] = target.grid[i];
            }
        }

        /// <summary>
        /// Returns true if this board is equal to the given board
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool Equal(Board b)
        {
            if (height == b.height && width == b.width && playerPly == b.playerPly) {
                for (int i = 0; i < width * height; ++i) {
                    if (grid[i] != b.grid[i]) {
                        return false;
                    }
                }
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Represents the player who is making the next action
        /// </summary>
        public int playerPly;

        public int width;
        public int height;

        /// <summary>
        /// Represents what is on each tile
        /// </summary>
        public int[] grid = new int[0]; // prevents warnings

        /// <summary>
        /// The move that was taken to reach this board state
        /// </summary>
        public Move lastMove; // there are warnings even if this is nullable

        /// <summary>
        /// Should return a value that is higher the better the board is for the AI / maximizing player.
        /// </summary>
        /// <returns></returns>
        public abstract float Heuristic();
        
        /// <summary>
        /// Calculates a move on this board for a given place in the grid
        /// </summary>
        /// <param name="index">Which tile the move is happening on</param>
        /// <returns></returns>
        public abstract Board[] CalculateAllMoves(int index);
    }
}
