using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SMS.Contracts.Library;
using SMS.Microservices.Library.Data;
using SMS.Microservices.Library.Models;
using SMS.ServiceDefaults;

namespace SMS.Microservices.Library.Endpoints;

public class LibraryEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/library/books", async (LibraryDbContext db, IMapper mapper) =>
        {
            return await db.Books.Select(b => mapper.Map<BookResponse>(b)).ToListAsync();
        });

        app.MapGet("/api/library/books/{id}", async (Guid id, LibraryDbContext db, IMapper mapper) =>
        {
            var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
            return book == null ? Results.NotFound() : Results.Ok(mapper.Map<BookResponse>(book));
        });

        app.MapPost("/api/library/books", async (CreateBookRequest request, LibraryDbContext db, IMapper mapper) =>
        {
            var book = mapper.Map<Book>(request);
            book.Id = Guid.NewGuid();
            book.CreatedAt = DateTime.UtcNow;
            book.UpdatedAt = DateTime.UtcNow;
            // TODO: Get CreatedBy and UpdatedBy from authenticated user
            book.CreatedBy = Guid.Empty;
            book.UpdatedBy = Guid.Empty;

            db.Books.Add(book);
            await db.SaveChangesAsync();

            return Results.Created($"/api/library/books/{book.Id}", mapper.Map<BookResponse>(book));
        });

        app.MapPut("/api/library/books/{id}", async (Guid id, UpdateBookRequest request, LibraryDbContext db, IMapper mapper) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest();
            }

            var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return Results.NotFound();
            }

            mapper.Map(request, book);
            book.UpdatedAt = DateTime.UtcNow;
            // TODO: Get UpdatedBy from authenticated user
            book.UpdatedBy = Guid.Empty;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/api/library/books/{id}", async (Guid id, LibraryDbContext db) =>
        {
            var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return Results.NotFound();
            }

            db.Books.Remove(book);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        // Book Loans Endpoints
        app.MapGet("/api/library/bookloans", async (LibraryDbContext db, IMapper mapper) =>
        {
            return await db.BookLoans.Select(bl => mapper.Map<BookLoanResponse>(bl)).ToListAsync();
        });

        app.MapGet("/api/library/bookloans/{id}", async (Guid id, LibraryDbContext db, IMapper mapper) =>
        {
            var bookLoan = await db.BookLoans.FirstOrDefaultAsync(bl => bl.Id == id);
            return bookLoan == null ? Results.NotFound() : Results.Ok(mapper.Map<BookLoanResponse>(bookLoan));
        });

        app.MapPost("/api/library/bookloans", async (CreateBookLoanRequest request, LibraryDbContext db, IMapper mapper) =>
        {
            var bookLoan = mapper.Map<BookLoan>(request);
            bookLoan.Id = Guid.NewGuid();
            bookLoan.LoanDate = DateTime.UtcNow;
            bookLoan.CreatedAt = DateTime.UtcNow;
            bookLoan.UpdatedAt = DateTime.UtcNow;
            // TODO: Get CreatedBy and UpdatedBy from authenticated user
            bookLoan.CreatedBy = Guid.Empty;
            bookLoan.UpdatedBy = Guid.Empty;

            db.BookLoans.Add(bookLoan);
            await db.SaveChangesAsync();

            return Results.Created($"/api/library/bookloans/{bookLoan.Id}", mapper.Map<BookLoanResponse>(bookLoan));
        });

        app.MapPut("/api/library/bookloans/{id}", async (Guid id, UpdateBookLoanRequest request, LibraryDbContext db, IMapper mapper) =>
        {
            if (id != request.Id)
            {
                return Results.BadRequest();
            }

            var bookLoan = await db.BookLoans.FirstOrDefaultAsync(bl => bl.Id == id);
            if (bookLoan == null)
            {
                return Results.NotFound();
            }

            mapper.Map(request, bookLoan);
            bookLoan.UpdatedAt = DateTime.UtcNow;
            // TODO: Get UpdatedBy from authenticated user
            bookLoan.UpdatedBy = Guid.Empty;

            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapDelete("/api/library/bookloans/{id}", async (Guid id, LibraryDbContext db) =>
        {
            var bookLoan = await db.BookLoans.FirstOrDefaultAsync(bl => bl.Id == id);
            if (bookLoan == null)
            {
                return Results.NotFound();
            }

            db.BookLoans.Remove(bookLoan);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}



