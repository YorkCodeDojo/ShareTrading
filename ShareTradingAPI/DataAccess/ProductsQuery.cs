using System.Threading.Tasks;
using ShareTradingAPI.ViewModels;

namespace ShareTradingAPI.DataAccess
{
    public class ProductsQuery : IProductsQuery
    {
        public Task<Product[]> Evaluate()
        {
            return Task.FromResult(new Product[4]
            {
                new Product() { ProductCode= "ProductA", Description= "You can invest here,  but not much happens"},
                new Product() { ProductCode= "ProductB", Description= "Invest in this Maths based company"},
                new Product() { ProductCode= "ProductC", Description= "This is where all the cool kids are investing."},
                new Product() { ProductCode= "ProductD", Description= "If you want to play it safe,  then this is the place for your money."}
            });
        }
    }
}
