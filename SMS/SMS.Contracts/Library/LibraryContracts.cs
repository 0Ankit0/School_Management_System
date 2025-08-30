using FluentValidation;
using System;
using System.Collections.Generic;

namespace SMS.Contracts.Library;

public class CreateBookRequest
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public DateTime PublishedDate { get; set; }
    public int Quantity { get; set; }
}

public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
{
    public CreateBookRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ISBN).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PublishedDate).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}

public class CreateBookLoanRequest
{
    public Guid BookExternalId { get; set; }
    public Guid StudentExternalId { get; set; }
    public DateTime DueDate { get; set; }
}

public class CreateBookLoanRequestValidator : AbstractValidator<CreateBookLoanRequest>
{
    public CreateBookLoanRequestValidator()
    {
        RuleFor(x => x.BookExternalId).NotEmpty();
        RuleFor(x => x.StudentExternalId).NotEmpty();
        RuleFor(x => x.DueDate).NotEmpty();
    }
}

public class BookResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public DateTime PublishedDate { get; set; }
    public int Quantity { get; set; }
}

public class BookLoanResponse
{
    public Guid Id { get; set; } // Maps to ExternalId
    public Guid BookExternalId { get; set; }
    public Guid StudentExternalId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }
}

public class UpdateBookRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public DateTime PublishedDate { get; set; }
    public int Quantity { get; set; }
}

public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
{
    public UpdateBookRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Author).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ISBN).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PublishedDate).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}

public class UpdateBookLoanRequest
{
    public Guid Id { get; set; }
    public Guid BookExternalId { get; set; }
    public Guid StudentExternalId { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public DateTime DueDate { get; set; }
}

public class UpdateBookLoanRequestValidator : AbstractValidator<UpdateBookLoanRequest>
{
    public UpdateBookLoanRequestValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.BookExternalId).NotEmpty();
        RuleFor(x => x.StudentExternalId).NotEmpty();
        RuleFor(x => x.LoanDate).NotEmpty();
        RuleFor(x => x.DueDate).NotEmpty();
    }
}
