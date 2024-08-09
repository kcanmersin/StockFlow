using FluentValidation;

namespace Core.Features.BuyStock
{
    public class BuyStockValidator : AbstractValidator<BuyStockCommand>
    {
        public BuyStockValidator()
        {
            RuleFor(c => c.UserId).NotEmpty().WithMessage("User ID is required.");
            RuleFor(c => c.StockSymbol)
                .NotEmpty().WithMessage("Stock symbol is required.")
                .Length(1, 5).WithMessage("Stock symbol must be between 1 and 5 characters.");
            RuleFor(c => c.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}
