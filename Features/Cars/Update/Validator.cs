using FastEndpoints;
using FluentValidation;

namespace car_stock_api.Features.Cars.Update;

/// <summary>
/// Validator for the update car endpoint, ensuring that the incoming request contains valid data.
/// </summary>
public class UpdateCarValidator : Validator<UpdateCarRequest>
{
    /// <summary>
    /// Configures the validation rules for the update car request.
    /// </summary>
    public UpdateCarValidator()
    {
        RuleFor(x => x.Make)
            .MaximumLength(50).WithMessage("Make must not exceed 50 characters.");

        RuleFor(x => x.Model)
            .MaximumLength(50).WithMessage("Model must not exceed 50 characters.");

        RuleFor(x => x.Year)
            .GreaterThan(1885).WithMessage("Year must be greater than 1885.")
            .LessThanOrEqualTo(DateTime.Now.Year).WithMessage($"Year must be less than or equal to {DateTime.Now.Year}.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Stock must be a non-negative integer.");
    }
}