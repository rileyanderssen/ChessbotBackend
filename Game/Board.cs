namespace MyBackend.Game;

using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Piece;

// add isOccupied helper
// isEnemy helper (get score?)
// etc

public class Board
{
    public Piece?[,] GameBoard { get; set; } = new Piece?[8, 8];

    public Board()
    {
        GameBoard = null!;
    }

    public bool IsOccupied(int row, int col)
    {
        if (GameBoard[row, col] != null)
        {
            return true;
        }

        return false;
    }

    public bool IsEnemy(int row, int col)
    {
        // since AI will always be black (top half of board), enemy is always white
        return GameBoard[row, col]?.Color == "WHITE";
    }

    public Board CopyBoard()
    {
        Board newBoard = new();
        Piece?[,] copiedBoard = new Piece?[8, 8];

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                copiedBoard[row, col] = GameBoard[row, col];
            }
        }

        newBoard.GameBoard = copiedBoard;
        return newBoard;
    }
    public int EnemyScore(int row, int col)
    {
        return GameBoard[row, col]!.Score;
    }

    public string PosName(int row, int col)
    {
        return GameBoard[row, col]!.GetType().Name;
    }

    public (int, int) KingPos(string team)
    {
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (IsOccupied(row, col))
                {
                    if (GameBoard[row, col]!.GetType().Name == "King")
                    {
                        if (GameBoard[row, col]!.Color == team)
                        {
                            return (row, col);
                        }
                    }
                }

            }
        }

        return (99, 99);
    }

    public bool IsPosSafe(int row, int col)
    {
        var listOfPaths = new List<List<(int StartRow, int StartCol, int EndRow, int EndCol)>>();
        var listOfAttacks = new List<List<(int StartRow, int StartCol, int EndRow, int EndCol)>>();
        var listOfAttackScore = new List<List<(int StartRow, int StartCol, int EndRow, int EndCol, int Score)>>();

        for (int checkRow = 0; checkRow < 8; checkRow++)
        {
            for (int checkCol = 0; checkCol < 8; checkCol++)
            {
                if (IsOccupied(checkRow, checkCol))
                {
                    if (IsEnemy(checkRow, checkCol))
                    {
                        var (_, attacksScore) = GameBoard[checkRow, checkCol]!.GetValidMoves(this);
                        listOfAttackScore.Add(attacksScore);

                        var attacks = GameBoard[checkRow, checkCol]!.GetEnemyAttacks(this);
                        listOfAttacks.Add(attacks);

                        var paths = GameBoard[checkRow, checkCol]!.GetEnemyAttackPaths(this);
                        listOfPaths.Add(paths);
                    }
                }
            }
        }

        for (int i = 0; i < listOfPaths.Count; i++)
        {
            for (int j = 0; j < listOfPaths[i].Count; j++)
            {
                if (listOfPaths[i][j].EndRow == row && listOfPaths[i][j].EndCol == col)
                {
                    return false;
                }
            }
        }

        for (int i = 0; i < listOfAttacks.Count; i++)
        {
            for (int j = 0; j < listOfAttacks[i].Count; j++)
            {
                if (listOfAttacks[i][j].EndRow == row && listOfAttacks[i][j].EndCol == col)
                {
                    return false;
                }
            }
        }

        for (int i = 0; i < listOfAttackScore.Count; i++)
        {
            for (int j = 0; j < listOfAttackScore[i].Count; j++)
            {
                if (listOfAttackScore[i][j].EndRow == row && listOfAttackScore[i][j].EndCol == col)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsEnemyPosSafe(int row, int col)
    {
        var listOfPaths = new List<List<(int StartRow, int StartCol, int EndRow, int EndCol)>>();
        var listOfAttacks = new List<List<(int StartRow, int StartCol, int EndRow, int EndCol, int Score)>>();

        for (int checkRow = 0; checkRow < 8; checkRow++)
        {
            for (int checkCol = 0; checkCol < 8; checkCol++)
            {
                if (IsOccupied(checkRow, checkCol))
                {
                    if (!IsEnemy(checkRow, checkCol))
                    {
                        var (_, attacks) = GameBoard[checkRow, checkCol]!.GetValidMoves(this);

                        if (attacks.Count > 0)
                        {
                            listOfAttacks.Add(attacks);
                        }

                        var paths = GameBoard[checkRow, checkCol]!.GetEnemyAttacks(this);

                        if (paths.Count > 0)
                        {
                            listOfPaths.Add(paths);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < listOfPaths.Count; i++)
        {
            for (int j = 0; j < listOfPaths[i].Count; j++)
            {
                if (listOfPaths[i][j].EndRow == row && listOfPaths[i][j].EndCol == col)
                {
                    return false;
                }
            }
        }

        for (int i = 0; i < listOfAttacks.Count; i++)
        {
            for (int j = 0; j < listOfAttacks[i].Count; j++)
            {
                if (listOfAttacks[i][j].EndRow == row && listOfAttacks[i][j].EndCol == col)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void ProcessBoard(string[][] chessBoard)
    {
        Piece?[,] board = new Piece[8, 8];

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (chessBoard[i][j] == ".")
                {
                    board[i, j] = null;
                }

                if (chessBoard[i][j][0] == 'w')
                {
                    if (chessBoard[i][j][1] == 'p')
                    {
                        board[i, j] = new Pawn(i, j, "WHITE", 1);
                    }
                    else if (chessBoard[i][j][1] == 'c')
                    {
                        board[i, j] = new Castle(i, j, "WHITE", 3);
                    }
                    else if (chessBoard[i][j][1] == 'h')
                    {
                        board[i, j] = new Horse(i, j, "WHITE", 2);
                    }
                    else if (chessBoard[i][j][1] == 'k')
                    {
                        board[i, j] = new King(i, j, "WHITE", 99);
                    }
                    else if (chessBoard[i][j][1] == 'q')
                    {
                        board[i, j] = new Queen(i, j, "WHITE", 10);
                    }
                    else if (chessBoard[i][j][1] == 'b')
                    {
                        board[i, j] = new Bishop(i, j, "WHITE", 4);
                    }
                }
                else if (chessBoard[i][j][0] == 'b')
                {
                    if (chessBoard[i][j][1] == 'p')
                    {
                        board[i, j] = new Pawn(i, j, "BLACK", 1);
                    }
                    else if (chessBoard[i][j][1] == 'c')
                    {
                        board[i, j] = new Castle(i, j, "BLACK", 3);
                    }
                    else if (chessBoard[i][j][1] == 'h')
                    {
                        board[i, j] = new Horse(i, j, "BLACK", 2);
                    }
                    else if (chessBoard[i][j][1] == 'k')
                    {
                        board[i, j] = new King(i, j, "BLACK", 99);
                    }
                    else if (chessBoard[i][j][1] == 'q')
                    {
                        board[i, j] = new Queen(i, j, "BLACK", 10);
                    }
                    else if (chessBoard[i][j][1] == 'b')
                    {
                        board[i, j] = new Bishop(i, j, "BLACK", 4);
                    }
                }
            }
        }

        GameBoard = board;
    }

    public (int, int, int) HighestPieceInDanger(Board GameBoard)
    {
        int highestScore = -1;
        int dangerRow = -1;
        int dangerCol = -1;
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (IsOccupied(row, col))
                {
                    if (!IsEnemy(row, col) && !IsPosSafe(row, col))
                    {
                        Console.WriteLine("NEXT IF PASSED");
                        if (GameBoard.GameBoard[row, col]!.Score > highestScore)
                        {
                            highestScore = GameBoard.GameBoard[row, col]!.Score;
                            dangerRow = row;
                            dangerCol = col;
                        }
                    }
                }
            }
        }

        return (highestScore, dangerRow, dangerCol);
    }

    public (int, int, int, int) DetermineSafetyMove(int pieceRow, int pieceCol)
    {
        var (validMoves, _) = GameBoard[pieceRow, pieceCol]!.GetValidMoves(this);

        // first, check if the piece can attack the piece putting it in danger
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (IsOccupied(row, col))
                {
                    if (IsEnemy(row, col))
                    {
                        // var (_, attacks) = GameBoard[row, col]!.GetValidMoves(this);
                        var attacks = GameBoard[row, col]!.GetEnemyAttackPaths(this);
                        for (int i = 0; i < attacks.Count; i++)
                        {
                            if (attacks[i].EndRow == pieceRow && attacks[i].EndCol == pieceCol)
                            {
                                var (_, ourAttacks) = GameBoard[pieceRow, pieceCol]!.GetValidMoves(this);
                                for (int j = 0; j < ourAttacks.Count; j++)
                                {
                                    if (ourAttacks[j].EndRow == attacks[i].StartRow && ourAttacks[j].EndCol == attacks[i].StartCol)
                                    {
                                        if (GameBoard[ourAttacks[j].EndRow, ourAttacks[j].EndCol]!.Score > GameBoard[pieceRow, pieceCol]!.Score)
                                        {
                                            return (pieceRow, pieceCol, ourAttacks[j].EndRow, ourAttacks[j].EndCol);
                                        }
                                        else
                                        {
                                            Board testBoard = CopyBoard();
                                            testBoard.GameBoard[ourAttacks[j].EndRow, ourAttacks[j].EndCol] = testBoard.GameBoard[pieceRow, pieceCol];
                                            testBoard.GameBoard[pieceRow, pieceCol] = null;
                                            testBoard.GameBoard[ourAttacks[j].EndRow, ourAttacks[j].EndCol]!.RowPos = ourAttacks[j].EndRow;
                                            testBoard.GameBoard[ourAttacks[j].EndRow, ourAttacks[j].EndCol]!.ColPos = ourAttacks[j].EndCol;

                                            if (testBoard.IsPosSafe(ourAttacks[j].EndRow, ourAttacks[j].EndCol))
                                            {
                                                return (pieceRow, pieceCol, ourAttacks[j].EndRow, ourAttacks[j].EndCol);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // second, check if any of our pieces can take the attacking piece
        // if it is not safe, ensure the score is valuable
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (IsOccupied(row, col))
                {
                    if (IsEnemy(row, col))
                    {
                        var enemyAttacks = GameBoard[row, col]!.GetEnemyAttackPaths(this);
                        if (enemyAttacks.Count > 0)
                        {
                            for (int i = 0; i < enemyAttacks.Count; i++)
                            {
                                if (enemyAttacks[i].EndRow == pieceRow && enemyAttacks[i].EndCol == pieceCol)
                                {
                                    for (int teamRow = 0; teamRow < 8; teamRow++)
                                    {
                                        for (int teamCol = 0; teamCol < 8; teamCol++)
                                        {
                                            var (_, friendlyAttacks) = GameBoard[row, col]!.GetValidMoves(this);
                                            if (friendlyAttacks.Count > 0)
                                            {
                                                for (int j = 0; j < friendlyAttacks.Count; j++)
                                                {
                                                    if (friendlyAttacks[j].EndRow == enemyAttacks[i].StartRow && friendlyAttacks[j].EndCol == enemyAttacks[i].StartCol)
                                                    {
                                                        // first, test if it is safe
                                                        Board testBoard = CopyBoard();
                                                        testBoard.GameBoard[friendlyAttacks[j].EndRow, friendlyAttacks[j].EndCol] = testBoard.GameBoard[friendlyAttacks[j].StartRow, friendlyAttacks[j].StartCol];
                                                        testBoard.GameBoard[friendlyAttacks[j].StartRow, friendlyAttacks[j].StartCol] = null;

                                                        testBoard.GameBoard[friendlyAttacks[j].EndRow, friendlyAttacks[j].EndCol]!.RowPos = friendlyAttacks[j].EndRow;
                                                        testBoard.GameBoard[friendlyAttacks[j].EndRow, friendlyAttacks[j].EndCol]!.ColPos = friendlyAttacks[j].EndCol;

                                                        if (testBoard.IsPosSafe(friendlyAttacks[j].EndRow, friendlyAttacks[j].EndCol))
                                                        {
                                                            return (friendlyAttacks[j].StartRow, friendlyAttacks[j].StartCol, friendlyAttacks[j].EndRow, friendlyAttacks[j].EndCol);
                                                        }
                                                        else
                                                        {
                                                            // if not safe, check if trade is valuable
                                                            if (GameBoard[friendlyAttacks[j].StartRow, friendlyAttacks[j].StartCol]!.Score < GameBoard[friendlyAttacks[j].EndRow, friendlyAttacks[j].EndCol]!.Score)
                                                            {
                                                                return (friendlyAttacks[j].StartRow, friendlyAttacks[j].StartCol, friendlyAttacks[j].EndRow, friendlyAttacks[j].EndCol);
                                                            }
                                                            else if (GameBoard[friendlyAttacks[j].StartRow, friendlyAttacks[j].StartCol]!.Score == GameBoard[friendlyAttacks[j].EndRow, friendlyAttacks[j].EndCol]!.Score)
                                                            {
                                                                // workshopping -> if scores are equal, also do the attack
                                                                return (friendlyAttacks[j].StartRow, friendlyAttacks[j].StartCol, friendlyAttacks[j].EndRow, friendlyAttacks[j].EndCol);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        // third, check if a safe move can be made that puts the piece targeting another player
        for (int i = 0; i < validMoves.Count; i++)
        {
            if (IsPosSafe(validMoves[i].EndRow, validMoves[i].EndCol))
            {
                Board testBoard = CopyBoard();
                int newRow = validMoves[i].EndRow;
                int newCol = validMoves[i].EndCol;
                testBoard.GameBoard[newRow, newCol] = testBoard.GameBoard[pieceRow, pieceCol];
                testBoard.GameBoard[pieceRow, pieceCol] = null;
                testBoard.GameBoard[newRow, newCol]!.RowPos = newRow;
                testBoard.GameBoard[newRow, newCol]!.ColPos = newCol;

                var (_, attacks) = testBoard.GameBoard[newRow, newCol]!.GetValidMoves(testBoard);
                if (attacks.Count > 0)
                {
                    return (pieceRow, pieceCol, newRow, newCol);
                }
            }
        }

        // forth, if the piece targeting the player can be attacked, attack it
        var (_, lastStandAttacks) = GameBoard[pieceRow, pieceCol]!.GetValidMoves(this);
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (IsOccupied(row, col))
                {
                    if (IsEnemy(row, col))
                    {
                        var (_, enemyAttacks) = GameBoard[row, col]!.GetValidMoves(this);
                        for (int i = 0; i < enemyAttacks.Count; i++)
                        {
                            if (enemyAttacks[i].EndRow == pieceRow && enemyAttacks[i].EndCol == pieceCol)
                            {
                                for (int j = 0; j < lastStandAttacks.Count; j++)
                                {
                                    if (lastStandAttacks[j].EndRow == enemyAttacks[i].StartRow && lastStandAttacks[j].EndCol == enemyAttacks[i].StartCol)
                                    {
                                        Console.WriteLine("Performing last stand attack");
                                        return (lastStandAttacks[j].StartRow, lastStandAttacks[j].StartCol, lastStandAttacks[j].EndRow, lastStandAttacks[j].EndCol);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // if no attacks can be made, make the first possible safe move
        Console.WriteLine("Doing final return from saftey move");
        for (int i = 0; i < validMoves.Count; i++)
        {
            if (IsPosSafe(validMoves[i].EndRow, validMoves[i].EndCol))
            {
                int newRow = validMoves[i].EndRow;
                int newCol = validMoves[i].EndCol;
                return (pieceRow, pieceCol, newRow, newCol);
            }
        }

        // return all -1's if no safe move can be made
        return (-1, -1, -1, -1);
    }

    public (int, int, int, int) DetermineNextMove()
    {
        var allMoves = new List<(int startRow, int startCol, int endRow, int endCol)>();
        var allAttacks = new List<(int startRow, int startCol, int endRow, int endCol, int score)>();

        // get all AI possible moves/attacks
        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                if (IsOccupied(row, col))
                {
                    if (!IsEnemy(row, col))
                    {
                        var (moves, attacks) = GameBoard[row, col]!.GetValidMoves(this);

                        allAttacks.AddRange(attacks);
                        allMoves.AddRange(moves);
                    }
                }
            }
        }

        // check if we have a piece in danger (get the highest score piece)
        // if it is safe to do so, and possible, take the piece putting them in danger
        // if it is a good trade off, and is possible, take the piece putting them in danger
        // otherwise retreat

        // TODO -> if any piece can safely take the attacking piece, do it

        (int heighestDangerScore, int dangerRow, int dangerCol) = HighestPieceInDanger(this);
        Console.WriteLine("Highest score in danger is");
        Console.WriteLine(heighestDangerScore);

        if (heighestDangerScore != -1)
        {
            (int safeStartRow, int safeStartCol, int safeEndRow, int safeEndCol) = DetermineSafetyMove(dangerRow, dangerCol);
            if (safeStartRow != -1)
            {
                Console.WriteLine("Safety move has been made");
                return (safeStartRow, safeStartCol, safeEndRow, safeEndCol);
            }
        }

        // if no piece is in danger, determine if a safe attack can be made
        // make the safe attack with the heighest score
        // if no safe attack can be made, make the heighest scoring danger attack
        // provided target score is heigher than team score
        (
            int heighestAttScore,
            int heighestAttStartRow,
            int heighestAttStartCol,
            int heighestAttEndRow,
            int heighestAttEndCol
        ) = (-1, -1, -1, -1, -1);
        (
            int heighestAttSafeScore,
            int heighestAttSafeStartRow,
            int heighestAttSafeStartCol,
            int heighestAttSafeEndRow,
            int heighestAttSafeEndCol
        ) = (-1, -1, -1, -1, -1);
        if (allAttacks.Count > 0)
        {
            for (int i = 0; i < allAttacks.Count; i++)
            {
                // test if safe attack can be made
                int attackStartRow = allAttacks[i].startRow;
                int attackStartCol = allAttacks[i].startCol;
                int attackEndRow = allAttacks[i].endRow;
                int attackEndCol = allAttacks[i].endCol;

                Board testBoard = CopyBoard();
                testBoard.GameBoard[attackEndRow, attackStartCol] = testBoard.GameBoard[attackStartRow, attackStartCol];
                testBoard.GameBoard[attackStartRow, attackStartCol] = null;

                testBoard.GameBoard[attackEndRow, attackEndCol]!.RowPos = attackEndRow;
                testBoard.GameBoard[attackEndRow, attackEndCol]!.ColPos = attackEndCol;

                if (testBoard.IsPosSafe(attackEndRow, attackEndCol))
                {
                    heighestAttSafeScore = GameBoard[attackEndRow, attackEndCol]!.Score;
                    heighestAttSafeStartRow = attackStartRow;
                    heighestAttSafeStartCol = attackStartCol;
                    heighestAttSafeEndRow = attackEndRow;
                    heighestAttSafeEndCol = attackEndCol;
                }
                else
                {
                    heighestAttScore = GameBoard[attackEndRow, attackEndCol]!.Score;
                    heighestAttStartRow = attackStartRow;
                    heighestAttStartCol = attackStartCol;
                    heighestAttEndRow = attackEndRow;
                    heighestAttEndCol = attackEndCol;
                }
            }

            // if no safe attack can be made
            // check if an attack with a higher trade off can be made
            if (heighestAttSafeScore == -1)
            {
                if (heighestAttScore != -1)
                {
                    if (heighestAttScore > GameBoard[heighestAttStartRow, heighestAttStartCol]!.Score)
                    {
                        Console.WriteLine("Attack with a heigher trade of can be made");
                        return (heighestAttStartRow, heighestAttStartCol, heighestAttEndRow, heighestAttEndCol);
                    }
                }
            }
            // if a safe attack can be made, make it
            else if (heighestAttSafeScore != -1)
            {
                Console.WriteLine("Safe attack has been made");
                return (heighestAttSafeStartRow, heighestAttSafeStartCol, heighestAttSafeEndRow, heighestAttSafeEndCol);
            }
        }


        // if all checks have passed
        // create a safe move list, then select a random safe move from the list
        var safeMoves = new List<(int startRow, int startCol, int endRow, int endCol)>();
        for (int i = 0; i < allMoves.Count; i++)
        {
            Board testBoard = CopyBoard();
            testBoard.GameBoard[allMoves[i].endRow, allMoves[i].endCol] = testBoard.GameBoard[allMoves[i].startRow, allMoves[i].startCol];
            testBoard.GameBoard[allMoves[i].startRow, allMoves[i].startCol] = null;

            testBoard.GameBoard[allMoves[i].endRow, allMoves[i].endCol]!.RowPos = allMoves[i].endRow;
            testBoard.GameBoard[allMoves[i].endRow, allMoves[i].endCol]!.ColPos = allMoves[i].endCol;

            if (testBoard.IsPosSafe(allMoves[i].endRow, allMoves[i].endCol))
            {
                safeMoves.Add((allMoves[i].startRow, allMoves[i].startCol, allMoves[i].endRow, allMoves[i].endCol));
            }
        }

        if (safeMoves.Count > 0)
        {
            int upperLimit = safeMoves.Count;
            Random r = new();
            int rMove = r.Next(0, upperLimit);

            Console.WriteLine("Random safe move made");
            return (safeMoves[rMove].startRow, safeMoves[rMove].startCol, safeMoves[rMove].endRow, safeMoves[rMove].endCol);
        }

        // if no safe move can be made, move the lowest scoring piece
        int lowestScore = 999;
        int lastStandStartRow = -1;
        int lastStandStartCol = -1;
        int lastStandEndRow = -1;
        int lastStandEndCol = -1;

        for (int i = 0; i < allMoves.Count; i++)
        {
            if (GameBoard[allMoves[i].startRow, allMoves[i].startCol]!.Score < lowestScore)
            {
                lowestScore = GameBoard[allMoves[i].startRow, allMoves[i].startCol]!.Score;
                lastStandStartRow = allMoves[i].startRow;
                lastStandStartCol = allMoves[i].startCol;
                lastStandEndRow = allMoves[i].endRow;
                lastStandEndCol = allMoves[i].endCol;
            }
        }

        Console.WriteLine("Last stand move has been made");
        return (lastStandStartRow, lastStandStartCol, lastStandEndRow, lastStandEndCol);
    }

    public bool IsKingInCheck(string kingColor)
    {
        (int kingRow, int kingCol) = KingPos(kingColor);

        if (kingColor == "WHITE")
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (IsOccupied(row, col))
                    {
                        if (!IsEnemy(row, col))
                        {
                            var (_, attacks) = GameBoard[row, col]!.GetValidMoves(this);
                            for (int i = 0; i < attacks.Count; i++)
                            {
                                if (attacks[i].EndRow == kingRow && attacks[i].EndCol == kingCol)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    if (IsOccupied(row, col))
                    {
                        if (IsEnemy(row, col))
                        {
                            var attacks = GameBoard[row, col]!.GetEnemyAttacks(this);

                            for (int i = 0; i < attacks.Count; i++)
                            {
                                if (attacks[i].EndRow == kingRow && attacks[i].EndCol == kingCol)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    public bool IsKingInCheckMate(string kingColour)
    {
        // NEED TO ALSO CHECK IF ANOTHER PIECE CAN TAKE THE ATTACKING PIECE
        // ^^ IF THERE IS ONLY ONE ATTACKING PIECE

        (int kingRow, int kingCol) = KingPos(kingColour);

        (int, int)[] offsets = new (int, int)[]
        {
            (0, 1), (0, -1), (1, 0), (-1, 0), (1, 1), (-1, 1), (1, -1), (-1, -1)
        };

        // current logic works (check if king can be moved safely)
        // -> also need to check if piece on white team can take the attacking piece
        if (kingColour == "WHITE")
        {
            foreach (var offset in offsets)
            {
                bool isPosSafe = true;
                int newRow = kingRow + offset.Item1;
                int newCol = kingCol + offset.Item2;

                if (newRow < 8 && newRow >= 0 && newCol < 8 && newCol >= 0)
                {
                    Board testBoard = CopyBoard();
                    if (!testBoard.IsOccupied(newRow, newCol) || !testBoard.IsEnemy(newRow, newCol))
                    {
                        testBoard.GameBoard[newRow, newCol] = testBoard.GameBoard[kingRow, kingCol];
                        testBoard.GameBoard[kingRow, kingCol] = null;

                        testBoard.GameBoard[newRow, newCol]!.RowPos = newRow;
                        testBoard.GameBoard[newRow, newCol]!.ColPos = newCol;

                        for (int row = 0; row < 8; row++)
                        {
                            for (int col = 0; col < 8; col++)
                            {
                                if (testBoard.IsOccupied(row, col))
                                {
                                    if (!testBoard.IsEnemy(row, col))
                                    {

                                        var (_, attacks) = testBoard.GameBoard[row, col]!.GetValidMoves(testBoard);
                                        for (int i = 0; i < attacks.Count; i++)
                                        {
                                            if (attacks[i].EndRow == newRow && attacks[i].EndCol == newCol)
                                            {
                                                isPosSafe = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (isPosSafe)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        else if (kingColour == "BLACK")
        {
            // black checkmate logic here
        }


        return true;
    }
}