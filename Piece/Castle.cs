using MyBackend.Game;

namespace MyBackend.Piece;

public class Castle : Piece
{
    public Castle(int RowPos, int ColPos, string Color, int Score) : base(RowPos, ColPos, Color, Score)
    {
    }

    public override (List<(int StartRow, int StartCol, int EndRow, int EndCol)> Moves, List<(int StartRow, int StartCol, int EndRow, int EndCol, int Score)> Attacks) GetValidMoves(Board gameBoard)
    {
        var moves = new List<(int, int, int, int)>();
        var attacks = new List<(int, int, int, int, int)>();

        int upCheck = RowPos;
        int downCheck = RowPos;
        int leftCheck = ColPos;
        int rightCheck = ColPos;

        // check up
        if (upCheck > 0)
        {
            while (upCheck > 0)
            {
                upCheck--;
                if (gameBoard.IsOccupied(upCheck, ColPos))
                {
                    if (gameBoard.IsEnemy(upCheck, ColPos))
                    {
                        int score = gameBoard.EnemyScore(upCheck, ColPos);
                        attacks.Add((RowPos, ColPos, upCheck, ColPos, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, upCheck, ColPos));
                }
            }
        }

        // check down
        if (downCheck < 7)
        {
            while (downCheck < 7)
            {
                downCheck++;
                if (gameBoard.IsOccupied(downCheck, ColPos))
                {
                    if (gameBoard.IsEnemy(downCheck, ColPos))
                    {
                        int score = gameBoard.EnemyScore(downCheck, ColPos);
                        attacks.Add((RowPos, ColPos, downCheck, ColPos, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, downCheck, ColPos));
                }
            }
        }

        // check right
        if (rightCheck < 7)
        {
            while (rightCheck < 7)
            {
                rightCheck++;
                if (gameBoard.IsOccupied(RowPos, rightCheck))
                {
                    if (gameBoard.IsEnemy(RowPos, rightCheck))
                    {
                        int score = gameBoard.EnemyScore(RowPos, rightCheck);
                        attacks.Add((RowPos, ColPos, RowPos, rightCheck, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, RowPos, rightCheck));
                }
            }
        }

        // check left
        if (leftCheck > 0)
        {
            while (leftCheck > 0)
            {
                leftCheck--;
                if (gameBoard.IsOccupied(RowPos, leftCheck))
                {
                    if (gameBoard.IsEnemy(RowPos, leftCheck))
                    {
                        int score = gameBoard.EnemyScore(RowPos, leftCheck);
                        attacks.Add((RowPos, ColPos, RowPos, leftCheck, score));
                    }

                    break;
                }
                else
                {
                    moves.Add((RowPos, ColPos, RowPos, leftCheck));
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
            int upCheck = RowPos;
            int downCheck = RowPos;
            int leftCheck = ColPos;
            int rightCheck = ColPos;

            if (upCheck > 0)
            {
                while (upCheck > 0)
                {
                    upCheck--;

                    if (gameBoard.IsOccupied(upCheck, ColPos))
                    {
                        if (!gameBoard.IsEnemy(upCheck, ColPos))
                        {
                            attacks.Add((RowPos, ColPos, upCheck, ColPos));
                        }

                        break;
                    }
                }
            }

            if (downCheck < 7)
            {
                while (downCheck < 7)
                {
                    downCheck++;

                    if (gameBoard.IsOccupied(downCheck, ColPos))
                    {
                        if (!gameBoard.IsEnemy(downCheck, ColPos))
                        {
                            attacks.Add((RowPos, ColPos, downCheck, ColPos));
                        }

                        break;
                    }
                }
            }

            if (rightCheck < 7)
            {
                while (rightCheck < 7)
                {
                    rightCheck++;

                    if (gameBoard.IsOccupied(RowPos, rightCheck))
                    {
                        if (!gameBoard.IsEnemy(RowPos, rightCheck))
                        {
                            attacks.Add((RowPos, ColPos, RowPos, rightCheck));
                        }

                        break;
                    }
                }
            }

            if (leftCheck > 0)
            {
                while (leftCheck > 0)
                {
                    leftCheck--;

                    if (gameBoard.IsOccupied(RowPos, leftCheck))
                    {
                        if (!gameBoard.IsEnemy(RowPos, leftCheck))
                        {
                            attacks.Add((RowPos, ColPos, RowPos, leftCheck));
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

        int upCheck = RowPos;
        int downCheck = RowPos;
        int leftCheck = ColPos;
        int rightCheck = ColPos;

        // check up
        if (upCheck > 0)
        {
            while (upCheck > 0)
            {
                upCheck--;

                if (gameBoard.IsOccupied(upCheck, ColPos))
                {
                    if (gameBoard.PosName(upCheck, ColPos) != "King")
                    {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, upCheck, ColPos));
            }
        }

        // check down
        if (downCheck < 7)
        {
            while (downCheck < 7)
            {
                downCheck++;

                if (gameBoard.IsOccupied(downCheck, ColPos))
                {
                    if (gameBoard.PosName(downCheck, ColPos) != "King")
                    {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, downCheck, ColPos));
            }
        }

        // right check
        if (rightCheck < 7)
        {
            while (rightCheck < 7)
            {
                rightCheck++;

                if (gameBoard.IsOccupied(RowPos, rightCheck))
                {
                    if (gameBoard.PosName(RowPos, rightCheck) != "King") {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, RowPos, rightCheck));
            }
        }

        // left check
        if (leftCheck > 0)
        {
            while (leftCheck > 0)
            {
                leftCheck--;

                if (gameBoard.IsOccupied(RowPos, leftCheck))
                {
                    if (gameBoard.PosName(RowPos, leftCheck) != "King")
                    {
                        break;
                    }
                }

                paths.Add((RowPos, ColPos, RowPos, leftCheck));
            }
        }

        return paths;
    }
}