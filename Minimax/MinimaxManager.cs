using System.Collections.Generic;

namespace Minimax
{
    public sealed class MinimaxManager
    {
        public MinimaxManager(Board b)
        {
            currentBoard = b;
            decidedNextBoard = b; // prevents warnings. will cause problems if applying the move prior to changing this
        }

        /// <summary>
        /// Will perform minimax calculations and then apply the best move
        /// </summary>
        /// <param name="depth">how many plies ahead should be calculated</param>
        /// <param name="maximizingPlayer">which player is the maximizing player</param>
        /// <param name="useAlphaBeta">if the minimax calculations should use alpha-beta pruning or not</param>
        public void DoMinimax(int depth, int maximizingPlayer, bool useAlphaBeta = true)
        {
            Board b;

            if (useAlphaBeta) MinimaxCalcAB(currentBoard, depth, maximizingPlayer, out b);
            else MinimaxCalc(currentBoard, depth, maximizingPlayer, out b);

            decidedNextBoard = b;
            ApplyNextMove();
        }

        public Board currentBoard;
        public Board decidedNextBoard;

        /// <summary>
        /// Alpha-Beta Minimax.
        /// Will return a heuristic which indicates the "best" scenario stemming from the given board.
        /// Outs a board that would result from the optimal move.
        /// </summary>
        /// <param name="board">the board for calculations to be performed on</param>
        /// <param name="depth">how many plies ahead should be calculated</param>
        /// <param name="maximizingPlayer">which player is the maximizing player</param>
        /// <param name="alpha">the cutoff for alpha-beta pruning. should not be changed without caution</param>
        /// <param name="beta">the cutoff for alpha-beta pruning. should not be changed without caution</param>
        /// <returns></returns>
        public float MinimaxCalcAB(Board board, int depth, int maximizingPlayer, out Board idealBoard, float alpha = float.NegativeInfinity, float beta = float.PositiveInfinity)
        {
            idealBoard = board;
            if (depth <= 0) return board.Heuristic();
            float h = board.Heuristic();
            if (h == float.NegativeInfinity || h == float.PositiveInfinity) return h;

            List<Board> potentialBoards = GetPotentialMoveList(board);
            if (potentialBoards.Count == 0) return h; // no possible moves

            idealBoard = potentialBoards[0];

            float value;
            if (board.playerPly == maximizingPlayer) value = float.NegativeInfinity;
            else value = float.PositiveInfinity;

            foreach (Board pgs in potentialBoards) {
                if (board.playerPly == maximizingPlayer) {
                    float tempVal = MinimaxCalcAB(pgs, depth - 1, maximizingPlayer, out Board temp, alpha, beta);

                    if (tempVal > value) {
                        value = tempVal;
                        idealBoard = pgs;

                        if (value > alpha) alpha = value;
                        if (value >= beta) break; // beta cutoff
                    }
                } else { // minimizing player
                    float tempVal = MinimaxCalcAB(pgs, depth - 1, maximizingPlayer, out Board temp, alpha, beta);

                    if (tempVal < value) {
                        value = tempVal;
                        idealBoard = pgs;

                        if (value > beta) beta = value;
                        if (value <= alpha) break; // alpha cutoff
                    }
                }
            }

            return value; // all calculations are complete in this branch, so a score is being returned
        }

        /// <summary>
        /// Will return a heuristic which indicates the "best" scenario stemming from the given board.
        /// Outs a board that would result from the optimal move.
        /// </summary>
        /// <param name="board">the board for calculations to be performed on</param>
        /// <param name="depth">how many plies ahead should be calculated</param>
        /// <param name="maximizingPlayer">which player is the maximizing player</param>
        /// <returns></returns>
        public float MinimaxCalc(Board board, int depth, int maximizingPlayer, out Board idealBoard)
        {
            idealBoard = board;
            if (depth <= 0) return board.Heuristic();
            float h = board.Heuristic();
            if (h == float.NegativeInfinity || h == float.PositiveInfinity) return h;

            List<Board> potentialBoards = GetPotentialMoveList(board);
            idealBoard = potentialBoards[0];

            float value;
            if (board.playerPly == maximizingPlayer) value = float.NegativeInfinity;
            else value = float.PositiveInfinity;

            foreach (Board pgs in potentialBoards) {
                if (board.playerPly == maximizingPlayer) {
                    float tempVal = MinimaxCalc(pgs, depth - 1, maximizingPlayer, out Board temp);

                    if (tempVal > value) {
                        value = tempVal;
                        idealBoard = pgs;
                    }
                } else { // minimizing player
                    float tempVal = MinimaxCalc(pgs, depth - 1, maximizingPlayer, out Board temp);

                    if (tempVal < value) {
                        value = tempVal;
                        idealBoard = pgs;
                    }
                }
            }

            return value; // all calculations are complete in this branch, so a score is being returned
        }

        /// <summary>
        /// Applies the next move and updates the internal board state.
        /// </summary>
        public void ApplyNextMove()
        {
            decidedNextBoard.lastMove.ApplyMove();
            currentBoard = decidedNextBoard;
        }

        private List<Board> GetPotentialMoveList(Board board)
        {
            List<Board> potentialBoards = new List<Board>();

            for (int i = 0; i < board.width * board.height; ++i) {
                Board[] possibleMoves = board.CalculateAllMoves(i);
                bool differentBoards = false;

                foreach (Board pgs in possibleMoves) {
                    for (int j = 0; j < board.width * board.height; ++j) {
                        if (pgs.grid[j] != board.grid[j]) {
                            differentBoards = true;
                            break;
                        }
                    }

                    if (differentBoards) {
                        bool add = true;

                        foreach (Board b in potentialBoards) {
                            if (pgs.Equal(b)) {
                                add = false;
                                break;
                            }
                        }

                        if (add) potentialBoards.Add(pgs);
                    }
                }
            }

            return potentialBoards;
        }
    }
}
