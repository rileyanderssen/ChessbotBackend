using Microsoft.AspNetCore.Mvc;
using MyBackend.Game;

namespace MyBackend.Piece;

public class Pawn : Piece
{
    public Pawn(int RowPos, int ColPos, string Color, int Score) : base(RowPos, ColPos, Color, Score)
    {
    }

    public override (List<(int StartRow, int StartCol, int EndRow, int EndCol)> Moves, List<(int StartRow, int StartCol, int EndRow, int EndCol, int Score)> Attacks) GetValidMoves(Board gameBoard)
    {
        var moves = new List<(int, int, int, int)>();
        var attacks = new List<(int, int, int, int, int)>();

        // AI is always black, so first row for pawn is always 1
        // if pawn in start row, two movements can be made (or attack)
        if (RowPos == 1)
        {
            (int, int)[] offsets = new (int, int)[]
            {
                (1, 0), (2, 0)
            };

            foreach (var offset in offsets)
            {
                int newRow = RowPos + offset.Item1;
                if (newRow >= 0 && newRow < 8)
                {
                    if (!gameBoard.IsOccupied(newRow, ColPos))
                    {
                        moves.Add((RowPos, ColPos, newRow, ColPos));
                    }
                }
            }
        }
        else if (RowPos > 1)
        {
            int newRow = RowPos + 1;
            if (newRow >= 0 && newRow < 8)
            {
                if (!gameBoard.IsOccupied(newRow, ColPos))
                {
                    moves.Add((RowPos, ColPos, newRow, ColPos));
                }
            }
        }

        // now check attacks
        (int, int)[] offsetsAttack = new (int, int)[]
        {
            (1, 1), (1, -1)
        };

        foreach (var offset in offsetsAttack)
        {
            int newRow = RowPos + offset.Item1;
            int newCol = ColPos + offset.Item2;

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
            {
                if (gameBoard.IsEnemy(newRow, newCol))
                {
                    int score = gameBoard.EnemyScore(newRow, newCol);
                    attacks.Add((RowPos, ColPos, newRow, newCol, score));
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
            (int, int)[] offsetsAttack = new (int, int)[]
            {
                (-1, 1), (-1, -1)
            };

            foreach (var offset in offsetsAttack)
            {
                int newRow = RowPos + offset.Item1;
                int newCol = ColPos + offset.Item2;

                if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
                {
                    attacks.Add((RowPos, ColPos, newRow, newCol));
                }
            }
        }

        return attacks;
    }

    public override List<(int StartRow, int StartCol, int EndRow, int EndCol)> GetEnemyAttackPaths(Board gameBoard)
    {
        var paths = new List<(int StartRow, int StartCol, int EndRow, int EndCol)>();

        (int, int)[] offsetsAttack = new (int, int)[]
        {
            (-1, 1), (-1, -1)
        };

        foreach (var offset in offsetsAttack)
        {
            int newRow = RowPos + offset.Item1;
            int newCol = ColPos + offset.Item2;

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
            {
                paths.Add((RowPos, ColPos, newRow, newCol));
            }
            
        }


        return paths;
    }
} 