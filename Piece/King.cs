using MyBackend.Game;

namespace MyBackend.Piece;

public class King : Piece
{
    public King(int rowPos, int colPos, string color, int score) : base(rowPos, colPos, color, score)
    {
    }

    public override (List<(int StartRow, int StartCol, int EndRow, int EndCol)> Moves, List<(int StartRow, int StartCol, int EndRow, int EndCol, int Score)> Attacks) GetValidMoves(Board gameBoard)
    {
        var moves = new List<(int, int, int, int)>();
        var attacks = new List<(int, int, int, int, int)>();

        // ERROR HERE

        (int, int)[] offsets = new (int, int)[]
        {
            (0, 1), (0, -1), (1, 0), (-1, 0),
            (1, 1), (-1, 1), (1, -1), (-1, -1)
        };

        foreach (var offset in offsets)
        {
            int newRow = RowPos + offset.Item1;
            int newCol = ColPos + offset.Item2;

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
            {
                if (!gameBoard.IsOccupied(newRow, newCol))
                {
                    moves.Add((RowPos, ColPos, newRow, newCol));

                }
                else if (gameBoard.IsEnemy(newRow, newCol))
                {
                    int score = gameBoard.GameBoard[newRow, newCol]!.Score;
                    attacks.Add((RowPos, ColPos, newRow, newCol, score));
                }
            }
        }

        return (moves, attacks);
    }

    public override List<(int StartRow, int StartCol, int EndRow, int EndCol)> GetEnemyAttacks(Board gameBoard)
    {
        var attacks = new List<(int, int, int, int)>();

        (int, int)[] offsets = new (int, int)[]
        {
            (0, 1), (0, -1), (1, 0), (-1, 0),
            (1, 1), (-1, 1), (1, -1), (-1, -1)
        };

        if (gameBoard.IsEnemy(RowPos, ColPos))
        {
            foreach (var offset in offsets)
            {
                int newRow = RowPos + offset.Item1;
                int newCol = ColPos + offset.Item2;

                if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
                {
                    if (gameBoard.IsOccupied(newRow, newCol))
                    {
                        if (!gameBoard.IsEnemy(newRow, newCol))
                        {
                            attacks.Add((RowPos, ColPos, newRow, newCol));
                        }
                    }
                }
            }
        }

        return attacks;
    }

    public override List<(int StartRow, int StartCol, int EndRow, int EndCol)> GetEnemyAttackPaths(Board gameBoard)
    {
        var paths = new List<(int StartRow, int StartCol, int EndRow, int EndCol)>();

        (int, int)[] offsets = new (int, int)[]
        {
            (0, 1), (0, -1), (1, 0), (-1, 0),
            (1, 1), (-1, 1), (1, -1), (-1, -1)
        };

        foreach (var offset in offsets)
        {
            int newRow = RowPos + offset.Item1;
            int newCol = ColPos + offset.Item2;

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
            {
                if (!gameBoard.IsOccupied(newRow, newCol))
                {
                    paths.Add((RowPos, ColPos, newRow, newCol));
                }
            }
        }

        return paths;
    }
}