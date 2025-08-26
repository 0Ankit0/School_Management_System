using FluentValidation;
using System;
using SMS.Contracts.Users;

namespace SMS.Contracts.Authentication;

public class LoginRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}

public class AuthResponse
{
    public string? Token { get; set; }
    public DateTime ExpiresAt { get; set; }
    public UserResponse? User { get; set; }
}
