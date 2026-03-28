using FastEndpoints;
using FluentValidation;

namespace CarStockAPI.Features.Cars.Add;

/// <summary>
/// Validator for the add car endpoint, ensuring that the incoming request contains valid data.
/// </summary>
public class AddCarValidator : Validator<AddCarRequest>
{
    /// <summary>
    /// Configures the validation rules for the add car request.
    /// </summary>
    public AddCarValidator()
    {
        RuleFor(x => x.Make)
            .NotEmpty().WithMessage("Make is required.")
            .MaximumLength(50).WithMessage("Make must not exceed 50 characters.");

        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Model is required.")
            .MaximumLength(50).WithMessage("Model must not exceed 50 characters.");

        RuleFor(x => x.Year)
            .GreaterThan(1885).WithMessage("Year must be greater than 1885.")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage($"Year must be less than or equal to {DateTime.Now.Year}.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock must be a non-negative integer.");
    }
}