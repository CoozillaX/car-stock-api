using FastEndpoints;
using FluentValidation;

namespace car_stock_api.Features.Auth.Login;

/// <summary>
/// Validator for the login endpoint, ensuring that the incoming request contains valid data.
/// </summary>
public class LoginValidator : Validator<LoginRequest>
{
    /// <summary>
    /// Configures the validation rules for the login request.
    /// </summary>
    public LoginValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(4).WithMessage("Username must be at least 4 characters long.")
            .MaximumLength(64).WithMessage("Username must not exceed 64 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(256).WithMessage("Password must not exceed 256 characters.");
    }
}