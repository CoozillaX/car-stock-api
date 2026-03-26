using FastEndpoints;
using FluentValidation;

namespace car_stock_api.Features.Cars.Search;

/// <summary>
/// Validator for the search cars endpoint, ensuring that the incoming request contains valid data.
/// </summary>
public class SearchCarsValidator : Validator<SearchCarRequest>
{
    /// <summary>
    /// Configures the validation rules for the search cars request.
    /// </summary>
    public SearchCarsValidator()
    {
        RuleFor(x => x.Make)
            .MaximumLength(50).WithMessage("Make must not exceed 50 characters.");

        RuleFor(x => x.Model)
            .MaximumLength(50).WithMessage("Model must not exceed 50 characters.");
    }
}