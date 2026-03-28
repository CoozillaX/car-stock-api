using FastEndpoints;
using FluentValidation;

namespace CarStockAPI.Features.Cars.Delete;

/// <summary>
/// Validator for the delete car endpoint, ensuring that the incoming request contains valid data.
/// </summary>
public class DeleteCarValidator : Validator<DeleteCarRequest>
{
    /// <summary>
    /// Configures the validation rules for the delete car request.
    /// </summary>
    public DeleteCarValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Car ID must be a positive integer.");
    }
}