using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<BookDb>(opt => opt.UseInMemoryDatabase("BookList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Book Reservation API",
            Description = "An ASP.NET Core Web API for managing Book reservations",
        });
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}


var bookItems = app.MapGroup("/bookitems");

bookItems.MapGet("/", GetAllBooks);
bookItems.MapGet("/reserved", GetBooksReserved);
bookItems.MapGet("/available", GetBooksAvailable);
bookItems.MapGet("/{id}", GetBook);
bookItems.MapPost("/", CreateBook);
bookItems.MapPost("/{id}/reserve", ReserveBook);
bookItems.MapPost("/{id}/remove-reserve", RemoveReserveBook);
bookItems.MapPut("/{id}", UpdateBook);
bookItems.MapDelete("/{id}", DeleteTodo);

app.Run();

// GET endpoint for all the books reserved/not reserved
static async Task<IResult> GetAllBooks(BookDb db)
{
    return TypedResults.Ok(await db.Books.ToArrayAsync());
}

// GET endpoint for all the books that are reserved.
static async Task<IResult> GetBooksReserved(BookDb db)
{
    return TypedResults.Ok(await db.Books.Where(t => t.Reserved == true).ToListAsync());
}

// GET endpoint for all the books that are Available.
static async Task<IResult> GetBooksAvailable(BookDb db)
{
    return TypedResults.Ok(await db.Books.Where(t => t.Reserved == false).ToListAsync());
}

// GET endpoint for a specific book details.
static async Task<IResult> GetBook(int id, BookDb db)
{
    return await db.Books.FindAsync(id)
        is Book book
            ? Results.Ok(book)
            : Results.NotFound("Book not found");
}


// POST endpoint for creating a book 

static async Task<IResult> CreateBook(Book todo, BookDb db)
{
    db.Books.Add(todo);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/bookitems/{todo.BookId}", todo);
}


// POST endpoint to reserve a book
static async Task<IResult> ReserveBook(int id, string comment, BookDb db)
{
    var book = await db.Books.FindAsync(id);

    if (book is null) return TypedResults.NotFound("Book not found");

    if (book.Reserved) return TypedResults.BadRequest("Book already reserved");
    
    book.Reserved = true;
    book.Comment = comment; // Storing reservation comment
    
    await db.SaveChangesAsync();

    return TypedResults.Created($"/bookitems/{book.BookId}", book);
}


// POST endpoint to remove a status of a book
static async Task<IResult> RemoveReserveBook(int id, string comment, BookDb db)
{
    var book = await db.Books.FindAsync(id);

    if (book is null) return TypedResults.NotFound("Book not found");

    if (book.Reserved) return TypedResults.BadRequest("Book available to reserve");

    book.Reserved = false;
    book.Comment = comment; // Storing reservation comment

    await db.SaveChangesAsync();

    return TypedResults.Created($"/bookitems/{book.BookId}", book);
}


// PUT endpoint to update details of a book
static async Task<IResult> UpdateBook(int id, Book inputBook, BookDb db)
{
    var book = await db.Books.FindAsync(id);

    if (book is null) return Results.NotFound("Book not found");

    book.Title = inputBook.Title;
    book.Comment = inputBook.Comment;
    book.Reserved = inputBook.Reserved;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

// DELETE endpoint to delete a book using its ID
static async Task<IResult> DeleteTodo(int id, BookDb db)
{
    if (await db.Books.FindAsync(id) is Book book)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return TypedResults.NotFound("Book not found");
}



/**
 * CODE APPENDIX
 * The code when using Results to return endpoints
 * 
 * app.MapGet("/bookitems", async (BookDb db) =>
    await db.Books.ToListAsync());

app.MapGet("/bookitems/reserved", async (BookDb db) =>
    await db.Books.Where(t => t.Reserved == true).ToListAsync());

app.MapGet("/bookitems/{id}", async (int id, BookDb db) =>
    await db.Books.FindAsync(id)
        is Book book
            ? Results.Ok(book)
            : Results.NotFound("Book not found"));

app.MapPost("/bookitems", async (Book book, BookDb db) =>
{
    db.Books.Add(book);
    await db.SaveChangesAsync();

    return Results.Created($"/bookitems/{book.BookId}", book);
});

app.MapPost("/bookitems/{id}/reserve", async (int id, string comment, BookDb db) =>
{
    var book = await db.Books.FindAsync(id);

    if (book == null)
    {
        return Results.NotFound("Book not found");
    }

    if (book.Reserved)
    {
        return Results.BadRequest("Book already reserved");
    }

    book.Reserved = true;
    book.Comment = comment; // Storing reservation comment
    await db.SaveChangesAsync();

    return Results.Created($"/bookitems/{book.BookId}", book);
});

app.MapPost("/bookitems/{id}/remove-reserve", async (int id, BookDb db) =>
{
    var book = await db.Books.FindAsync(id);

    if (book == null)
    {
        return Results.NotFound("Book not found");
    }

    if (book.Reserved == false)
    {
        return Results.BadRequest("Book already available to reserve");
    }

    book.Reserved = false;
    await db.SaveChangesAsync();

    return Results.Created($"/bookitems/{book.BookId}", book);
});

app.MapPut("/bookitems/{id}", async (int id, Book inputBook, BookDb db) =>
{
    var book = await db.Books.FindAsync(id);

    if (book is null) return Results.NotFound("Book not found");

    book.Title = inputBook.Title;
    book.Comment = inputBook.Comment;
    book.Reserved = inputBook.Reserved;

    await db.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/bookitems/{id}", async (int id, BookDb db) =>
{
    if (await db.Books.FindAsync(id) is Book book)
    {
        db.Books.Remove(book);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound("Book not found");
});

 **/