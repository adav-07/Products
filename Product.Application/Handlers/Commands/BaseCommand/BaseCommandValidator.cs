using FluentValidation;

namespace Product.Application.Handlers.Commands.BaseCommand;

public abstract class BaseCommandValidator<T1, T2> : AbstractValidator<T1> where T1 : BaseCommand<T2>
{
    protected BaseCommandValidator()
    {
        //RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock Must Be >= 0");
    }
}