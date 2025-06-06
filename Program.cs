using Microsoft.Extensions.ObjectPool;
using MyBackend.Game;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowLocalhost3000");

Dictionary<string, int> scores = new Dictionary<string, int>
{
    { "Alice", 95 },
    { "Bob", 87 },
    { "Charlie", 78 }
};

app.MapGet("/hello", () =>
{
    return Results.Ok(new { message = "Hello, from backend" });
});

app.MapPost("/score", (NameRequest request) =>
{
    if (scores.TryGetValue(request.Name, out int score))
    {
        return Results.Ok(new { Name = request.Name, Score = score });
    }
    else
    {
        return Results.NotFound($"No score found for {request.Name}");
    }
});

app.MapPost("/user-move", (MoveRequest request) =>
{
    var board = request.Chessboard;

    Board gameBoard = new();
    gameBoard.ProcessBoard(board!);
    (int startRow, int startCol, int endRow, int endCol) = gameBoard.DetermineNextMove();

    return Results.Ok(new { message = "Board state has been receieved - from backend... determining validity", startRow = startRow, startCol = startCol, endRow = endRow, endCol = endCol });
});

app.MapPost("/check-win-condition", (MoveRequest request) =>
{
    var board = request.Chessboard;

    Board gameBoard = new();
    gameBoard.ProcessBoard(board!);
    // check win condition here

    // first, test if one of the kings are in check
    var whiteKingInCheck = gameBoard.IsKingInCheck("WHITE");

    if (whiteKingInCheck)
    {
        // need to also check for check mate here, only return check if not in check mate
        var checkMate = gameBoard.IsKingInCheckMate("WHITE");

        if (checkMate)
        {
            return Results.Ok(new { check = false, checkMate = true, colorInCheckMate = "WHITE" });
        }

        return Results.Ok(new { check = true, checkMate = false, colorInCheck = "WHITE" });
    }

    var blackKingInCheck = gameBoard.IsKingInCheck("BLACK");

    if (blackKingInCheck)
    {
        // need to also check for check mate here, only return check if not in check mate
        var checkMate = gameBoard.IsKingInCheckMate("BLACK");

        if (checkMate)
        {
            return Results.Ok(new { check = false, checkMate = true, colorInCheckMate = "BLACK" });
        }

        return Results.Ok(new { check = true, checkMate = false, colorInCheck = "BLACK" });
    }

    return Results.Ok(new { check = false, checkMate = false });
});

app.MapGet("/", () => "Hello World!");



app.Run();

record NameRequest(string Name);
record BoardState(string[][] Array);




// example endpoints

// app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
//     await db.Todos.FindAsync(id)
//         is Todo todo
//             ? Results.Ok(todo)
//             : Results.NotFound());

// app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
// {
//     db.Todos.Add(todo);
//     await db.SaveChangesAsync();

//     return Results.Created($"/todoitems/{todo.Id}", todo);
// });

// app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
// {
//     var todo = await db.Todos.FindAsync(id);

//     if (todo is null) return Results.NotFound();

//     todo.Name = inputTodo.Name;
//     todo.IsComplete = inputTodo.IsComplete;

//     await db.SaveChangesAsync();

//     return Results.NoContent();
// });

// app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
// {
//     if (await db.Todos.FindAsync(id) is Todo todo)
//     {
//         db.Todos.Remove(todo);
//         await db.SaveChangesAsync();
//         return Results.NoContent();
//     }

//     return Results.NotFound();
// });