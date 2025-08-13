using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SMS.Contracts.Authentication;
using SMS.Contracts.Users;
using AutoMapper;
using SMS.ServiceDefaults;

namespace SMS.Microservices.User.Endpoints;

public class AuthEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/register", Register)
            .WithName("RegisterUser")
            .AllowAnonymous();

        app.MapPost("/api/account/login", Login)
            .WithName("LoginUser")
            .AllowAnonymous();
    }

    public static async Task<IResult> Register(
        [FromBody] CreateUserRequest request,
        UserManager<IdentityUser> userManager,
        IMapper mapper)
    {
        if (!Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionaryExtensions.IsValid(new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())) // Simplified validation check
        {
            return Results.BadRequest(request); // Return request for now, proper validation will be handled by FluentValidation
        }

        var user = new IdentityUser { UserName = request.Username, Email = request.Username };
        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            // Assign role
            // TODO: Implement role assignment based on request.RoleId
            // For now, just add to a default role or no role
            await userManager.AddToRoleAsync(user, "User"); // Example: Add to 'User' role

            return Results.Ok(new { Message = "User registered successfully." });
        }

        foreach (var error in result.Errors)
        {
            // ModelState.AddModelError(string.Empty, error.Description); // Not directly applicable in Minimal APIs this way
        }
        return Results.BadRequest(result.Errors.Select(e => e.Description));
    }

    public static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IMapper mapper)
    {
        if (!Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionaryExtensions.IsValid(new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary())) // Simplified validation check
        {
            return Results.BadRequest(request); // Return request for now, proper validation will be handled by FluentValidation
        }

        var result = await signInManager.PasswordSignInAsync(request.Username, request.Password, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var user = await userManager.FindByNameAsync(request.Username);
            var roles = await userManager.GetRolesAsync(user);

            // TODO: Generate JWT token
            var token = ""; // Placeholder for JWT token

            var userResponse = mapper.Map<UserResponse>(user);
            userResponse.RoleName = roles.FirstOrDefault(); // Example: just take the first role

            return Results.Ok(new AuthResponse { Token = token, ExpiresAt = DateTime.UtcNow.AddHours(1), User = userResponse });
        }

        return Results.BadRequest("Invalid login attempt.");
    }
}
