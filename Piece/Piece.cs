using MyBackend.Game;

namespace MyBackend.Piece;

public abstract class Piece
{
    public int RowPos { get; set; }
    public int ColPos { get; set; }
    public string Color { get; set; }
    public int Score { get; set; }

    public Piece(int rowPos, int colPos, string color, int score)
    {
        RowPos = rowPos;
        ColPos = colPos;
        Color = color;
        Score = score;
    }

    // before moving on, look into how inherited class functions work
    // need to be able to check moves from all pieces on the board
    // how does this work?

    // public abstract List<(int, int)> GetValidMoves(Piece[,] board);
    // public abstract List<(int, int)> GetValidMoves(Board gameBoard);
    public abstract (List<(int StartRow, int StartCol, int EndRow, int EndCol)> Moves, List<(int StartRow, int StartCol, int EndRow, int EndCol, int Score)> Attacks) GetValidMoves(Board gameBoard);

    // need to get the potential attack moves of enemies to plan for defence
    public abstract List<(int StartRow, int StartCol, int EndRow, int EndCol)> GetEnemyAttacks(Board gameBoard);

    public abstract List<(int StartRow, int StartCol, int EndRow, int EndCol)> GetEnemyAttackPaths(Board gameBoard);

};