using FluentValidation;
using Product.Application.Handlers.Commands.BaseCommand;

namespace Product.Application.Validators;

public class StockAndIdValidator<TCommand, TResponse> : AbstractValidator<TCommand>
    where TCommand : BaseCommandWithId<TResponse>
{
    public StockAndIdValidator()
    {
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock must be >= 0");

        RuleFor(x => x.Id).NotEmpty();
    }
}
