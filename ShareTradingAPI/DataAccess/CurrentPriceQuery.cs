using ShareTradingAPI.Pricing;
using System.Threading.Tasks;

namespace ShareTradingAPI.DataAccess
{
    public class CurrentPriceQuery : ICurrentPriceQuery
    {
        public static class ErrorConditions
        {
            public const int ProductDoesNotExist = -1;
            public const int PriceDoesNotExist = -2;
        }

        readonly Pricing.ProductA _productA;
        readonly Pricing.ProductB _productB;
        readonly Pricing.ProductC _productC;
        readonly Pricing.ProductD _productD;
        readonly ITimeCalculator _timeCalculator;

        public CurrentPriceQuery(Pricing.ProductA productA,
                                 Pricing.ProductB productB,
                                 Pricing.ProductC productC,
                                 Pricing.ProductD productD,
                                 ITimeCalculator timeCalculator)
        {
            _productA = productA;
            _productB = productB;
            _productC = productC;
            _productD = productD;
            _timeCalculator = timeCalculator;
        }

        public Task<int> Evaluate(string productCode)
        {
            var seconds = _timeCalculator.Evaluate();

            switch (productCode)
            {
                case "ProductA":
                    return Task.FromResult(_productA.Evaluate(seconds));

                case "ProductB":
                    return Task.FromResult(_productB.Evaluate(seconds));

                case "ProductC":
                    return Task.FromResult(_productC.Evaluate(seconds));

                case "ProductD":
                    return Task.FromResult(_productD.Evaluate(seconds));

                default:
                    return Task.FromResult(ErrorConditions.ProductDoesNotExist);
            }

        }

    }
}
