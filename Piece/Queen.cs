using MyBackend.Game;

namespace MyBackend.Piece;

public class Queen : Piece
{
    public Queen(int rowPos, int colPos, string color, int score) : base(rowPos, colPos, color, score)
    {
    }

    // public override List<(int, int)> GetValidMoves(Piece[,] board)
    public override (List<(int StartRow, int StartCol, int EndRow, int EndCol)> Moves, List<(int StartRow, int StartCol, int EndRow, int EndCol, int Score)> Attacks) GetValidMoves(Board gameBoard)
    {
        var moves = new List<(int, int, int, int)>();
        var attacks = new List<(int, int, int, int, int)>();

        int currentRow = RowPos;
        int currentCol = ColPos;

        // up right check
        if (currentCol < 7 && currentRow > 0)
        {
            while (currentCol < 7 && currentRow > 0)
            {
                currentCol += 1;
                currentRow -= 1;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.IsEnemy(currentRow, currentCol))
                    {
                        int score = gameBoard.EnemyScore(currentRow, currentCol);
                        attacks.Add((RowPos, ColPos, currentRow, currentCol, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, currentRow, currentCol));
                }
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // up left check
        if (currentCol > 0 && currentRow > 0)
        {
            while (currentCol > 0 && currentRow > 0)
            {
                currentCol -= 1;
                currentRow -= 1;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.IsEnemy(currentRow, currentCol))
                    {
                        int score = gameBoard.EnemyScore(currentRow, currentCol);
                        attacks.Add((RowPos, ColPos, currentRow, currentCol, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, currentRow, currentCol));
                }
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // down right check
        if (currentRow < 7 && currentCol < 7)
        {
            while (currentRow < 7 && currentCol < 7)
            {
                currentCol += 1;
                currentRow += 1;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.IsEnemy(currentRow, currentCol))
                    {
                        int score = gameBoard.EnemyScore(currentRow, currentCol);
                        attacks.Add((RowPos, ColPos, currentRow, currentCol, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, currentRow, currentCol));
                }
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // down left check
        if (currentRow < 7 && currentCol > 0)
        {
            while (currentRow < 7 && currentCol > 0)
            {
                currentCol -= 1;
                currentRow += 1;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.IsEnemy(currentRow, currentCol))
                    {
                        int score = gameBoard.EnemyScore(currentRow, currentCol);
                        attacks.Add((RowPos, ColPos, currentRow, currentCol, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, currentRow, currentCol));
                }
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // right check
        if (currentCol < 7)
        {
            while (currentCol < 7)
            {
                currentCol++;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.IsEnemy(currentRow, currentCol))
                    {
                        int score = gameBoard.EnemyScore(currentRow, currentCol);
                        attacks.Add((RowPos, ColPos, currentRow, currentCol, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, currentRow, currentCol));
                }
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // left check
        if (currentCol > 0)
        {
            while (currentCol > 0)
            {
                currentCol--;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.IsEnemy(currentRow, currentCol))
                    {
                        int score = gameBoard.EnemyScore(currentRow, currentCol);
                        attacks.Add((RowPos, ColPos, currentRow, currentCol, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, currentRow, currentCol));
                }
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // up check
        if (currentRow > 0)
        {
            while (currentRow > 0)
            {
                currentRow--;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.IsEnemy(currentRow, currentCol))
                    {
                        int score = gameBoard.EnemyScore(currentRow, currentCol);
                        attacks.Add((RowPos, ColPos, currentRow, currentCol, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, currentRow, currentCol));
                }
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // down check
        if (currentRow < 7)
        {
            while (currentRow < 7)
            {
                currentRow++;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.IsEnemy(currentRow, currentCol))
                    {
                        int score = gameBoard.EnemyScore(currentRow, currentCol);
                        attacks.Add((RowPos, ColPos, currentRow, currentCol, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, currentRow, currentCol));
                }
            }
        }

        return (moves, attacks);
    }

    public override List<(int StartRow, int StartCol, int EndRow, int EndCol)> GetEnemyAttacks(Board gameBoard)
    {
        var attacks = new List<(int, int, int, int)>();

        if (gameBoard.IsEnemy(RowPos, ColPos))
        {
            int currentRow = RowPos;
            int currentCol = ColPos;

            // up check
            if (currentRow > 0)
            {
                while (currentRow > 0)
                {
                    currentRow--;

                    if (gameBoard.IsOccupied(currentRow, currentCol))
                    {
                        if (!gameBoard.IsEnemy(currentRow, currentCol))
                        {
                            attacks.Add((RowPos, ColPos, currentRow, currentCol));
                        }

                        break;
                    }
                }
            }

            currentRow = RowPos;
            currentCol = ColPos;

            // right check
            if (currentCol < 7)
            {
                while (currentCol < 7)
                {
                    currentCol++;

                    if (gameBoard.IsOccupied(currentRow, currentCol))
                    {
                        if (!gameBoard.IsEnemy(currentRow, currentCol))
                        {
                            attacks.Add((RowPos, ColPos, currentRow, currentCol));
                        }

                        break;
                    }
                }
            }

            currentRow = RowPos;
            currentCol = ColPos;

            // down check
            if (currentRow < 7)
            {
                while (currentRow < 7)
                {
                    currentRow++;

                    if (gameBoard.IsOccupied(currentRow, currentCol))
                    {
                        if (!gameBoard.IsEnemy(currentRow, currentCol))
                        {
                            attacks.Add((RowPos, ColPos, currentRow, currentCol));
                        }

                        break;
                    }
                }
            }

            currentRow = RowPos;
            currentCol = ColPos;

            // left check
            if (currentCol > 0)
            {
                while (currentCol > 0)
                {
                    currentCol--;

                    if (gameBoard.IsOccupied(currentRow, currentCol))
                    {
                        if (!gameBoard.IsEnemy(currentRow, currentCol))
                        {
                            attacks.Add((RowPos, ColPos, currentRow, currentCol));
                        }

                        break;
                    }
                }
            }

            currentRow = RowPos;
            currentCol = ColPos;

            // up right check
            if (currentRow > 0 && currentCol < 7)
            {
                while (currentRow > 0 && currentCol < 7)
                {
                    currentRow--;
                    currentCol++;

                    if (gameBoard.IsOccupied(currentRow, currentCol))
                    {
                        if (!gameBoard.IsEnemy(currentRow, currentCol))
                        {
                            attacks.Add((RowPos, ColPos, currentRow, currentCol));
                        }

                        break;
                    }
                }
            }

            currentRow = RowPos;
            currentCol = ColPos;

            // down right check
            if (currentRow < 7 && currentCol < 7)
            {
                while (currentRow < 7 && currentCol < 7)
                {
                    currentCol++;
                    currentRow++;

                    if (gameBoard.IsOccupied(currentRow, currentCol))
                    {
                        if (!gameBoard.IsEnemy(currentRow, currentCol))
                        {
                            attacks.Add((RowPos, ColPos, currentRow, currentCol));
                        }

                        break;
                    }
                }
            }

            currentRow = RowPos;
            currentCol = ColPos;

            // up left check
            if (currentRow > 0 && currentCol > 0)
            {
                while (currentRow > 0 && currentCol > 0)
                {
                    currentCol--;
                    currentRow--;

                    if (gameBoard.IsOccupied(currentRow, currentCol))
                    {
                        if (!gameBoard.IsEnemy(currentRow, currentCol))
                        {
                            attacks.Add((RowPos, ColPos, currentRow, currentCol));
                        }

                        break;
                    }
                }
            }

            currentRow = RowPos;
            currentCol = ColPos;

            // down left check
            if (currentRow < 7 && currentCol > 0)
            {
                while (currentRow < 7 && currentCol > 0)
                {
                    currentRow++;
                    currentCol--;

                    if (gameBoard.IsOccupied(currentRow, currentCol))
                    {
                        if (!gameBoard.IsEnemy(currentRow, currentCol))
                        {
                            attacks.Add((RowPos, ColPos, currentRow, currentCol));
                        }

                        break;
                    }
                }
            }
        }


        return attacks;
    }

    public override List<(int StartRow, int StartCol, int EndRow, int EndCol)> GetEnemyAttackPaths(Board gameBoard)
    {
        var paths = new List<(int StartRow, int StartCol, int EndRow, int EndCol)>();

        int currentRow = RowPos;
        int currentCol = ColPos;

        // up check
        if (currentRow > 0)
        {
            while (currentRow > 0)
            {
                currentRow--;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.PosName(currentRow, currentCol) != "King") {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, currentRow, currentCol));
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // down check
        if (currentRow < 7)
        {
            while (currentRow < 7)
            {
                currentRow++;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.PosName(currentRow, currentCol) != "King") {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, currentRow, currentCol));
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // left check
        if (currentCol > 0)
        {
            while (currentCol > 0)
            {
                currentCol--;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.PosName(currentRow, currentCol) != "King") {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, currentRow, currentCol));
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // right check
        if (currentCol < 7)
        {
            while (currentCol < 7)
            {
                currentCol++;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.PosName(currentRow, currentCol) != "King") {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, currentRow, currentCol));
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // up right check
        if (currentCol < 7 && currentRow > 0)
        {
            while (currentCol < 7 && currentRow > 0)
            {
                currentCol++;
                currentRow--;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.PosName(currentRow, currentCol) != "King") {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, currentRow, currentCol));
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // up left check
        if (currentCol > 0 && currentRow > 0)
        {
            while (currentCol > 0 && currentRow > 0)
            {
                currentCol--;
                currentRow--;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.PosName(currentRow, currentCol) != "King") {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, currentRow, currentCol));
            }
        }
        
        currentRow = RowPos;
        currentCol = ColPos;

        // down right check
        if (currentCol < 7 && currentRow < 7)
        {
            while (currentCol < 7 && currentRow < 7)
            {
                currentCol++;
                currentRow++;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.PosName(currentRow, currentCol) != "King") {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, currentRow, currentCol));
            }
        }

        currentRow = RowPos;
        currentCol = ColPos;

        // down left check
        if (currentCol > 0 && currentRow < 7)
        {
            while (currentCol > 0 && currentRow < 7)
            {
                currentCol--;
                currentRow++;

                if (gameBoard.IsOccupied(currentRow, currentCol))
                {
                    if (gameBoard.PosName(currentRow, currentCol) != "King") {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, currentRow, currentCol));
            }
        }


        return paths;
    }
    
}
