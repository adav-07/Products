using FluentValidation;
using Product.Application.Handlers.Commands.BaseCommand;

namespace Product.Application.Validators;

public class StockValidator<T1, T2> : BaseCommandValidator<T1, T2>
    where T1 : BaseCommand<T2>
{
    public StockValidator()
    {
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock Must Be >= 0");
    }
}
