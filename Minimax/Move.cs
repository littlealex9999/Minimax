namespace Minimax
{
    /// <summary>
    /// A representation of the difference in two board states.
    /// </summary>
    public abstract class Move
    {
        /// <summary>
        /// This should call any functions to make this move actually happen.
        /// </summary>
        public abstract void ApplyMove();

        /// <summary>
        /// This should set any variables required to apply the move, and return a board that would result from this move.
        /// </summary>
        /// <param name="board">The board state prior to the move happening</param>
        /// <param name="index">The index of the acting piece, and any other required index</param>
        /// <returns></returns>
        public abstract Board CalculateMove(Board board, params int[] index);
    }
}
